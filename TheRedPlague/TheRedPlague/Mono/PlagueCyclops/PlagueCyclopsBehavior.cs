using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Nautilus.Utility;
using UnityEngine;

namespace TheRedPlague.Mono.PlagueCyclops;

public class PlagueCyclopsBehavior : MonoBehaviour
{
    public bool applyInstantly;

    private static readonly FMODAsset EngineBreakSound =
        AudioUtils.GetFmodAsset("PlagueCyclopsEngineBreak");
    
    private static readonly FMODAsset TentaclesSpawnSound =
        AudioUtils.GetFmodAsset("PlagueCyclopsTentaclesSpawn");
    
    private static readonly Dictionary<string, FMODAsset> NewVoiceLines = new()
    {
        {"AheadFlank", AudioUtils.GetFmodAsset("PlagueCyclopsAheadFlank")},
        {"AheadSlow", AudioUtils.GetFmodAsset("PlagueCyclopsAheadSlow")},
        {"AheadStandard", AudioUtils.GetFmodAsset("PlagueCyclopsAheadStandard")},
        {"CyclopsEnginePowerDown", AudioUtils.GetFmodAsset("PlagueCyclopsEnginePoweringDown")},
        {"CyclopsEnginePowerUp", AudioUtils.GetFmodAsset("PlagueCyclopsEnginePoweringUp")},
        {"CyclopsWelcomeAboard", AudioUtils.GetFmodAsset("PlagueCyclopsWelcomeAboard")},
        {"CyclopsWelcomeAboardAttention", AudioUtils.GetFmodAsset("PlagueCyclopsWelcomeAboard")}
    };
    
    public static void ConvertPlagueCyclops(GameObject cyclops, bool instantConversion)
    {
        if (cyclops.GetComponent<PlagueCyclopsBehavior>() != null) return;
        
        cyclops.AddComponent<PlagueCyclopsBehavior>().applyInstantly = instantConversion;
    }

    private IEnumerator Start()
    {
        if (applyInstantly)
        {
            SoundAlarms();
            SpawnTentacles();
            SetLightsRed();
            ReplaceVoiceLines();
            PlayAssimilationVoiceLine();
            KnockOutModules();
            SpawnExteriorModel();
            yield break;
        }
        
        yield return new WaitForSeconds(3);
        SoundAlarms();
        yield return new WaitForSeconds(2);
        KnockOutModules();
        MainCameraControl.main.ShakeCamera(1f, 3);
        yield return new WaitForSeconds(1);
        SpawnTentacles();
        MainCameraControl.main.ShakeCamera(0.8f, 7);
        yield return new WaitForSeconds(7);
        MainCameraControl.main.ShakeCamera(2, 3);
        SetLightsOff();
        ReplaceVoiceLines();
        MainCameraControl.main.ShakeCamera(0.5f, 4);
        yield return new WaitForSeconds(2);
        Utils.PlayFMODAsset(EngineBreakSound, Player.main.transform.position);
        MainCameraControl.main.ShakeCamera(2f, 4);
        yield return new WaitForSeconds(7);
        SetLightsRed();
        PlayAssimilationVoiceLine();
        SpawnExteriorModel();
        MainCameraControl.main.ShakeCamera(1f, 2f);
    }

    public void SpawnTentacles()
    {
        var tentacles = Instantiate(Plugin.AssetBundle.LoadAsset<GameObject>("CyclopsInfestationSequencePrefab"), transform);
        tentacles.transform.localPosition = new Vector3(0.9f, 0, 7.1f);
        tentacles.transform.localEulerAngles = new Vector3(0, 90, 0);
        tentacles.transform.localScale = Vector3.one * 0.16f;
        MaterialUtils.ApplySNShaders(tentacles.gameObject);
        var skyAppliers = GetComponents<SkyApplier>();
        var baseInterior = skyAppliers.First(applier => applier.anchorSky == Skies.BaseInterior);
        var skyApplier = tentacles.AddComponent<SkyApplier>();
        skyApplier.renderers = tentacles.GetComponentsInChildren<Renderer>();
        skyApplier.anchorSky = Skies.BaseInterior;
        skyApplier.customSkyPrefab = baseInterior.customSkyPrefab;
        
        Utils.PlayFMODAsset(TentaclesSpawnSound, tentacles.transform.position);
    }

    public void SetLightsOff()
    {
        foreach (var renderer in gameObject.GetComponentsInChildren<Renderer>())
        {
            var materials = renderer.materials;
            foreach (var material in materials)
            {
                material.SetColor(ShaderPropertyID._GlowColor, new Color(0, 0, 0));
                var color = material.color;
                var specColor = material.GetColor("_SpecColor");
                material.color = new Color(color.r / 4, color.g / 4, color.b / 4, color.a);
                material.SetColor("_SpecColor", new Color(specColor.r / 4, specColor.g / 4, specColor.b / 4, color.a));
            }
        }
    }

    public void SpawnExteriorModel()
    {
        var exteriorModel = Instantiate(Plugin.AssetBundle.LoadAsset<GameObject>("PlagueCyclopsAttachments"), transform);
        exteriorModel.transform.localPosition = new Vector3(0, 0, 0);
        exteriorModel.transform.localEulerAngles = new Vector3(0, 0, 0);
        exteriorModel.transform.localScale = Vector3.one * 10;
        MaterialUtils.ApplySNShaders(exteriorModel.gameObject);
        exteriorModel.AddComponent<SkyApplier>().renderers = exteriorModel.GetComponentsInChildren<Renderer>();

        var renderer = exteriorModel.transform.Find("Cyclopsattachements").GetChild(0).GetComponent<Renderer>();
        
        var material = new Material(MaterialUtils.IonCubeMaterial);
        material.SetColor(ShaderPropertyID._Color, Color.black);
        material.SetColor(ShaderPropertyID._SpecColor, Color.black);
        material.SetColor(ShaderPropertyID._SpecColor, Color.black);
        material.SetColor(ShaderPropertyID._GlowColor, Color.red);
        material.SetFloat(ShaderPropertyID._GlowStrength, 2.2f);
        material.SetFloat(ShaderPropertyID._GlowStrengthNight, 2.2f);
        material.SetColor("_DetailsColor", Color.red);
        material.SetColor("_SquaresColor", new Color(3, 2, 1));
        material.SetFloat("_SquaresTile", 78);
        material.SetFloat("_SquaresSpeed", 8.8f);
        material.SetVector("_NoiseSpeed", new Vector4(0.5f, 0.3f, 0f));
        material.SetVector("_FakeSSSParams", new Vector4(0.2f, 1f, 1f));
        material.SetVector("_FakeSSSSpeed", new Vector4(0.5f, 0.5f, 1.37f));
        
        renderer.material = material;

        var clog = gameObject.GetComponent<CyclopsClogEngines>();
        if (clog != null) clog.plagueCyclops = true;
    }
    
    public void SetLightsRed()
    {
        foreach (var renderer in gameObject.GetComponentsInChildren<Renderer>())
        {
            var materials = renderer.materials;
            foreach (var material in materials)
            {
                material.SetColor(ShaderPropertyID._GlowColor, new Color(1, 0, 0));
                var color = material.color;
                material.color = new Color(color.r * 2, color.g * 2, color.b * 2, color.a);
                material.SetColor("_SpecColor", Color.red);
            }
        }
    }

    public void SoundAlarms()
    {
        transform.Find("HelmHUD").gameObject.GetComponent<CyclopsHelmHUDManager>().OnTakeCreatureDamage();
    }

    public void KnockOutModules()
    {
        var modulesParent = transform
            .Find(
                "CyclopsMeshStatic/undamaged/cyclops_LOD0/cyclops_engine_room/cyclops_engine_console/Submarine_engine_GEO/submarine_engine_console_01_wide");
        foreach (Transform child in modulesParent)
        {
            var moduleModel = child.gameObject;
            if (!moduleModel.activeSelf) continue;
            if (moduleModel.name == "console") continue;
            var rb = moduleModel.AddComponent<Rigidbody>();
            rb.interpolation = RigidbodyInterpolation.Interpolate;
            rb.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
            rb.AddForce(rb.transform.forward * 8, ForceMode.VelocityChange);
            moduleModel.AddComponent<CapsuleCollider>();
        }
    }

    public void ReplaceVoiceLines()
    {
        foreach (var vo in gameObject.GetComponents<VoiceNotification>())
        {
            if (NewVoiceLines.TryGetValue(vo.text, out var newSound))
            {
                vo.sound = newSound;
            }
        }
    }

    public void PlayAssimilationVoiceLine()
    {
        Utils.PlayFMODAsset(AudioUtils.GetFmodAsset("PlagueCyclopsAssimilationSuccessful"), transform.position);
        Subtitles.Add("PlagueCyclopsAssimilationSuccessful");
    }
}