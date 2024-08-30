using Nautilus.Assets;
using Nautilus.Assets.Gadgets;
using UnityEngine;
using System.Collections;
using Nautilus.Utility;
using TheRedPlague.Mono;

namespace TheRedPlague.PrefabFiles;

public static class NewInfectionDome
{
    public static PrefabInfo Info { get; } = PrefabInfo.WithTechType("NewInfectionDome");

    private static Material _shieldMaterial;
    private static bool _cyclopsLoaded;
    
    public static void Register()
    {
        var infectionDome = new CustomPrefab(Info);
        infectionDome.SetGameObject(GetPrefab);
        // infectionDome.SetSpawns(new SpawnLocation(Vector3.zero, Vector3.zero, Vector3.one * 1000));
        infectionDome.Register();
        infectionDome.RemoveFromCache();
    }
    
    private static IEnumerator GetPrefab(IOut<GameObject> prefab)
    {
        var obj = Object.Instantiate(Plugin.AssetBundle.LoadAsset<GameObject>("NewInfectionDome"));
        obj.SetActive(false);
        MaterialUtils.ApplySNShaders(obj);
        PrefabUtils.AddBasicComponents(obj, Info.ClassID, Info.TechType,
            LargeWorldEntity.CellLevel.Global);
        var renderer = obj.GetComponentInChildren<Renderer>();

        _cyclopsLoaded = false;
        
        yield return new WaitUntil(() => LightmappedPrefabs.main);

        LightmappedPrefabs.main.RequestScenePrefab("Cyclops", OnCyclopsReferenceLoaded);

        yield return new WaitUntil(() => _cyclopsLoaded);
        
        var material = new Material(_shieldMaterial);
        var materials = renderer.materials;
        materials[0] = material;
        renderer.materials = materials;
        
        var domeController = obj.AddComponent<InfectionDomeController>();

        prefab.Set(obj);

        yield break;
    }

    private static void OnCyclopsReferenceLoaded(GameObject obj)
    {
        _shieldMaterial = obj.transform.Find("FX/x_Cyclops_GlassShield").gameObject.GetComponent<Renderer>().material;
        _cyclopsLoaded = true;
    }
}