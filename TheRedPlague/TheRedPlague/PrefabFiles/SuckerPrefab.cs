using System.Collections;
using JetBrains.Annotations;
using Nautilus.Assets;
using Nautilus.Utility;
using TheRedPlague.Mono;
using TheRedPlague.Mono.CreatureBehaviour.Sucker;
using UnityEngine;
using UWE;

namespace TheRedPlague.PrefabFiles;

public static class SuckerPrefab
{
    public static PrefabInfo Info { get; } = PrefabInfo.WithTechType("Sucker");

    public static void Register()
    {
        var prefab = new CustomPrefab(Info);
        prefab.SetGameObject(GetPrefab);
        prefab.Register();
    }

    private static IEnumerator GetPrefab(IOut<GameObject> prefab)
    {
        var go = Object.Instantiate(Plugin.AssetBundle.LoadAsset<GameObject>("Sucker"));
        go.SetActive(false);
        PrefabUtils.AddBasicComponents(go, Info.ClassID, Info.TechType, LargeWorldEntity.CellLevel.Medium);
        MaterialUtils.ApplySNShaders(go);
        // var infect = go.AddComponent<InfectAnything>();
        // infect.infectionHeightStrength = 0.05f;

        var request = PrefabDatabase.GetPrefabAsync("98ac710d-5390-49fd-a850-dbea7bc07aef");
        yield return request;
        if (request.TryGetPrefab(out var controlRoomPrefab))
        {
            var skyApplier = go.EnsureComponent<SkyApplier>();
            skyApplier.customSkyPrefab = controlRoomPrefab.GetComponent<SkyApplier>().customSkyPrefab;
            skyApplier.dynamic = false;
            skyApplier.anchorSky = Skies.Custom;
        }

        var rb = go.AddComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.mass = 2000;
        var wf = go.AddComponent<WorldForces>();
        wf.useRigidbody = rb;
        
        go.transform.Find("SuckerEyePivot").gameObject.AddComponent<SuckerLook>();
        go.AddComponent<SuckerDamageable>();

        prefab.Set(go);
    }
}