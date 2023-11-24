using System.Collections;
using HarmonyLib;
using UnityEngine;
using UWE;
using WeatherMod.Mono;
using WeatherMod.WeatherEvents;

namespace WeatherMod.Patches;

[HarmonyPatch(typeof(ReactOnClick), nameof(ReactOnClick.OnHandClick))]
public static class MarkEasterEgg
{
    [HarmonyPostfix]
    public static void Postfix(ReactOnClick __instance)
    {
        if (__instance.gameObject.name.Contains("Marki"))
        {
            CustomWeatherManager.Main?.SetWeather(new GoldenThunderstorm());
        }
    }
}