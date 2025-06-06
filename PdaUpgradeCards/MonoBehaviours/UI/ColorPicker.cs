using System;
using UnityEngine;
using UnityEngine.UI;

namespace PdaUpgradeCards.MonoBehaviours.UI;

public class ColorPicker : MonoBehaviour
{
    public Image[] previewImages;
    public Image saturationOverlay;

    public Slider hueSlider;
    public Slider saturationSlider;
    public Slider valueSlider;

    public string playerPrefKey;
    
    private float _hue;
    private float _saturation;
    private float _value;

    public float ValueScale
    {
        private get => _valueScale;
        set
        {
            _valueScale = value;
            UpdateColor();
        }
    }

    private float _valueScale;
    
    private Color _displayedColor;
    private Color _pureColor;

    public Color ActiveColor => _displayedColor;
    
    public delegate void OnColorChangeDelegate(Color color);
    
    public OnColorChangeDelegate OnColorChange;
    
    private string HKey => playerPrefKey + "_H";
    private string SKey => playerPrefKey + "_S";
    private string VKey => playerPrefKey + "_V";

    private bool _canSave;

    public void SetColor(Color color)
    {
        var adjustedColor = (color / ValueScale).WithAlpha(1);
        Color.RGBToHSV(adjustedColor, out _hue, out _saturation, out _value);
        hueSlider.value = _hue;
        saturationSlider.value = _saturation;
        valueSlider.value = _value;
        UpdateColor();
    }

    private void Start()
    {
        _canSave = true;
        if (string.IsNullOrEmpty(playerPrefKey)) return;
        if (!PlayerPrefs.HasKey(HKey) || !PlayerPrefs.HasKey(SKey) || !PlayerPrefs.HasKey(VKey)) return;
        _hue = PlayerPrefs.GetFloat(HKey);
        _saturation = PlayerPrefs.GetFloat(SKey);
        _value = PlayerPrefs.GetFloat(VKey);
        hueSlider.value = _hue;
        saturationSlider.value = _saturation;
        valueSlider.value = _value;
    }

    public void OnHueValueChanged(float hue)
    {
        _hue = hue;
        UpdateColor();
    }

    public void OnSaturationValueChanged(float saturation)
    {
        _saturation = saturation;
        UpdateColor();
    }

    public void OnBrightnessValueChanged(float value)
    {
        _value = value;
        UpdateColor();
    }

    private void UpdateColor()
    {
        _pureColor = Color.HSVToRGB(_hue, _saturation, _value);
        _displayedColor = new Color(_pureColor.r  * ValueScale, _pureColor.g  * ValueScale, _pureColor.b  * ValueScale);
        OnColorChange?.Invoke(_displayedColor);
        foreach (var handle in previewImages)
        {
            handle.color = _pureColor;
        }
        
        if (saturationOverlay != null)
            saturationOverlay.color = new Color(1, 1, 1, _value);
    }

    private void OnDisable()
    {
        if (!_canSave)
            return;
        
        if (string.IsNullOrEmpty(playerPrefKey))
            return;
        
        PlayerPrefs.SetFloat(HKey, _hue);
        PlayerPrefs.SetFloat(SKey, _saturation);
        PlayerPrefs.SetFloat(VKey, _value);
    }
}