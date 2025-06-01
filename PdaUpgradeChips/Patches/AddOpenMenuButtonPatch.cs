using HarmonyLib;
using PdaUpgradeChips.MonoBehaviours;
using UnityEngine;
using UnityEngine.UI;

namespace PdaUpgradeChips.Patches;

[HarmonyPatch(typeof(uGUI_PDA))]
internal static class AddOpenMenuButtonPatch
{
    [HarmonyPatch(nameof(uGUI_PDA.Awake))]
    [HarmonyPostfix]
    public static void PdaAwakePostfix(uGUI_PDA __instance)
    {
        CreateButton(__instance.transform);
    }

    private static void CreateButton(Transform pdaScreen)
    {
        var inventoryTab = pdaScreen.Find("Content/InventoryTab");
        var buttonObj = new GameObject("ViewPdaUpgradesButton");
        
        // Create button object
        var rect = buttonObj.AddComponent<RectTransform>();
        rect.SetParent(inventoryTab);
        rect.localScale = Vector3.one;
        rect.anchoredPosition = new Vector2(500, 240);
        rect.localRotation = Quaternion.identity;
        
        // Add background image
        var buttonImage = buttonObj.AddComponent<Image>();
        var headImageBackground = inventoryTab.Find("Equipment/Head/Background").GetComponent<Image>();
        buttonImage.sprite = headImageBackground.sprite;
        buttonImage.color = headImageBackground.color;
        buttonImage.type = Image.Type.Simple;
        buttonImage.material = headImageBackground.material;
        
        // Add icon
        var iconObj = new GameObject("ButtonIcon");
        var iconRect = iconObj.AddComponent<RectTransform>();
        iconRect.SetParent(buttonObj.transform);
        iconRect.localScale = Vector3.one * 0.7f;
        iconRect.anchoredPosition = Vector2.zero;
        iconRect.localRotation = Quaternion.identity;
        var normalIcon = Plugin.Bundle.LoadAsset<Sprite>("ViewUpgradeChipsButtonIcon");
        var selectedIcon = Plugin.Bundle.LoadAsset<Sprite>("ViewUpgradeChipsButtonSelectedIcon");
        var pressedIcon = Plugin.Bundle.LoadAsset<Sprite>("ViewUpgradeChipsButtonPressedIcon");
        var iconImage = iconObj.AddComponent<Image>();
        iconImage.sprite = normalIcon;

        // Add button functionality
        var button = buttonObj.AddComponent<Button>();
        button.targetGraphic = iconImage;
        button.transition = Selectable.Transition.SpriteSwap;
        var state = button.spriteState;
        state.selectedSprite = selectedIcon;
        state.highlightedSprite = selectedIcon;
        state.pressedSprite = pressedIcon;
        state.disabledSprite = normalIcon;
        button.spriteState = state;
        button.onClick = new Button.ButtonClickedEvent();
        button.onClick.AddListener(OnButtonClicked);
    }

    private static void OnButtonClicked()
    {
        PdaUpgradesManager.Main.DisplayMenu();
    }
}