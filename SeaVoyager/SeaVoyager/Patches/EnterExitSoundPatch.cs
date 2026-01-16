using HarmonyLib;
using Nautilus.Utility;
using SeaVoyager.Mono;

namespace SeaVoyager.Patches;

[HarmonyPatch(typeof(UseableDiveHatch))]
public static class EnterExitSoundPatch
{
    private static readonly FMODAsset EnterSound = AudioUtils.GetFmodAsset("SeaVoyagerEnter");
    private static readonly FMODAsset ExitSound = AudioUtils.GetFmodAsset("SeaVoyagerExit");
    
    [HarmonyPrefix]
    [HarmonyPatch(nameof(UseableDiveHatch.OnHandClick))]
    public static void OnHandClickPrefix(UseableDiveHatch __instance, GUIHand hand)
    {
        if (__instance is not SeaVoyagerDoor)
            return;
        var player = hand.gameObject.GetComponent<Player>();
        Utils.PlayFMODAsset(player.IsInside() ? ExitSound : EnterSound, __instance.transform.position);
    }
}