using UnityEngine;

namespace PdaUpgradeChips.MonoBehaviours;

public class MinimapPostProcessing : MonoBehaviour
{
    public Shader shader;

    private Material _material;
    
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (_material == null)
        {
            _material = new Material(shader);
            _material.hideFlags = HideFlags.DontSave;
        }
        _material.SetFloat(ShaderPropertyID._SonarPingDistance, Plugin.Config.MinimapSonarEffectModifier);
        Graphics.Blit(source, destination, _material);
    }
}