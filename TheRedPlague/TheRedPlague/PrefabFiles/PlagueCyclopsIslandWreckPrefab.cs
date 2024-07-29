using Nautilus.Assets;
using Nautilus.Assets.Gadgets;
using UnityEngine;
using System.Collections;
using Nautilus.Utility;
using TheRedPlague.Mono;
using TheRedPlague.Mono.PlagueCyclops;
using TheRedPlague.PrefabFiles.UpgradeModules;
using UWE;

namespace TheRedPlague.PrefabFiles;

public class PlagueCyclopsIslandWreckPrefab
{
    public static PrefabInfo Info { get; } = PrefabInfo.WithTechType("PlagueCyclopsIslandWreck");

    private static GameObject _cyclopsReference;

    private static bool _loaded;

    private static readonly string[] InfectedChildren = new[]
    {
        "cyclops_damaged_LOD0/cyclops_control_room (1)",
        "cyclops_damaged_LOD0/cyclops_diving_chamber (1)",
        "cyclops_damaged_LOD0/cyclops_ladder_hallway (1)",
        "cyclops_damaged_LOD0/cyclops_main_room (1)",
        "cyclops_damaged_LOD0/lower_engine_room (1)",
        "cyclops_damaged_LOD0/Submarine_Steering_Console (1)",
        "cyclops_damaged_LOD0/cyclops_launch_bay",
        "cyclops_damaged_LOD0/Engine_room/cyclops_engine_wire",
        "cyclops_damaged_LOD0/Engine_room/engine_room_pipes",
        "cyclops_damaged_LOD0/Engine_room/Engine_room_walls",
        "cyclops_damaged_LOD0/Engine_room/submarine_engine_01_base",
        "cyclops_damaged_LOD0/Engine_room/submarine_engine_01_spin",
    };

    public static void Register()
    {
        var prefab = new CustomPrefab(Info);
        prefab.SetGameObject(GetPrefab);
        prefab.SetSpawns(new SpawnLocation(new Vector3(-171.581f, -810.000f, 341.000f), new Vector3(0, 188.571f, 68.571f)));
        prefab.Register();
    }

    private static IEnumerator GetPrefab(IOut<GameObject> result)
    {
        if (_cyclopsReference == null)
        {
            _loaded = false;

            yield return new WaitUntil(() => LightmappedPrefabs.main);

            LightmappedPrefabs.main.RequestScenePrefab("Cyclops", OnSubPrefabLoaded);

            yield return new WaitUntil(() => _loaded);
        }

        var damagedCyclops = Object.Instantiate(_cyclopsReference.transform.Find("CyclopsMeshStatic/damaged").gameObject);
        damagedCyclops.SetActive(false);
        PrefabUtils.AddBasicComponents(damagedCyclops, Info.ClassID, Info.TechType, LargeWorldEntity.CellLevel.Far);

        foreach (var infectedChild in InfectedChildren)
        {
            damagedCyclops.transform.Find(infectedChild).gameObject.AddComponent<InfectAnything>().infectionHeightStrength = -0.5f;
        }

        var rb = damagedCyclops.AddComponent<Rigidbody>();
        rb.mass = 10000;
        rb.isKinematic = true;
        rb.useGravity = false;
        
        var wf = damagedCyclops.AddComponent<WorldForces>();
        wf.useRigidbody = rb;

        var fall = damagedCyclops.AddComponent<PlagueCyclopsWreckFall>();
        fall.rigidbody = rb;

        var corePrefabTask = CraftData.GetPrefabForTechTypeAsync(PlagueCyclopsCore.Info.TechType);
        yield return corePrefabTask;
        var core = Object.Instantiate(corePrefabTask.GetResult(), damagedCyclops.transform);
        core.transform.localPosition = new Vector3(-0.14f, -1.173f, 21.01f);
        core.transform.localEulerAngles = new Vector3(0, 0, 350);

        var trigger = new GameObject("WelcomeTrigger");
        trigger.transform.parent = damagedCyclops.transform;
        trigger.transform.localPosition = Vector3.forward * -5;
        trigger.transform.localScale = Vector3.one * 10;
        trigger.AddComponent<SphereCollider>().isTrigger = true;
        var voices = trigger.AddComponent<WelcomeAboardWreckTrigger>();
        var voiceEmitter = damagedCyclops.AddComponent<FMOD_CustomEmitter>();
        voiceEmitter.restartOnPlay = true;
        voiceEmitter.followParent = true;

        voices.emitter = voiceEmitter;
        voices.plagueCyclopsCore = core.GetComponent<Pickupable>();

        var screamAmbience = damagedCyclops.AddComponent<CyclopsScreamingAmbience>();
        screamAmbience.wreck = fall;

        damagedCyclops.AddComponent<ConstructionObstacle>();
        
        damagedCyclops.transform.Find("cyclops_damaged_LOD0/Engine_room/submarine_engine_console_01/engine_console_key_01_01").gameObject.SetActive(false);
        damagedCyclops.transform.Find("cyclops_damaged_LOD0/Engine_room/submarine_engine_console_01/engine_console_key_01_03").gameObject.SetActive(false);
        damagedCyclops.transform.Find("cyclops_damaged_LOD0/Engine_room/submarine_engine_console_01/engine_console_key_01_04").gameObject.SetActive(false);

        damagedCyclops.AddComponent<PlayPlagueCyclopsMusic>();

        var suckerPrefabTask = PrefabDatabase.GetPrefabAsync("Sucker");
        yield return suckerPrefabTask;
        if (suckerPrefabTask.TryGetPrefab(out var suckerPrefab))
        {
            SpawnSucker(damagedCyclops.transform, suckerPrefab, new Vector3(-0.238f, -1.246f, 20.938f),  new Vector3(345, 227.6f, 25.26f), Vector3.one);
            SpawnSucker(damagedCyclops.transform, suckerPrefab, new Vector3(-1.059f, -2.163f, 2.640f),  new Vector3(0, 221.805f, 0.000f), Vector3.one * 1.772f);
            SpawnSucker(damagedCyclops.transform, suckerPrefab, new Vector3(-1.189f, 0.529f, 3.067f),  new Vector3(3.5f, 271, 87), Vector3.one * 1.687f);
            SpawnSucker(damagedCyclops.transform, suckerPrefab, new Vector3(-0.886f, -1.105f, -1.216f),  new Vector3(89, 312, 312), Vector3.one * 3.97f);
            SpawnSucker(damagedCyclops.transform, suckerPrefab, new Vector3(-1.297f, -0.830f, 28.018f),  new Vector3(314, 270, 270), Vector3.one * 7.45f);
        }
        
        var organPrefabTask = PrefabDatabase.GetPrefabAsync("OrgansProp2");
        yield return organPrefabTask;
        if (organPrefabTask.TryGetPrefab(out var organPrefab))
        {
            var organ = Object.Instantiate(organPrefab, damagedCyclops.transform);
            organ.transform.localPosition = new Vector3(0.642f, -0.934f, 15.632f);
            organ.transform.localEulerAngles = new Vector3(12.9f, 83.8f, 7.6f);
            Object.DestroyImmediate(organ.GetComponent<LargeWorldEntity>());
        }
        
        result.Set(damagedCyclops);
    }

    private static void SpawnSucker(Transform parent, GameObject prefab, Vector3 pos, Vector3 angles, Vector3 scale)
    {
        var sucker = Object.Instantiate(prefab, parent);
        sucker.transform.localPosition = pos;
        sucker.transform.localEulerAngles = angles;
        sucker.transform.localScale = scale;
        sucker.transform.Find("BlockTriggers").gameObject.SetActive(false);
        Object.DestroyImmediate(sucker.GetComponent<LargeWorldEntity>());
    }

    private static void OnSubPrefabLoaded(GameObject prefab)
    {
        _cyclopsReference = prefab;
        _loaded = true;
    }
}