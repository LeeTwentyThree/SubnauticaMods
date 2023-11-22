using HarmonyLib;
using WeatherMod.Mono;

namespace WeatherMod.Patches;

[HarmonyPatch(typeof(DayNightCycle)), HarmonyPatch(nameof(DayNightCycle.GetDayScalar))]
public class DaylightOverrider
{
    [HarmonyPostfix]
    public static void OverrideDayLight(ref float __result)
    {
        var weatherManager = CustomWeatherManager.Main;
        if (weatherManager == null || weatherManager.CurrentEvent == null) return;
        __result *= MainCamera.camera.transform.position.y > Ocean.GetOceanLevel()
            ? weatherManager.CurrentEvent.AboveWaterSunlightScale
            : weatherManager.CurrentEvent.BelowWaterSunlightScale;
    }
}