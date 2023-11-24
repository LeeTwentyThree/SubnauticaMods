using HarmonyLib;
using UnityEngine;
using WeatherMod.Mono;

namespace WeatherMod.Patches;

[HarmonyPatch(typeof(BreathingSound), nameof(BreathingSound.Start))]
public static class WaterDropsOnScreenSpawner
{
    public static ParticleSystem ScreenDrops { get; private set; }
    
    [HarmonyPostfix]
    public static void Postfix(BreathingSound __instance)
    {
        var weatherObject = __instance.transform.parent.Find("weather").gameObject;
        var screenDropsPrefab = weatherObject.GetComponent<PrefabSpawn>().prefab.GetComponent<PrefabSpawn>().prefab
            .GetComponent<VFXWeatherManager>().screenDropsPrefab;
        
        var fpParticleEmissionPoint = Utils.GetLocalPlayerComp().fpParticleEmissionPoint;
        if (fpParticleEmissionPoint != null)
        {
            var screenDrops = UnityEngine.Object.Instantiate<GameObject>(screenDropsPrefab, fpParticleEmissionPoint.transform, true);
            screenDrops.transform.localPosition = new Vector3(-0.16f, 0f, 0f);
            screenDrops.transform.localEulerAngles = new Vector3(0f, 90f, 90f);
            var screenDropsMaterial = screenDrops.GetComponent<ParticleSystemRenderer>().material;
            screenDropsMaterial.SetColor(ShaderPropertyID._Color, new Color(1, 1, 1, 0.3f));
            screenDropsMaterial.SetFloat("_sunSensitivity", 0.1f);
            screenDrops.AddComponent<WaterDropsOnScreen>();
            
            ScreenDrops = screenDrops.GetComponent<ParticleSystem>();
        }
    }
}