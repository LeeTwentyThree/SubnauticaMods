using UnityEngine;

namespace WeatherMod;

public static class FogManager
{
    private static float _currentTransitionDuration;

    private static bool _initializedValues;
    
    private static Color _defaultWaterReflectionColor;
    private static Color _defaultWaterRefractionColor;

    public static void ChangeCurrentFog(FogSettings newFogSettings)
    {
        var skyManager = uSkyManager.main;

        if (!_initializedValues)
        {
            _defaultWaterReflectionColor =
                WaterSurface._instance.surfaceMaterial.GetColor(ShaderPropertyID._ReflectionColor);
            _defaultWaterRefractionColor =
                WaterSurface._instance.surfaceMaterial.GetColor(ShaderPropertyID._RefractionColor);

            _initializedValues = true;
        }
        
        skyManager.skyFogDensity = newFogSettings.FogDensity;
        skyManager.skyFogColor = newFogSettings.GetColorGradient();
        
        WaterSurface._instance.surfaceMaterial.SetColor(ShaderPropertyID._ReflectionColor, ScaleColor(_defaultWaterReflectionColor, newFogSettings.WaterBrightness));
        WaterSurface._instance.surfaceMaterial.SetColor(ShaderPropertyID._RefractionColor, ScaleColor(_defaultWaterRefractionColor, newFogSettings.WaterBrightness));
    }

    private static Color ScaleColor(Color a, Color b)
    {
        return new Color(a.r * b.r, a.g * b.g, a.b * b.b, a.a * b.a);
    }

    private static Color ScaleColor(Color a, float b)
    {
        return new Color(a.r * b, a.g * b, a.b * b, a.a);
    }
}