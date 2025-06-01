using HarmonyLib;
using PdaUpgradeChips.MonoBehaviours;

namespace PdaUpgradeChips.Patches;

[HarmonyPatch(typeof(uGUI_PDA))]
internal static class ToggleOpenMenuButtonPatch
{
    [HarmonyPatch(nameof(uGUI_PDA.OpenTab))]
    [HarmonyPostfix]
    public static void OpenTabPostfix(uGUI_PDA __instance, PDATab tabId)
    {
        var button = PdaUpgradeButton.Main;
        if (button == null)
        {
            Plugin.Logger.LogError("Upgrades tab button not found!");
            return;
        }

        var equipmentTab = ((uGUI_InventoryTab)__instance.tabInventory).equipment;
        button.gameObject.SetActive(tabId == PDATab.Inventory && equipmentTab.equipment == Inventory.main.equipment);
    }
}