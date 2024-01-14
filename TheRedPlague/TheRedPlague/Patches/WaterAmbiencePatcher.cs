using HarmonyLib;
using Nautilus.Utility;

namespace TheRedPlague.Patches;

[HarmonyPatch(typeof(WaterAmbience))]
public static class WaterAmbiencePatcher
{
    [HarmonyPatch(nameof(WaterAmbience.Start))]
    [HarmonyPostfix]
    public static void StartPostfix(WaterAmbience __instance)
    {
        var dunesAmbience = __instance.transform.Find("background").Find("dunes").gameObject
            .GetComponent<FMOD_CustomLoopingEmitter>();
        dunesAmbience.SetAsset(AudioUtils.GetFmodAsset("InfectedDunesAmbience"));
    }
}