using System;
using System.Collections;
using mset;
using Nautilus.Assets;
using PdaUpgradeChips.MonoBehaviours;
using UnityEngine;
using UWE;
using Object = UnityEngine.Object;

namespace PdaUpgradeChips.Prefabs;

public class PocketDimensionPrefab
{
    private static readonly int LightmapStrength = Shader.PropertyToID("_LightmapStrength");
    public PrefabInfo Info { get; }
    public string RoomModelClassId { get; }
    public Vector3 LocalSpawnPosition { get; }
    public Func<GameObject, IEnumerator> ModifyPrefab { get; set; }

    public PocketDimensionPrefab(PrefabInfo info, string roomModelClassId, Vector3 localSpawnPosition)
    {
        Info = info;
        RoomModelClassId = roomModelClassId;
        LocalSpawnPosition = localSpawnPosition;
    }

    public void Register()
    {
        var prefab = new CustomPrefab(Info);
        prefab.SetGameObject(CreatePrefabAsync);
        prefab.Register();
    }

    private IEnumerator CreatePrefabAsync(IOut<GameObject> result)
    {
        var roomTask = PrefabDatabase.GetPrefabAsync(RoomModelClassId);
        yield return roomTask;
        if (!roomTask.TryGetPrefab(out var roomPrefab))
        {
            Plugin.Logger.LogError("Failed to load room prefab!");
            yield break;
        }

        var baseTask = PrefabDatabase.GetPrefabAsync("e9b75112-f920-45a9-97cc-838ee9b389bb");
        yield return baseTask;
        if (!baseTask.TryGetPrefab(out var basePrefab))
        {
            Plugin.Logger.LogError("Failed to load base prefab!");
            yield break;
        }

        var prefab = new GameObject(Info.ClassID);
        prefab.SetActive(false);

        var room = Object.Instantiate(roomPrefab, prefab.transform, false);
        room.transform.localScale = Vector3.one;
        room.transform.localEulerAngles = Vector3.zero;
        room.transform.localPosition = Vector3.zero;

        Object.DestroyImmediate(room.GetComponent<LargeWorldEntity>());
        Object.DestroyImmediate(room.GetComponent<PrefabIdentifier>());

        prefab.AddComponent<PrefabIdentifier>().ClassId = Info.ClassID;
        prefab.AddComponent<TechTag>().type = Info.TechType;
        prefab.AddComponent<LargeWorldEntity>().cellLevel = LargeWorldEntity.CellLevel.Global;

        var interiorSky = Object
            .Instantiate(basePrefab.transform.Find("SkyBaseInterior").gameObject, room.transform, false)
            .GetComponent<Sky>();
        var glassSky = Object.Instantiate(basePrefab.transform.Find("SkyBaseGlass").gameObject, room.transform, false)
            .GetComponent<Sky>();

        var lightControl = prefab.AddComponent<LightingController>();
        lightControl.skies = new[]
        {
            new LightingController.MultiStatesSky
            {
                sky = interiorSky,
                masterIntensities = new[] { 1.4f, 1f, 0.5f },
                diffIntensities = new[] { 7, 1.5f, 1.2f },
                specIntensities = new[] { 2, 0.6f, 0.5f },
                startDiffuseIntensity = 7,
                startMasterIntensity = 1.4f,
                startSpecIntensity = 2
            },
            new LightingController.MultiStatesSky
            {
                sky = glassSky,
                masterIntensities = new[] { 0.73f, 0.4f, 0.4f },
                diffIntensities = new[] { 1.02f, 0.5f, 0.5f },
                specIntensities = new[] { 0.44f, 0.3f, 0.3f },
                startDiffuseIntensity = 1.02f,
                startMasterIntensity = 0.73f,
                startSpecIntensity = 0.44f
            }
        };
        lightControl.emissiveController = new LightingController.MultiStatesEmissive()
        {
            intensities = new[] { 1, 0.7f, 0f }
        };
        lightControl.fadeDuration = 0.3f;

        var lod = prefab.AddComponent<BehaviourLOD>();
        lod.veryCloseThreshold = 30;
        lod.closeThreshold = 50;
        lod.farThreshold = 100;

        var subRoot = prefab.AddComponent<PocketDimensionSub>();
        subRoot.interiorSky = interiorSky;
        subRoot.glassSky = glassSky;
        subRoot.lightControl = lightControl;
        subRoot.isBase = true;
        subRoot.dimensionTechType = Info.TechType;

        var subDamageSounds = new GameObject("SubDamageSoundsDummy");
        subDamageSounds.transform.SetParent(prefab.transform, false);
        subDamageSounds.AddComponent<SubDamageSounds>();
        subRoot.subDamageSoundsPrefab = subDamageSounds;

        subRoot.LOD = lod;
        subRoot.modulesRoot = prefab.transform; 

        var oldSkyAppliers = room.GetComponentsInChildren<SkyApplier>();
        foreach (var sa in oldSkyAppliers)
        {
            Object.DestroyImmediate(sa);
        }

        var entrancePosition = new GameObject("EntrancePosition").transform;
        entrancePosition.SetParent(prefab.transform, false);
        entrancePosition.localPosition = LocalSpawnPosition;
        entrancePosition.localRotation = Quaternion.identity;
        subRoot.entrancePosition = entrancePosition;

        var basePowerRelay = basePrefab.GetComponent<PowerRelay>();
        
        var powerRelay = prefab.AddComponent<PowerRelay>();
        powerRelay.powerSystemPreviewPrefab = basePowerRelay.powerSystemPreviewPrefab;
        
        var powerCellsParent = new GameObject("PowerCellsParent").transform;
        powerCellsParent.SetParent(prefab.transform, false);
        powerCellsParent.localPosition = new Vector3(0, -15, 0);
        powerCellsParent.localEulerAngles = Vector3.zero;
        powerCellsParent.gameObject.AddComponent<ChildObjectIdentifier>().ClassId = "PocketDimensionPower";

        var placeholdersGroup = prefab.AddComponent<PrefabPlaceholdersGroup>();
        var powerCellLocations = new[] { new Vector3(-1, -1, 0),  new Vector3(1, -1, 0) };

        var placeholders = new PrefabPlaceholder[powerCellLocations.Length];
        
        for (int i = 0; i < powerCellLocations.Length; i++)
        {
            var placeholder = new GameObject("PowerCellPlaceholder");
            var placeholderComponent = placeholder.AddComponent<PrefabPlaceholder>();
            placeholderComponent.prefabClassId = "0cb22d0e-ba5e-4e4b-b7a7-a67931fb5e0c";
            placeholder.transform.SetParent(powerCellsParent, false);
            placeholder.transform.localPosition = powerCellLocations[i];
            placeholder.transform.localRotation = Quaternion.identity;
            placeholders[i] = placeholderComponent;
        }

        placeholdersGroup.prefabPlaceholders = placeholders;
        
        // MODIFY PREFAB AFTER MAIN CHANGES
        
        if (ModifyPrefab != null)
        {
            yield return ModifyPrefab(prefab);
        }
        
        // FINAL ADJUSTMENTS

        var interiorSkyApplier = room.AddComponent<SkyApplier>();
        var roomRenderers = room.GetComponentsInChildren<Renderer>();
        foreach (var renderer in roomRenderers)
        {
            var materials = renderer.materials;
            foreach (var material in materials)
            {
                material.SetFloat(LightmapStrength, 0.5f);
            }
        }

        interiorSkyApplier.renderers = roomRenderers;
        interiorSkyApplier.anchorSky = Skies.BaseInterior;
        interiorSkyApplier.lightControl = lightControl;

        result.Set(prefab);
    }

    internal static IEnumerator ModifyPocketDimensionTier1(GameObject obj)
    {
        var room = obj.transform.Find("ExplorableWreckRoom04(Clone)");
        var colliders = room.Find("explorable_wreckage_modular_room_03_collision/Cube (20)")
            .GetComponents<BoxCollider>();
        var floor = colliders[5];
        floor.center = new Vector3(floor.center.x, 0, floor.center.z);
        var backWall = colliders[7];
        backWall.center = new Vector3(backWall.center.x, backWall.center.y, -5.1f);
        var ventCoverTask = PrefabDatabase.GetPrefabAsync("235f771a-bb5a-4f58-8484-4ad9a6f4e95c");
        yield return ventCoverTask;
        if (ventCoverTask.TryGetPrefab(out var ventCoverPrefab))
        {
            var ventCover = Object.Instantiate(ventCoverPrefab, room, false);
            CleanUpPrefabComponents(ventCover);
            ventCover.transform.localPosition = new Vector3(6.2f, 3.05f, -2.4f);
            ventCover.transform.localEulerAngles = new Vector3(90, 90, 0);
        }

        var doorFrameTask = PrefabDatabase.GetPrefabAsync("055b3160-f57b-46ba-80f5-b708d0c8180e");
        yield return doorFrameTask;
        if (doorFrameTask.TryGetPrefab(out var doorFramePrefab))
        {
            var doorFrame = Object.Instantiate(doorFramePrefab, room, false);
            CleanUpPrefabComponents(doorFrame);
            doorFrame.transform.localPosition = new Vector3(-0.1f, 0.4f, 0.2f);
            doorFrame.transform.localEulerAngles = new Vector3(0, 0, 0);
            doorFrame.GetComponentInChildren<Renderer>().material.color = Color.white;
        }

        var doorTask = PrefabDatabase.GetPrefabAsync("ef1370e3-832f-4008-ac39-99ad24f43f76");
        yield return doorTask;
        if (doorTask.TryGetPrefab(out var doorPrefab))
        {
            var door = Object.Instantiate(doorPrefab, room, false);
            CleanUpPrefabComponents(door);
            door.transform.localPosition = new Vector3(-0.1f, 0.4f, 0.2f);
            door.transform.localEulerAngles = new Vector3(0, 0, 0);
            door.AddComponent<ExitDimensionDoor>();
        }
    }

    internal static IEnumerator ModifyPocketDimensionTier3(GameObject obj)
    {
        obj.GetComponent<PocketDimensionSub>().entrancePosition.localPosition = new Vector3(0, 0, -1);
        var room = obj.transform.Find("CrashedShip_cargo_room(Clone)");
        room.localEulerAngles = new Vector3(-90, 0, 0);
        room.localScale = Vector3.one * 0.4f;
        
        var doorFrameTask = PrefabDatabase.GetPrefabAsync("055b3160-f57b-46ba-80f5-b708d0c8180e");
        yield return doorFrameTask;
        if (doorFrameTask.TryGetPrefab(out var doorFramePrefab))
        {
            var doorFrame = Object.Instantiate(doorFramePrefab, room, false);
            CleanUpPrefabComponents(doorFrame);
            doorFrame.transform.localPosition = new Vector3(0.5f, 0.2f, -6.5f);
            doorFrame.transform.localEulerAngles = new Vector3(90, 0, 0);
            doorFrame.transform.localScale = Vector3.one * 2.5f;
            doorFrame.GetComponentInChildren<Renderer>().material.color = Color.white;
        }

        var doorTask = PrefabDatabase.GetPrefabAsync("ef1370e3-832f-4008-ac39-99ad24f43f76");
        yield return doorTask;
        if (doorTask.TryGetPrefab(out var doorPrefab))
        {
            var door = Object.Instantiate(doorPrefab, room, false);
            CleanUpPrefabComponents(door);
            door.transform.localPosition = new Vector3(0.5f, 0.2f, -6.5f);
            door.transform.localEulerAngles = new Vector3(90, 0, 0);
            door.transform.localScale = Vector3.one * 2.5f;
            door.AddComponent<ExitDimensionDoor>();
        }
        
        var holeCoverTask = PrefabDatabase.GetPrefabAsync("872c799a-4de2-4531-a846-3b362d666e0b");
        yield return holeCoverTask;
        if (holeCoverTask.TryGetPrefab(out var holeCoverPrefab))
        {
            var holeCover = Object.Instantiate(holeCoverPrefab, room, false);
            CleanUpPrefabComponents(holeCover);
            holeCover.transform.localPosition = new Vector3(28, 76, -27);
            holeCover.transform.localEulerAngles = new Vector3(0, 179, 180);
            holeCover.transform.localScale = new Vector3(7, 7, 3);
        }
    }

    private static void CleanUpPrefabComponents(GameObject obj)
    {
        Object.DestroyImmediate(obj.GetComponent<PrefabIdentifier>());
        Object.DestroyImmediate(obj.GetComponent<LargeWorldEntity>());
        Object.DestroyImmediate(obj.GetComponent<SkyApplier>());
    }
}