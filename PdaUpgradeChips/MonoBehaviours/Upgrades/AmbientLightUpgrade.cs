using PdaUpgradeChips.Data;
using UnityEngine;

namespace PdaUpgradeChips.MonoBehaviours.Upgrades;

public class AmbientLightUpgrade : UpgradeChipBase
{
    private Light _light;
    
    private void Start()
    {
        _light = Player.main.gameObject.AddComponent<Light>();
        _light.color = PdaElements.PdaColorPicker.GetActiveColor();
        _light.intensity = 0.4f;
        _light.shadows = LightShadows.Soft;
        _light.type = LightType.Point;
        _light.range = 25;
        
        PdaElements.PdaColorPicker.OnColorChanged += OnColorChanged;
    }

    private void OnDestroy()
    {
        Destroy(_light);
        PdaElements.PdaColorPicker.OnColorChanged -= OnColorChanged;
    }

    private void OnColorChanged(Color color)
    {
        if (_light)
            _light.color = color;
    }
}