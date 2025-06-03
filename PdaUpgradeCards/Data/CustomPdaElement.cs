using System;
using PdaUpgradeCards.Patches;
using UnityEngine;

namespace PdaUpgradeCards.Data;

public class CustomPdaElement
{
    public CustomPdaElement(string id, Action<RectTransform> placeButton, PDATab tab, bool activeByDefault)
    {
        Id = id;
        PlaceButton = placeButton;
        Tab = tab;
        ActiveByDefault = activeByDefault;
        BuildCustomPdaElementsPatch.BuildUIElementEvent += BuildAndCreateUI;
        ErrorMessage.AddMessage("registering UI");
    }

    private string Id { get; }
    private Action<RectTransform> PlaceButton { get; }
    private PDATab Tab { get; }
    private bool ActiveByDefault { get; }
    private GameObject _uiElement;

    public void SetElementActive(bool active)
    {
        if (_uiElement == null)
        {
            Plugin.Logger.LogError("UI element instance not found!");
            return;
        }
        _uiElement.SetActive(active);
    }

    private void BuildAndCreateUI(uGUI_PDA pda)
    {
        ErrorMessage.AddMessage("creating Ui");
        var ui = BuildElement(pda);
        PlaceButton?.Invoke(ui);
        _uiElement = ui.gameObject;
        _uiElement.SetActive(ActiveByDefault);
    }

    protected virtual RectTransform BuildElement(uGUI_PDA pda)
    {
        var tab = pda.GetTab(Tab).transform;
        var buttonObj = new GameObject(Id);
        
        // Create button object
        var rect = buttonObj.AddComponent<RectTransform>();
        rect.SetParent(tab);
        rect.localScale = Vector3.one;
        rect.localRotation = Quaternion.identity;
        rect.localPosition = Vector3.zero;

        return rect;
    }
}