using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;

namespace PdaUpgradeChips.Patches;

[HarmonyPatch(typeof(uGUI_InventoryTab))]
internal static class AddEquipmentUISlotsPatch
{
    [HarmonyPatch(nameof(uGUI_InventoryTab.Awake))]
    [HarmonyPostfix]
    public static void AwakePostfix(uGUI_InventoryTab __instance)
    {
        var equipmentParent = __instance.transform.Find("Equipment");
        var headSlot = equipmentParent.Find("Head").gameObject;
        var hintIcon = Plugin.Bundle.LoadAsset<Sprite>("UpgradeChipHint");
        int i = 0;
        float spacing = 60;
        float zigZagDistance = 50;
        foreach (var slotName in PdaUpgradesAPI.GetUpgradeEquipmentSlotNames())
        {
            var module = Object.Instantiate(headSlot, equipmentParent);
            module.transform.localPosition =
                new Vector2(
                    i % 2 * zigZagDistance - zigZagDistance / 2f,
                    spacing * PdaUpgradesAPI.UpgradeSlotsCount / 2 - spacing * i );
            i++;
            module.name = slotName;
            module.transform.localScale = Vector3.one * 0.6f;
            var slot = module.GetComponent<uGUI_EquipmentSlot>();
            slot.slot = slotName;
            slot.hint.GetComponent<Image>().sprite = hintIcon;
            slot.hint.transform.localScale = new Vector3(0.7f, 1f, 1f);
        }
    }
}