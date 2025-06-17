using System;
using Nautilus.Utility;
using PdaUpgradeChips.Patches;
using UnityEngine;

namespace PdaUpgradeChips.Data;

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

    protected string Id { get; }
    private Action<RectTransform> PlaceElement { get; }
    private PDATab Tab { get; }
    private bool ActiveByDefault { get; }
    protected GameObject UIElement;

    private bool? _active;
    
    public void SetElementActive(bool active)
    {
        _active = active;
        if (UIElement == null)
        {
            Plugin.Logger.LogWarning($"UI element instance '{Id}' not found!");
            return;
        }
        UIElement.SetActive(active);
    }

    private void BuildAndCreateUI(uGUI_PDA pda)
    {
        var ui = BuildElement(pda);
        PlaceElement?.Invoke(ui);
        UIElement = ui.gameObject;
        UIElement.SetActive(_active ?? ActiveByDefault);
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