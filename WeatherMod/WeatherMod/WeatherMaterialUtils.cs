using Nautilus.Utility;
using UnityEngine;

namespace WeatherMod;

public class WeatherMaterialUtils
{
    private static string[] _particleKeywords = new[]
    {
        "FX_DEFORM_OFF",
        "FX_LIGHTMODE_PIXEL",
        "FX_LIGHT_2SIDED",
        "FX_NEARCLIP",
        "FX_SOFTEDGES",
        "FX_UNDERWATER_OFF",
        "WBOIT"
    };
    
    public static void ApplyRainMaterial(Renderer renderer)
    {
        var material = renderer.material;
        material.shader = MaterialUtils.Shaders.ParticlesUBER;
        material.color = new Color(0.6f, 0.6f, 0.6f, 0.761f);
        material.SetColor(ShaderPropertyID._ColorStrengthAtNight, new Color(0.1f, 0.1f, 0.1f, 0.6f));
        foreach (var keywords in _particleKeywords)
            material.EnableKeyword(keywords);
    }
}