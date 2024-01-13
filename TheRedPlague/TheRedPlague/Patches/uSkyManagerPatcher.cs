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
        __instance.gameObject.AddComponent<SunBrightnessModifier>();
    }
}