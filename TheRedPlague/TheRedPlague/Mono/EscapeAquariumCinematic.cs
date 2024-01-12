using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Nautilus.Extensions;
using Nautilus.Utility;
using UnityEngine;

namespace TheRedPlague.Mono;

public class EscapeAquariumCinematic : MonoBehaviour
{
    private static Vector3 _spawnPos = new Vector3(307.21f, -1538.24f, -431.58f);
    private static Vector3 _teleporterPos = new Vector3(242.93f, -1584.60f, -308.00f);

    private Renderer[] _renderers;
    private List<Material> _materials;
    
    public static void PlayCinematic(string key)
    {
        if (key != StoryUtils.OpenAquariumTeleporterGoalKey)
        {
            return;
        }
        new GameObject("EscapeAquariumCinematic").AddComponent<EscapeAquariumCinematic>();
    }

    private IEnumerator Start()
    {
        var originalEmperor = SeaEmperor.main;
        originalEmperor.gameObject.SetActive(false);

        var seaEmperorJuvenileTask = CraftData.GetPrefabForTechTypeAsync(TechType.SeaEmperorJuvenile);
        yield return seaEmperorJuvenileTask;
        
        var juvenileEmperor = Instantiate(seaEmperorJuvenileTask.GetResult());
        ZombieManager.InfectSeaEmperor(juvenileEmperor);
        juvenileEmperor.transform.position = _spawnPos;
        juvenileEmperor.transform.localScale = Vector3.one * 2;
        juvenileEmperor.transform.LookAt(_teleporterPos);
        juvenileEmperor.GetComponent<Creature>().enabled = false;
        yield return new WaitForSeconds(4);
        juvenileEmperor.GetComponent<SwimBehaviour>().SwimTo(_teleporterPos, 20);
        yield return new WaitUntil(() =>
            Vector3.SqrMagnitude(juvenileEmperor.transform.position - _teleporterPos) < 38 * 38);
        StartCoroutine(TeleportEmperorCinematic(juvenileEmperor));
    }

    private IEnumerator TeleportEmperorCinematic(GameObject emperor)
    {
        _renderers = emperor.GetComponentsInChildren<Renderer>();
        _materials = new List<Material>();
        var portalSoundEmitter = new GameObject("PortalSoundEmitter").AddComponent<FMOD_CustomEmitter>();
        portalSoundEmitter.SetAsset(AudioUtils.GetFmodAsset("event:/env/use_teleporter_use_loop"));
        portalSoundEmitter.stopImmediatelyOnDisable = true;
        portalSoundEmitter.Play();
        foreach (var renderer in _renderers)
        {
            var rendererMaterials = renderer.materials;
            _materials.AddRange(rendererMaterials);
        }

        foreach (var material in _materials)
        {
            material.EnableKeyword("MARMO_EMISSION");
            material.SetColor(ShaderPropertyID._GlowColor, Color.green);
        }
        yield return new WaitForSeconds(0.2f);
        FadingOverlay.PlayFX(new Color(0.5f, 1f, 0.5f), 0.1f, 0.2f, 1f);
        yield return new WaitForSeconds(0.15f);
        
        Destroy(emperor);

        yield return new WaitForSeconds(3);
        
        Destroy(portalSoundEmitter.gameObject);
    }
}