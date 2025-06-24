using HarmonyLib;
using PdaUpgradeChips.MonoBehaviours.Upgrades;

namespace PdaUpgradeChips.Patches;

[HarmonyPatch(typeof(uGUI_DepthCompass))]
public static class ObfuscateDepthPatch
{
    [HarmonyPatch(nameof(uGUI_DepthCompass.UpdateDepth))]
    [HarmonyPostfix]
    public static void UpdateDepthPostfix(uGUI_DepthCompass __instance)
    {
        if (PocketDimensionUpgrade.GetPlayerInsideAnyPocketDimension())
        {
            __instance.depthText.text = "???";
        }
    }
}