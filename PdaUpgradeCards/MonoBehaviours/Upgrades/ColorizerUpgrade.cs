using PdaUpgradeCards.Data;
using UnityEngine;

namespace PdaUpgradeCards.MonoBehaviours.Upgrades;

public class ColorizerUpgrade : UpgradeChipBase
{
    private PdaColorChanger _colorChanger;
    
    private void Start()
    {
        var pdaObject = Player.main.pda.transform.Find("Mesh").gameObject;
        var materials = pdaObject.GetComponent<Renderer>().materials;
        var borderMaterial = materials[1];
        var customScreenMaterial = materials[2];
        
        _colorChanger = pdaObject.EnsureComponent<PdaColorChanger>();
        _colorChanger.borderMaterial = borderMaterial;
        _colorChanger.screenMaterial = customScreenMaterial;
        _colorChanger.whiteTexture = Plugin.Bundle.LoadAsset<Texture2D>("PdaScreenIllumWhite");

        PdaElements.PdaColorPicker.OnColorChanged += OnColorChanged;
        PdaElements.PdaColorPicker.SetElementActive(true);
        
        _colorChanger.SetCustomColor(PdaElements.PdaColorPicker.GetActiveColor());
    }

    private void OnDestroy()
    {
        if (_colorChanger)
            _colorChanger.ResetColor();
        Destroy(_colorChanger);
        PdaElements.PdaColorPicker.OnColorChanged -= OnColorChanged;
        PdaElements.PdaColorPicker.SetElementActive(false);
    }

    private void OnColorChanged(Color color)
    {
        _colorChanger.SetCustomColor(color);
    }
}