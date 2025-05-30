using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;

namespace PdaUpgradeChips.Patches;

[HarmonyPatch(typeof(uGUI_InventoryTab))]
public static class AddEquipmentUISlotsPatch
{
    [HarmonyPatch(nameof(uGUI_InventoryTab.Awake))]
    [HarmonyPostfix]
    public static void AwakePostfix(uGUI_InventoryTab __instance)
    {
        var equipmentParent = __instance.transform.Find("Equipment");
        var headIcon = __instance.transform.Find("Head").gameObject;
        var hintIcon = Plugin.Bundle.LoadAsset<Sprite>("UpgradeChipHint");
        int i = 0;
        float spacing = 100f;
        foreach (var slotName in PdaUpgradesAPI.GetUpgradeEquipmentSlotNames())
        {
            var module = Object.Instantiate(headIcon, equipmentParent);
            module.transform.localPosition =
                new Vector2(0, spacing * i - spacing * PdaUpgradesAPI.UpgradeSlotsCount / 2);
            i++;
            module.name = slotName;
            var slot = module.GetComponent<uGUI_EquipmentSlot>();
            slot.slot = slotName;
            slot.hint.GetComponent<Image>().sprite = hintIcon;
        }
    }
}