using DebugHelper.Managers;
using DebugHelper.Systems;
using HarmonyLib;

namespace DebugHelper.Patches
{
    [HarmonyPatch(typeof(Player))]
    internal static class PlayerPatches
    {
        [HarmonyPatch(nameof(Player.Start))]
        [HarmonyPostfix()]
        public static void PlayerStartPostfix(Player __instance)
        {
            __instance.gameObject.EnsureComponent<DebugAutomation>();
            DebugOverlay.CreateInstance();
            DebugCollidersManager.CreateInstance();
        }
    }
}
