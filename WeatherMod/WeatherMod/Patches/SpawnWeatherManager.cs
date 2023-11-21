using HarmonyLib;
using UnityEngine;
using WeatherMod.Mono;

namespace WeatherMod.Patches;

[HarmonyPatch(typeof(Player), nameof(Player.Start))]
public static class SpawnWeatherManager
{
    [HarmonyPostfix]
    public static void Postfix()
    {
        var weatherManagerObject = new GameObject("WeatherManager");
        weatherManagerObject.AddComponent<WeatherMod.Mono.CustomWeatherManager>();
    }
}