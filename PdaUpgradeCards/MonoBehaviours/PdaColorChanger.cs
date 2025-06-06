using UnityEngine;

namespace PdaUpgradeCards.MonoBehaviours;

public class PdaColorChanger : MonoBehaviour
{
    private Texture _originalIllumTexture;

    public Material borderMaterial;
    public Material screenMaterial;
    public Texture2D whiteTexture;

    private bool _whiteTextureActive;

    public void SetCustomColor(Color color)
    {
        if (!_whiteTextureActive)
        {
            _originalIllumTexture = screenMaterial.GetTexture(ShaderPropertyID._Illum);
            borderMaterial.SetTexture(ShaderPropertyID._Illum, whiteTexture);
            screenMaterial.SetTexture(ShaderPropertyID._Illum, whiteTexture);
            _whiteTextureActive = true;
        }

        screenMaterial.color = color;
        screenMaterial.SetColor(ShaderPropertyID._SpecColor, color);
        screenMaterial.SetColor(ShaderPropertyID._GlowColor, color);
        
        borderMaterial.color = color;
        borderMaterial.SetColor(ShaderPropertyID._SpecColor, color);
        borderMaterial.SetColor(ShaderPropertyID._GlowColor, color);
    }

    public void ResetColor()
    {
        if (!screenMaterial)
            return;

        if (_whiteTextureActive && _originalIllumTexture)
        {
            screenMaterial.SetTexture(ShaderPropertyID._Illum, _originalIllumTexture);
            borderMaterial.SetTexture(ShaderPropertyID._Illum, _originalIllumTexture);
        }

        screenMaterial.color = Color.white;
        screenMaterial.SetColor(ShaderPropertyID._SpecColor, Color.white);
        screenMaterial.SetColor(ShaderPropertyID._GlowColor, Color.white);
        
        borderMaterial.color = Color.white;
        borderMaterial.SetColor(ShaderPropertyID._SpecColor, Color.white);
        borderMaterial.SetColor(ShaderPropertyID._GlowColor, Color.white);
    }

    private void OnDestroy()
    {
        ResetColor();
    }
}