using UnityEngine;

namespace WeatherMod;

public class FogSettings
{
    public float FogDensity { get; set; } = 0.0002f;
    
    public Color Color1 { get; set; } = new Color(0.003921569f, 0.007843138f, 0.01568628f);
    public Color Color2 { get; set; } = new Color(0.2196078f, 0.5058824f, 1);
    public Color Color3 { get; set; } = new Color(0.2196078f, 0.5058824f, 1f);
    public Color Color4 { get; set; } = new Color(0.003921569f, 0.007843138f, 0.01568628f, 1f);
    public float WaterBrightness { get; set; } = 1f;
    public float SunlightBrightnessAboveWater { get; set; } = 1f;
    public float SunlightBrightnessBelowWater { get; set; } = 1f;

    public FogSettings()
    {
    }

    public FogSettings(float fogDensity, Color mainFogColor, float waterBrightness = 1f, float sunlightBrightnessAboveWater = 1f, float sunlightBrightnessBelowWater = 1f)
    {
        FogDensity = fogDensity;
        SetMainFogColor(mainFogColor);
        WaterBrightness = waterBrightness;
        SunlightBrightnessAboveWater = sunlightBrightnessAboveWater;
        SunlightBrightnessBelowWater = sunlightBrightnessBelowWater;
    }

    // Sets the fog colors that actually matter
    public void SetMainFogColor(Color mainFogColor)
    {
        Color2 = mainFogColor;
        Color3 = mainFogColor;
    }

    public Gradient GetColorGradient()
    {
        var gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] {new GradientColorKey(Color1, 0.2558785f), new GradientColorKey(Color2, 0.3000076f), new GradientColorKey(Color3, 0.7000076f), new GradientColorKey(Color4, 0.7499962f)},
            new GradientAlphaKey[] {new GradientAlphaKey(1, 0), new GradientAlphaKey(1, 1)} );
        return gradient;
    }
    
    /* Default values:
     * Alpha is a constant value of 1
     * Times:
     * 0.2558785
     * 0.3000076
     * 0.7000076
     * 0.7499962
     */
}