using System.Collections;
using Nautilus.Assets;
using Nautilus.Utility;
using TheRedPlague.MaterialModifiers;
using TheRedPlague.Mono;
using UnityEngine;

namespace TheRedPlague.PrefabFiles;

public static class AuroraTentaclePrefab
{
    public static PrefabInfo Info { get; } = PrefabInfo.WithTechType("AuroraTentacle");

    public static void Register()
    {
        var prefab = new CustomPrefab(Info);
        prefab.SetGameObject(GetPrefab);
        prefab.Register();
    }

    private static IEnumerator GetPrefab(IOut<GameObject> prefab)
    {
        var obj = Object.Instantiate(Plugin.AssetBundle.LoadAsset<GameObject>("AuroraTentacle"));
        obj.SetActive(false);
        PrefabUtils.AddBasicComponents(obj, Info.ClassID, Info.TechType, LargeWorldEntity.CellLevel.Global);
        MaterialUtils.ApplySNShaders(obj, 7f, 1, 1f, new AirStrikeDeviceModifier());
        var behavior = obj.AddComponent<AuroraTentacleBehavior>();
        behavior.animator = obj.GetComponentInChildren<Animator>();
        behavior.renderer = obj.GetComponentInChildren<Renderer>();
        behavior.renderer.enabled = false;
        prefab.Set(obj);
        yield break;
    }
}