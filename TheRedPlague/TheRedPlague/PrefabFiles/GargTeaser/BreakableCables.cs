using System.Collections;
using Nautilus.Assets;
using Nautilus.Utility;
using TheRedPlague.Mono.PlagueGarg;
using UnityEngine;
using UWE;

namespace TheRedPlague.PrefabFiles.GargTeaser;

public static class BreakableCables
{
    public static PrefabInfo Info { get; } = PrefabInfo.WithTechType("BreakableCables");

    public static void Register()
    {
        var prefab = new CustomPrefab(Info);
        prefab.SetGameObject(GetPrefabAsync);
        prefab.Register();
    }

    private static IEnumerator GetPrefabAsync(IOut<GameObject> prefab)
    {
        var obj = Object.Instantiate(Plugin.AssetBundle.LoadAsset<GameObject>("BreakableCablePrefab"));
        obj.SetActive(false);
        PrefabUtils.AddBasicComponents(obj, Info.ClassID, Info.TechType, LargeWorldEntity.CellLevel.VeryFar);
        var request = PrefabDatabase.GetPrefabAsync("31f84eba-d435-438c-a58e-f3f7bae8bfbd");
        yield return request;
        if (request.TryGetPrefab(out var baseGameCable))
        {
            obj.GetComponentInChildren<Renderer>().material =
                new Material(baseGameCable.GetComponentInChildren<Renderer>().material);
        }
        obj.AddComponent<ConstructionObstacle>();
        obj.AddComponent<BreakableCable>().animator = obj.GetComponentInChildren<Animator>();
        prefab.Set(obj);
    }
}