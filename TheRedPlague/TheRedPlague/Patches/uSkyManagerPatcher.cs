using HarmonyLib;
using TheRedPlague.Mono;
using uSky;

namespace TheRedPlague.Patches;

[HarmonyPatch(typeof(uSkyManager))]
public static class uSkyManagerPatcher
{
    [HarmonyPatch(nameof(uSkyManager.Start))]
    [HarmonyPostfix]
    public static void StartPostfix(uSkyManager __instance)
    {
        var light = __instance.GetComponent<uSkyLight>();
        light.SunIntensity *= (1f - (Plugin.ModConfig.DarknessPercent * 0.01f));
    }
}