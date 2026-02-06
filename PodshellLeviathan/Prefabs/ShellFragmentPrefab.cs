using System.Collections;
using Nautilus.Assets;
using Nautilus.Utility;
using UnityEngine;

namespace PodshellLeviathan.Prefabs;

public static class ShellFragmentPrefab
{
    public static PrefabInfo Info { get; } = PrefabInfo.WithTechType("PodshellFragment")
        .WithIcon(Plugin.Assets.LoadAsset<Sprite>("ShellFragmentIcon"))
        .WithSizeInInventory(new Vector2int(3, 3));
    
    public static void Register()
    {
        var prefab = new CustomPrefab(Info);
        prefab.SetGameObject(GetPrefab);
        prefab.Register();
    }

    private static IEnumerator GetPrefab(IOut<GameObject> result)
    {
        var task = Plugin.Assets.LoadAssetAsync<GameObject>("ShellFragmentPrefab");
        yield return task;
        var obj = Object.Instantiate(task.asset as GameObject);
        obj.SetActive(false);
        PrefabUtils.AddBasicComponents(obj, Info.ClassID, Info.TechType, LargeWorldEntity.CellLevel.Near);
        MaterialUtils.ApplySNShaders(obj, 8);
        PrefabUtils.AddWorldForces(obj, 10, 0.8f, 1.4f);
        obj.AddComponent<Pickupable>();
        yield return null;
        result.Set(obj);
    }
}