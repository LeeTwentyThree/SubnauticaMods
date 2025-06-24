using HarmonyLib;
using PdaUpgradeChips.MonoBehaviours;

namespace PdaUpgradeChips.Patches;

[HarmonyPatch]
internal static class DisableSavingInDimensionPatch
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(IngameMenu), nameof(IngameMenu.GetAllowSaving))]
    public static void GetAllowSavingPostfix(ref bool __result)
    {
        var sub = Player.main.GetCurrentSub();
        if (sub == null)
            return;
        if (sub is PocketDimensionSub)
            __result = false;
    }
}