using HarmonyLib;
using Nautilus.Utility;
using TheRedPlague.Mono;

namespace TheRedPlague.Patches;

[HarmonyPatch(typeof(Creature))]
public static class WarperPatcher
{
    [HarmonyPatch(nameof(Creature.Start))]
    [HarmonyPostfix]
    public static void StartPostfix(Creature __instance)
    {
        if (__instance is not Warper) return;
        
        var sounds = __instance.gameObject.EnsureComponent<PlayRandomSounds>();
        sounds.asset = AudioUtils.GetFmodAsset("InfectedWarperIdle");
        sounds.minDelay = 10;
        sounds.maxDelay = 25;

        __instance.gameObject.EnsureComponent<WarperBecomeFriendly>();
        __instance.gameObject.EnsureComponent<WarperDropHeartOnDeath>();
    }
}