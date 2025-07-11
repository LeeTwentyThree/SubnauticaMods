﻿using System;
using PdaUpgradeChips.MonoBehaviours.UI;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace PdaUpgradeChips.Data;

public class CustomColorPicker : CustomPdaElement
{
    public delegate void OnColorChangedDelegate(Color color);

    public event OnColorChangedDelegate OnColorChanged;

    private ColorPicker _colorPickerInstance;

    public CustomColorPicker(string id, Action<RectTransform> placeElement, PDATab tab, bool activeByDefault) : base(id,
        placeElement, tab, activeByDefault)
    {
    }

    public Color GetActiveColor()
    {
        if (_colorPickerInstance == null)
        {
            Plugin.Logger.LogWarning("No color picker UI element found for " + Id + "!");
            return Color.white;
        }

        return _colorPickerInstance.ActiveColor;
    }

    protected override RectTransform BuildElement(uGUI_PDA pda)
    {
        var parent = base.BuildElement(pda);
        var colorPicker = Object.Instantiate(Plugin.Bundle.LoadAsset<GameObject>("ColorPickerPanel"));
        var pickerTransform = colorPicker.GetComponent<RectTransform>();
        pickerTransform.SetParent(parent, false);
        pickerTransform.localScale = Vector3.one;
        pickerTransform.anchoredPosition = Vector2.zero;
        pickerTransform.localRotation = Quaternion.identity;

        _colorPickerInstance = colorPicker.gameObject.AddComponent<ColorPicker>();
        _colorPickerInstance.OnColorChange += OnColorChangedCallback;
        _colorPickerInstance.previewImages = new[]
        {
            pickerTransform.Find("BarsParent/HueBar/Handle Slide Area/Handle").GetComponent<Image>(),

            pickerTransform.Find("BarsParent/SaturationBar/Handle Slide Area/Handle").GetComponent<Image>(),
            pickerTransform.Find("BarsParent/SaturationBar/CurrentColor").GetComponent<Image>(),

            pickerTransform.Find("BarsParent/ValueBar/Handle Slide Area/Handle").GetComponent<Image>(),
            pickerTransform.Find("BarsParent/ValueBar/ValueBarBackground").GetComponent<Image>()
        };

        _colorPickerInstance.saturationOverlay =
            pickerTransform.Find("BarsParent/SaturationBar/CurrentColor/SaturationOverlay").GetComponent<Image>();

        var hueSlider = pickerTransform.Find("BarsParent/HueBar").GetComponent<Slider>();
        hueSlider.onValueChanged
            .AddListener(_colorPickerInstance.OnHueValueChanged);
        var saturationSlider = pickerTransform.Find("BarsParent/SaturationBar").GetComponent<Slider>();
        saturationSlider.onValueChanged
            .AddListener(_colorPickerInstance.OnSaturationValueChanged);
        var valueSlider = pickerTransform.Find("BarsParent/ValueBar").GetComponent<Slider>();
        valueSlider.onValueChanged
            .AddListener(_colorPickerInstance.OnBrightnessValueChanged);

        _colorPickerInstance.hueSlider = hueSlider;
        _colorPickerInstance.saturationSlider = saturationSlider;
        _colorPickerInstance.valueSlider = valueSlider;
        
        _colorPickerInstance.playerPrefKey = Id;

        pickerTransform.Find("ExpandButton").GetComponent<Button>().onClick.AddListener(_colorPickerInstance.OnExpandButtonPressed);
        _colorPickerInstance.barsPanel = pickerTransform.Find("BarsParent").gameObject;
        
        return parent;
    }

    private void OnColorChangedCallback(Color color)
    {
        OnColorChanged?.Invoke(color);
    }
}