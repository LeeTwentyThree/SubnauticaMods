using Nautilus.Assets;
using Nautilus.Assets.Gadgets;
using Nautilus.Utility;
using TheRedPlague.Mono;
using UnityEngine;

namespace TheRedPlague.PrefabFiles;

public static class InfectedArchitectPrefab
{
    public static PrefabInfo Info { get; } = PrefabInfo.WithTechType("InfectedArchitect");

    public static void Register()
    {
        var prefab = new CustomPrefab(Info);
        prefab.SetGameObject(GetGameObject);
        prefab.SetSpawns(new SpawnLocation(new Vector3(-1319.740f, -223.150f, 280)));
        prefab.Register();
    }

    private static GameObject GetGameObject()
    {
        var obj = Object.Instantiate(Plugin.AssetBundle.LoadAsset<GameObject>("InfectedArchitectPrefab"));
        obj.SetActive(false);
        PrefabUtils.AddBasicComponents(obj, Info.ClassID, Info.TechType, LargeWorldEntity.CellLevel.Global);
        MaterialUtils.ApplySNShaders(obj, 7, 2f, 3f);
        obj.AddComponent<InfectionTrackerTarget>();
        return obj;
    }
}