using UnityEngine;

namespace WeatherMod;

public static class FogManager
{
    private static float _currentTransitionDuration = 4f;

    private static bool _initializedValues;

    private static Color _defaultWaterReflectionColor;
    private static Color _defaultWaterRefractionColor;

    private static float _timeLastTransitionBegun;

    private static float _lastFogDensity;
    private static float _lastWaterBrightness = 1f;
    private static Color[] _lastGradientColors;
    private static float _lastExposure = 0.66f;
    private static float _lastSunlightBrightnessBelowWater = 1f;

    private static float _newFogDensity;
    private static float _newWaterBrightness = 1f;
    private static Color[] _newGradientColors;
    private static float _newExposure = 0.66f;
    private static float _newSunlightBrightnessBelowWater = 1f;
    
    private static Gradient _skyFogColorGradient;

    public static float Exposure { get; private set; } = 1f;
    public static float SunlightBrightnessBelowWater { get; private set; } = 1f;

    public static void ChangeCurrentFog(FogSettings newFogSettings)
    {
        var skyManager = uSkyManager.main;

        if (!_initializedValues)
        {
            _defaultWaterReflectionColor =
                WaterSurface._instance.surfaceMaterial.GetColor(ShaderPropertyID._ReflectionColor);
            _defaultWaterRefractionColor =
                WaterSurface._instance.surfaceMaterial.GetColor(ShaderPropertyID._RefractionColor);

            _skyFogColorGradient = uSkyManager.main.skyFogColor;
            
            _initializedValues = true;
        }

        _lastFogDensity = skyManager.skyFogDensity;
        _lastGradientColors = new[]
        {
            skyManager.skyFogColor.colorKeys[0].color, skyManager.skyFogColor.colorKeys[1].color,
            skyManager.skyFogColor.colorKeys[2].color, skyManager.skyFogColor.colorKeys[3].color
        };
        _lastWaterBrightness = _newWaterBrightness;
        _lastSunlightBrightnessBelowWater = _newSunlightBrightnessBelowWater;
        _lastExposure = _newExposure;

        _newFogDensity = newFogSettings.FogDensity;
        _newGradientColors = new[]
            {newFogSettings.Color1, newFogSettings.Color2, newFogSettings.Color3, newFogSettings.Color4};
        _newWaterBrightness = newFogSettings.WaterBrightness;
        _newSunlightBrightnessBelowWater = newFogSettings.SunlightBrightnessBelowWater;
        _newExposure = newFogSettings.Exposure;

        _timeLastTransitionBegun = Time.time;

        FogManagerUpdater.Ensure();
    }

    private static Color ScaleColor(Color a, Color b)
    {
        return new Color(a.r * b.r, a.g * b.g, a.b * b.b, a.a * b.a);
    }

    private static Color ScaleColor(Color a, float b)
    {
        return new Color(a.r * b, a.g * b, a.b * b, a.a);
    }
    
    internal static void Update()
    {
        if (Time.time > _timeLastTransitionBegun + _currentTransitionDuration + 1.1987f)
            return;

        var lerpT = Mathf.Clamp01((Time.time - _timeLastTransitionBegun) / _currentTransitionDuration);
        
        var skyManager = uSkyManager.main;

        skyManager.skyFogDensity = Mathf.Lerp(_lastFogDensity, _newFogDensity, lerpT);
        
        _skyFogColorGradient.colorKeys = new[]
        {
            new GradientColorKey(Color.Lerp(_lastGradientColors[0], _newGradientColors[0], lerpT), 0.2558785f),
            new GradientColorKey(Color.Lerp(_lastGradientColors[1], _newGradientColors[1], lerpT), 0.3000076f),
            new GradientColorKey(Color.Lerp(_lastGradientColors[2], _newGradientColors[2], lerpT), 0.7000076f),
            new GradientColorKey(Color.Lerp(_lastGradientColors[3], _newGradientColors[3], lerpT), 0.7499962f)
        };
        
        WaterSurface._instance.surfaceMaterial.SetColor(ShaderPropertyID._ReflectionColor,
            ScaleColor(_defaultWaterReflectionColor, Mathf.Lerp(_lastWaterBrightness, _newWaterBrightness, lerpT)));
        WaterSurface._instance.surfaceMaterial.SetColor(ShaderPropertyID._RefractionColor,
            ScaleColor(_defaultWaterRefractionColor, Mathf.Lerp(_lastWaterBrightness, _newWaterBrightness, lerpT)));

        Exposure = Mathf.Lerp(_lastExposure, _newExposure, lerpT);
        skyManager.Exposure = Exposure;
        SunlightBrightnessBelowWater = Mathf.Lerp(_lastSunlightBrightnessBelowWater, _newSunlightBrightnessBelowWater, lerpT);
    }
}