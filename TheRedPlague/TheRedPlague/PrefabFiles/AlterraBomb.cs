using System.Collections;
using Nautilus.Assets;
using Nautilus.Assets.PrefabTemplates;
using TheRedPlague.Mono.Tools;
using UnityEngine;

namespace TheRedPlague.PrefabFiles;

public static class AlterraBomb
{
    public static PrefabInfo Info { get; } = PrefabInfo.WithTechType("AlterraBomb");

    public static void Register()
    {
        var customPrefab = new CustomPrefab(Info);
        customPrefab.SetGameObject(new CloneTemplate(Info, "60f7bcb6-1d0f-40a2-ab68-e1fae7370d4a")
        {
            ModifyPrefabAsync = ModifyPrefabAsync
        });
        customPrefab.Register();
    }

    private static IEnumerator ModifyPrefabAsync(GameObject prefab)
    {
        var seamothTask = CraftData.GetPrefabForTechTypeAsync(TechType.Seamoth);
        yield return seamothTask;
        var seamothPrefab = seamothTask.GetResult();
        var destructionEffect = seamothPrefab.GetComponent<SeaMoth>().destructionEffect;
        prefab.AddComponent<BombExplodeOnImpact>().explosionPrefab = destructionEffect;
        var rb = prefab.GetComponent<Rigidbody>();
        rb.mass = 2;
        rb.useGravity = false;
        rb.isKinematic = true;
        var wf = prefab.AddComponent<WorldForces>();
        wf.useRigidbody = rb;
        var renderer = prefab.GetComponentInChildren<Renderer>();
        var materials = renderer.materials;
        materials[0].color = new Color(0.7f, 0f, 0f);
    }
}