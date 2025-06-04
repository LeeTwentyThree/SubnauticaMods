using System;
using Nautilus.Utility;
using PdaUpgradeCards.Patches;
using UnityEngine;

namespace PdaUpgradeCards.Data;

public class CustomPdaElement
{
    public CustomPdaElement(string id, Action<RectTransform> placeElement, PDATab tab, bool activeByDefault)
    {
        Id = id;
        PlaceElement = placeElement;
        Tab = tab;
        ActiveByDefault = activeByDefault;
        BuildCustomPdaElementsPatch.BuildUIElementEvent += BuildAndCreateUI;
        SaveUtils.RegisterOnQuitEvent(OnQuit);
    }

    private string Id { get; }
    private Action<RectTransform> PlaceElement { get; }
    private PDATab Tab { get; }
    private bool ActiveByDefault { get; }
    private GameObject _uiElement;

    private bool? _active;
    
    public void SetElementActive(bool active)
    {
        _active = active;
        if (_uiElement == null)
        {
            Plugin.Logger.LogWarning($"UI element instance '{Id}' not found!");
            return;
        }
        _uiElement.SetActive(active);
    }

    private void BuildAndCreateUI(uGUI_PDA pda)
    {
        var ui = BuildElement(pda);
        PlaceElement?.Invoke(ui);
        _uiElement = ui.gameObject;
        _uiElement.SetActive(_active ?? ActiveByDefault);
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
    
    private void OnQuit()
    {
        _active = null;
    }
}