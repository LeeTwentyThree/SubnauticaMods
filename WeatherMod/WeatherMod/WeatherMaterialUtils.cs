using Nautilus.Utility;
using UnityEngine;

namespace WeatherMod;

public class WeatherMaterialUtils
{
    public static void ApplyRainMaterial(Renderer renderer)
    {
        var material = renderer.material;
        material.shader = MaterialUtils.Shaders.ParticlesUBER;
        material.color = new Color(0.5f, 0.5f, 0.5f, 0.761f);
        material.SetColor(ShaderPropertyID._ColorStrengthAtNight, new Color(0.1f, 0.1f, 0.1f, 0.6f));
    }
}