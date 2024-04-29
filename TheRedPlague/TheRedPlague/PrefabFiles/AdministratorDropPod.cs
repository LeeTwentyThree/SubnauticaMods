using System.Collections;
using Nautilus.Assets;
using Nautilus.Handlers;
using UnityEngine;
using UWE;

namespace TheRedPlague.PrefabFiles;

public static class AdministratorDropPod
{
    public static PrefabInfo Info { get; } = PrefabInfo.WithTechType("AdministratorDropPod");
    public static PingType PingType { get; private set; }

    public static void Register()
    {
        var prefab = new CustomPrefab(Info);
        prefab.SetGameObject(GetPrefab);
        prefab.Register();
        PingType = EnumHandler.AddEntry<PingType>("AdministratorDropPodPing")
            .WithIcon(Plugin.AssetBundle.LoadAsset<Sprite>("AdministratorEscapePodPing"));
    }

    private static IEnumerator GetPrefab(IOut<GameObject> prefab)
    {
        var task = PrefabDatabase.GetPrefabAsync("00037e80-3037-48cf-b769-dc97c761e5f6");
        yield return task;
        task.TryGetPrefab(out var original);
        var pod = Object.Instantiate(original);
        pod.SetActive(false);
        pod.EnsureComponent<LargeWorldEntity>().cellLevel = LargeWorldEntity.CellLevel.Global;

        var exteriorRenderer = pod.transform.Find("life_pod_exploded_02_02/exterior/life_pod_damaged")
            .GetComponent<Renderer>();
        var exteriorMaterial = exteriorRenderer.materials[2];
        exteriorMaterial.mainTexture = Plugin.AssetBundle.LoadAsset<Texture2D>("AdministratorEscapePodDecal");
        
        prefab.Set(pod);
    }
}