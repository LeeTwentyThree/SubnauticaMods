using UnityEngine;

namespace PdaUpgradeCards.MonoBehaviours;

public class PdaColorChanger : MonoBehaviour
{
    private Texture _originalIllumTexture;
    
    public Material screenMaterial;
    public Texture2D whiteTexture;

    private bool _whiteTextureActive;
    
    public void SetCustomColor(Color color)
    {
        if (!_whiteTextureActive)
        {
            _originalIllumTexture = screenMaterial.GetTexture(ShaderPropertyID._Illum);
            screenMaterial.SetTexture(ShaderPropertyID._Illum, whiteTexture);
            _whiteTextureActive = true;
        }

        screenMaterial.color = color;
        screenMaterial.SetColor(ShaderPropertyID._SpecColor, color);
        screenMaterial.SetColor(ShaderPropertyID._GlowColor, color); }

    public void ResetColor()
    {
        if (!screenMaterial)
            return;
        
        if (_whiteTextureActive && _originalIllumTexture)
            screenMaterial.SetTexture(ShaderPropertyID._Illum, _originalIllumTexture);
        
        screenMaterial.color = Color.white;
        screenMaterial.SetColor(ShaderPropertyID._SpecColor, Color.white);
        screenMaterial.SetColor(ShaderPropertyID._GlowColor, Color.white);
    }

    private void OnDestroy()
    {
        ResetColor();
    }
}