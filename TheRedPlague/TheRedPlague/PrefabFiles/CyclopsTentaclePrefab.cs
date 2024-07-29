using ECCLibrary;
using Nautilus.Assets;
using Nautilus.Utility;
using TheRedPlague.Mono.PlagueCyclops;
using UnityEngine;

namespace TheRedPlague.PrefabFiles;

public static class CyclopsTentaclePrefab
{
    public static PrefabInfo Info { get; } = PrefabInfo.WithTechType("CyclopsTentacle");

    private static LiveMixinData _liveMixinData;

    public static void Register()
    {
        var prefab = new CustomPrefab(Info);
        prefab.SetGameObject(GetGameObject);
        prefab.Register();

        _liveMixinData = CreatureDataUtils.CreateLiveMixinData(100);
        _liveMixinData.broadcastKillOnDeath = true;
    }

    private static GameObject GetGameObject()
    {
        var obj = Object.Instantiate(Plugin.AssetBundle.LoadAsset<GameObject>("CyclopsTentaclePrefab"));
        obj.SetActive(false);
        PrefabUtils.AddBasicComponents(obj, Info.ClassID, Info.TechType, LargeWorldEntity.CellLevel.Medium);
        MaterialUtils.ApplySNShaders(obj);
        var release = obj.AddComponent<CyclopsTentacle>();
        release.animator = obj.GetComponentInChildren<Animator>();
        release.identifier = obj.GetComponent<UniqueIdentifier>();
        release.colliders = obj.GetComponents<Collider>();
        var lm = obj.AddComponent<LiveMixin>();
        lm.data = _liveMixinData;
        lm.health = _liveMixinData.maxHealth;
        var rb = obj.AddComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.mass = 2000;
        obj.AddComponent<ConstructionObstacle>();
        return obj;
    }
}