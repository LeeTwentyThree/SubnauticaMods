using System.Collections;
using Nautilus.Assets;
using Nautilus.Handlers;
using Nautilus.Utility;
using TheRedPlague.Mono;
using TheRedPlague.Mono.FleshBlobs;
using UnityEngine;

namespace TheRedPlague.PrefabFiles.Creatures;

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
        
        CoordinatedSpawnsHandler.RegisterCoordinatedSpawnsForOneTechType(Infos[0].TechType, new SpawnLocation(new Vector3(-1195, -185, 400)), new SpawnLocation(new Vector3(-1660, -330, 20)));
        CoordinatedSpawnsHandler.RegisterCoordinatedSpawnsForOneTechType(Infos[1].TechType, new SpawnLocation(new Vector3(-1605, -230, 670)));
        CoordinatedSpawnsHandler.RegisterCoordinatedSpawnsForOneTechType(Infos[2].TechType, new SpawnLocation(new Vector3(-333, -54, -67)));
        CoordinatedSpawnsHandler.RegisterCoordinatedSpawnsForOneTechType(Infos[7].TechType, new SpawnLocation(new Vector3(-1438, -340, 291)), new SpawnLocation(new Vector3(-1305, -300, 301)));
    }

    public static IEnumerator GetGameObject(IOut<GameObject> prefab)
    {
        var obj = Object.Instantiate(Plugin.AssetBundle.LoadAsset<GameObject>("FleshBlobLeaderPrefab"));
        obj.SetActive(false);
        PrefabUtils.AddBasicComponents(obj, null, TechType.None, LargeWorldEntity.CellLevel.Global);
        MaterialUtils.ApplySNShaders(obj);
        var behaviour = obj.AddComponent<FleshBlobLeaderBehaviour>();
        var movement = obj.AddComponent<FleshBlobMovement>();
        behaviour.movement = movement;
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

        var sounds = obj.AddComponent<FleshBlobSoundController>();

        var tornadoEmitter = obj.AddComponent<FMOD_CustomEmitter>();
        tornadoEmitter.SetAsset(AudioUtils.GetFmodAsset("FleshBlobTornadoLoop"));
        tornadoEmitter.followParent = true;
        
        var tornadoController = obj.AddComponent<FleshBlobTornadoController>();
        tornadoController.tornadoEmitter = tornadoEmitter;
        tornadoController.vfxParent = obj.transform.Find("VFX").gameObject;

        obj.AddComponent<FleshBlobGravity>();

        var killTriggerParent = obj.transform.Find("KillTriggers").gameObject;
        var killTriggerManager = obj.AddComponent<FleshBlobKillTriggerManager>();
        killTriggerManager.triggerParent = killTriggerParent;
        foreach (Transform child in killTriggerParent.transform)
        {
            child.gameObject.AddComponent<FleshBlobKillTrigger>().manager = killTriggerManager;
        }

        obj.AddComponent<FleshBlobGrowth>();
        
        yield break;
    }
}