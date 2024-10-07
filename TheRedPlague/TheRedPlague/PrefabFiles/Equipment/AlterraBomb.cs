using System.Collections;
using Nautilus.Assets;
using Nautilus.Assets.PrefabTemplates;
using Nautilus.Utility;
using TheRedPlague.Mono.Tools;
using UnityEngine;

namespace TheRedPlague.PrefabFiles.Equipment;

public static class AlterraBomb
{
    public static PrefabInfo Info { get; } = PrefabInfo.WithTechType("AlterraBomb");

    public static void Register()
    {
        var customPrefab = new CustomPrefab(Info);
        customPrefab.SetGameObject(GetPrefabAsync);
        customPrefab.Register();
    }

    private static IEnumerator GetPrefabAsync(IOut<GameObject> prefab)
    {
        var obj = Object.Instantiate(Plugin.AssetBundle.LoadAsset<GameObject>("ExplosiveCratePrefab"));
        obj.SetActive(false);
        PrefabUtils.AddBasicComponents(obj, Info.ClassID, Info.TechType, LargeWorldEntity.CellLevel.Near);
        MaterialUtils.ApplySNShaders(obj);
        var seamothTask = CraftData.GetPrefabForTechTypeAsync(TechType.Seamoth);
        yield return seamothTask;
        var seamothPrefab = seamothTask.GetResult();
        var destructionEffect = seamothPrefab.GetComponent<SeaMoth>().destructionEffect;
        obj.AddComponent<BombExplodeOnImpact>().explosionPrefab = destructionEffect;
        var rb = obj.AddComponent<Rigidbody>();
        rb.mass = 2;
        rb.useGravity = false;
        rb.isKinematic = true;
        var wf = obj.AddComponent<WorldForces>();
        wf.useRigidbody = rb;
        obj.AddComponent<ConstructionObstacle>();
        
        prefab.Set(obj);
    }
}