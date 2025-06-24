using System;
using UnityEngine;
using UnityEngine.UI;

namespace PdaUpgradeChips.Data;

public class CustomPdaButton : CustomPdaElement
{
    private Action OnPressed { get; }
    private ButtonIcons Icons { get; }

    public CustomPdaButton(string id, Action<RectTransform> placeElement, ButtonIcons icons, PDATab tab, Action onPressed, bool activeByDefault) :
        base(id, placeElement, tab, activeByDefault)
    {
        OnPressed = onPressed;
        Icons = icons;
    }

    protected override RectTransform BuildElement(uGUI_PDA pda)
    {
        var buttonRect = base.BuildElement(pda);
        var buttonObj = buttonRect.gameObject;
        
        // Add background image
        var buttonImage = buttonObj.AddComponent<Image>();
        var headImageBackground = pda.tabInventory.transform.Find("Equipment/Head/Background").GetComponent<Image>();
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
        var normalIcon = Icons.NormalIcon;
        var selectedIcon = Icons.SelectedIcon;
        var pressedIcon = Icons.PressedIcon;
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
        button.onClick.AddListener(OnButtonPressed);

        return buttonRect;
    }

    private void OnButtonPressed()
    {
        OnPressed?.Invoke();
    }

    public struct ButtonIcons
    {
        public ButtonIcons(string normalIcon, string selectedIcon, string pressedIcon)
        {
            NormalIcon = Plugin.Bundle.LoadAsset<Sprite>(normalIcon);
            SelectedIcon = Plugin.Bundle.LoadAsset<Sprite>(selectedIcon);
            PressedIcon = Plugin.Bundle.LoadAsset<Sprite>(pressedIcon);
        }

        public Sprite NormalIcon { get; }
        public Sprite SelectedIcon { get; }
        public Sprite PressedIcon { get; }
    }
}