using System.Collections;
using HarmonyLib;
using UnityEngine;
using UWE;
using WeatherMod.Mono;

namespace WeatherMod.Patches;

[HarmonyPatch(typeof(Player), nameof(Player.Start))]
public static class SpawnWeatherManager
{
    [HarmonyPostfix]
    public static void Postfix()
    {
        CoroutineHost.StartCoroutine(SpawnRainCoroutine());
    }

    private static IEnumerator SpawnRainCoroutine()
    {
        yield return new WaitUntil(() => uGUI.main.loading.isLoading == false);
        var weatherManagerObject = new GameObject("WeatherManager");
        weatherManagerObject.AddComponent<CustomWeatherManager>();
    }
}