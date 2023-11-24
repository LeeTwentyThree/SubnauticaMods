using System.Collections;
using HarmonyLib;
using UnityEngine;
using UWE;
using WeatherMod.Mono;

namespace WeatherMod.Patches;

[HarmonyPatch(typeof(WaterSurface), nameof(WaterSurface.SetupSurfaceMaterial))]
public static class WaterSurfaceDarkener
{
    [HarmonyPostfix]
    public static void Postfix(WaterSurface __instance, Camera camera, Material material)
    {
        if (!__instance.useWaterWaterBrightnessCurve)
            return;

        var brightnessMultiplier = GetBrightnessMultiplier();
        
        material.SetFloat(ShaderPropertyID._UnderWaterSkyBrightness, __instance.underWaterBrightnessCurve.Evaluate(Ocean.GetDepthOf(camera.transform.position)) * brightnessMultiplier * brightnessMultiplier);
    }

    private static float GetBrightnessMultiplier()
    {
        var weatherManager = CustomWeatherManager.Main;
        if (weatherManager == null || weatherManager.CurrentEvent == null) return 1f;
        return FogManager.SunlightBrightnessBelowWater;
    }
}