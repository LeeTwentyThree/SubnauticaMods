using HarmonyLib;
using System;
using UnityEngine;

namespace TheRumbling.Patches;

[HarmonyPatch(typeof(WaterTemperatureSimulation))]
internal static class WaterTemperatureSimulationPatches
{
    [HarmonyPatch(nameof(WaterTemperatureSimulation.GetTemperature))]
    [HarmonyPatch(new Type[] { typeof(float), typeof(Vector3), typeof(float) })]
    [HarmonyPrefix]
    public static void Prefix(Vector3 wsPos, ref float baseTemperature)
    {
        var closestTitan = RumblingManager.GetNearestTitan(wsPos);
        if (closestTitan == null)
        {
            return;
        }
        float distanceToTitan = Vector3.Distance(wsPos, closestTitan.heatEmitter.position);
        baseTemperature += Mathf.Clamp((1 - distanceToTitan / Balance.TemperatureFalloffDistance) * Balance.MaxTemperature, 0, Balance.MaxTemperature);
    }
}
