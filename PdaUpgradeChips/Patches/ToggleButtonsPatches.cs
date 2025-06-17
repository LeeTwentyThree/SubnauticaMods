using HarmonyLib;
using PdaUpgradeChips.Data;

namespace PdaUpgradeChips.Patches;

[HarmonyPatch]
internal static class ToggleButtonsPatches
{
    [HarmonyPatch(typeof(uGUI_PDA), nameof(uGUI_PDA.OpenTab))]
    [HarmonyPostfix]
    public static void OpenTabPostfix(uGUI_PDA __instance, PDATab tabId)
    {
        var equipmentTab = ((uGUI_InventoryTab)__instance.tabInventory).equipment;
        var activate = tabId == PDATab.Inventory && equipmentTab.equipment == Inventory.main.equipment;
        PdaElements.OpenMenuButton.SetElementActive(activate);
    }
}