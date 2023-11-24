using HarmonyLib;

namespace WeatherMod.Patches;

[HarmonyPatch(typeof(UpdateSwimCharge), nameof(UpdateSwimCharge.na))]
public static class WaterDropsOnScreenSpawner
{
    
}