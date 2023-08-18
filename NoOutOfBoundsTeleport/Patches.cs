using HarmonyLib;

namespace NoOutOfBoundsTeleport;

[HarmonyPatch]
internal static class Patches
{
    [HarmonyPatch(typeof(OutOfBoundsWarp), nameof(OutOfBoundsWarp.Start))]
    [HarmonyPostfix]
    public static void OutOfBoundsWarpStartPostfix(OutOfBoundsWarp __instance)
    {
        UnityEngine.Object.Destroy(__instance);
    }
}
