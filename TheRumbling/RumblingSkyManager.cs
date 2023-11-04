using UnityEngine;
using uSky;

namespace TheRumbling;

public static class RumblingSkyManager
{
    private static bool _initialized;
    private static Gradient _defaultSkyFogColorGradient;
    private static Gradient _defaultMeanSkyColorGradient;
    private static float _defaultFogDensity;
    private static Color _defaultSkyTint;
    private static Color _defaultReflectionColor;
    private static Color _defaultRefractionColor;
    private static Gradient _rumblingSkyFogColorGradient;
    private static Gradient _rumblingMeanSkyColorGradient;
    private static float _defaultSunIntensity;

    public static void ToggleRumblingSkybox(bool useRumblingSkyboxSettings)
    {
        var skyManager = uSkyManager.main;
        var waterSurfaceMaterial = WaterSurface._instance.surfaceMaterial;

        if (!_initialized)
        {
            _defaultSkyFogColorGradient = skyManager.skyFogColor;
            _defaultMeanSkyColorGradient = skyManager.meanSkyColor;
            _defaultFogDensity = skyManager.skyFogDensity;
            _defaultSkyTint = skyManager.SkyTint;
            _defaultReflectionColor = waterSurfaceMaterial.GetColor(ShaderPropertyID._ReflectionColor);
            _defaultRefractionColor = waterSurfaceMaterial.GetColor(ShaderPropertyID._RefractionColor);

            _rumblingSkyFogColorGradient = new Gradient();
            _rumblingMeanSkyColorGradient = new Gradient();
            _rumblingSkyFogColorGradient.SetKeys(
                new GradientColorKey[] {new GradientColorKey(new Color(0.5f, 0.1f, 0f), 1)},
                new GradientAlphaKey[] {new GradientAlphaKey(0, 1), new GradientAlphaKey(1, 1)});
            _rumblingMeanSkyColorGradient.SetKeys(
                new GradientColorKey[] {new GradientColorKey(new Color(0.5f, 0.1f, 0f), 1)},
                new GradientAlphaKey[] {new GradientAlphaKey(0, 1), new GradientAlphaKey(1, 1)});

            _defaultSunIntensity = uSkyManager.main.GetComponent<uSkyLight>().SunIntensity;
            _initialized = true;
        }

        skyManager.skyFogColor = useRumblingSkyboxSettings ? _rumblingSkyFogColorGradient : _defaultSkyFogColorGradient;
        skyManager.meanSkyColor =
            useRumblingSkyboxSettings ? _rumblingMeanSkyColorGradient : _defaultMeanSkyColorGradient;
        skyManager.skyFogDensity = useRumblingSkyboxSettings ? 1E-3f : _defaultFogDensity;
        skyManager.SkyTint = useRumblingSkyboxSettings ? Color.red : _defaultSkyTint;

        uSkyManager.main.GetComponent<uSkyLight>().SunIntensity =
            useRumblingSkyboxSettings ? 0.4f : _defaultSunIntensity;

        waterSurfaceMaterial.SetColor(ShaderPropertyID._ReflectionColor,
            useRumblingSkyboxSettings ? new Color(0.2f, 0.1f, 0.1f, 1f) : _defaultReflectionColor);
        waterSurfaceMaterial.SetColor(ShaderPropertyID._RefractionColor,
            useRumblingSkyboxSettings ? new Color(0.2f, 0.1f, 0.1f, 1f) : _defaultRefractionColor);

        var clouds = GameObject.Find("x_Clouds(Clone)");
        if (clouds)
            clouds.SetActive(!useRumblingSkyboxSettings);
    }
}