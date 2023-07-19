using HarmonyLib;

namespace LiveMinimap;

[HarmonyPatch]
internal static class Patches
{
    [HarmonyPatch(typeof(Player), nameof(Player.Start))]
    [HarmonyPostfix]
    public static void PlayerStartPostfix(Player __instance)
    {
        __instance.gameObject.AddComponent<MinimapEnabler>();
    }
}
