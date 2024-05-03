using System.Collections;
using Nautilus.Assets;
using Nautilus.Utility;
using TheRedPlague.Mono;
using TheRedPlague.Mono.FleshBlobs;
using UnityEngine;

namespace TheRedPlague.PrefabFiles;

public static class FleshBlobLeaders
{
    public static PrefabInfo[] Infos { get; private set; }

    public static void RegisterAll()
    {
        Infos = new PrefabInfo[FleshBlobData.Paths.Length];
        
        for (var i = 0; i < FleshBlobData.Paths.Length; i++)
        {
            var info = PrefabInfo.WithTechType($"FleshBlobLeader{i}");
            var prefab = new CustomPrefab(info);
            prefab.SetGameObject(GetGameObject);
            prefab.Register();
            Infos[i] = info;
        }
    }

    public static IEnumerator GetGameObject(IOut<GameObject> prefab)
    {
        var obj = Object.Instantiate(Plugin.AssetBundle.LoadAsset<GameObject>("FleshBlobLeaderPrefab"));
        obj.SetActive(false);
        PrefabUtils.AddBasicComponents(obj, null, TechType.None, LargeWorldEntity.CellLevel.Global);
        MaterialUtils.ApplySNShaders(obj);
        obj.AddComponent<FleshBlobLeaderBehaviour>();
        prefab.Set(obj);

        var renderers = obj.GetComponentsInChildren<Renderer>();
        foreach (var r in renderers)
        {
            var material = r.material;
            material.EnableKeyword("MARMO_EMISSION");
            material.SetFloat("_EmissionLM", 0.5f);
            material.SetFloat("_EmissionLMNight", 0.5f);
            material.SetFloat("_GlowStrength", 0);
            material.SetFloat("_GlowStrengthNight", 0);
        }
        
        var cubeRenderer = obj.transform.Find("VFX/PlagueTornado").GetComponent<Renderer>();
        var cubeMaterial = new Material(MaterialUtils.IonCubeMaterial);
        cubeMaterial.SetColor(ShaderPropertyID._Color, Color.black);
        cubeMaterial.SetColor(ShaderPropertyID._SpecColor, Color.black);
        cubeMaterial.SetColor(ShaderPropertyID._SpecColor, Color.black);
        cubeMaterial.SetColor(ShaderPropertyID._GlowColor, Color.red);
        cubeMaterial.SetFloat(ShaderPropertyID._GlowStrength, 2.2f);
        cubeMaterial.SetFloat(ShaderPropertyID._GlowStrengthNight, 2.2f);
        cubeMaterial.SetColor("_DetailsColor", Color.red);
        cubeMaterial.SetColor("_SquaresColor", new Color(3, 2, 1));
        cubeMaterial.SetFloat("_SquaresTile", 7.5f);
        cubeMaterial.SetFloat("_SquaresSpeed", 8.8f);
        cubeMaterial.SetVector("_NoiseSpeed", new Vector4(0.5f, 0.3f, 0f));
        cubeMaterial.SetVector("_FakeSSSParams", new Vector4(0.2f, 1f, 1f));
        cubeMaterial.SetVector("_FakeSSSSpeed", new Vector4(0.5f, 0.5f, 1.37f));
        cubeRenderer.material = cubeMaterial;

        obj.transform.Find("VFX/PlagueTornado-Blobs").gameObject.AddComponent<InfectAnything>();
        
        yield break;
    }
}