using System.Collections;
using Nautilus.Assets;
using Nautilus.Assets.Gadgets;
using Nautilus.Utility;
using TheRedPlague.Mono;
using UnityEngine;

namespace TheRedPlague.PrefabFiles;

public static class LowQualityForceFieldIsland
{
    public static PrefabInfo Info { get; } = PrefabInfo.WithTechType("ForceFieldIslandLowQuality");

    public static void Register()
    {
        var lowQualityForceFieldIsland = new CustomPrefab(Info);
        lowQualityForceFieldIsland.SetGameObject(GetLowQualityForceFieldIslandPrefab);
        lowQualityForceFieldIsland.SetSpawns(new SpawnLocation(new Vector3(-128, 160, -128), new Vector3(0, 180, 0),
            new Vector3(1, 1, -1)));
        lowQualityForceFieldIsland.Register();
    }
    
    private static IEnumerator GetLowQualityForceFieldIslandPrefab(IOut<GameObject> prefab)
    {
        var obj = Object.Instantiate(Plugin.AssetBundle.LoadAsset<GameObject>("ForcefieldIslandLowQuality"));
        MaterialUtils.ApplySNShaders(obj);
        PrefabUtils.AddBasicComponents(obj, Info.ClassID, Info.TechType,
            LargeWorldEntity.CellLevel.Global);
        obj.AddComponent<LowQualityIslandMesh>();
        // var renderer = obj.GetComponentInChildren<Renderer>();
        yield return null;
        prefab.Set(obj);
    }
}