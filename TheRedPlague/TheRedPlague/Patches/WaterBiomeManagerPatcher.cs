using System.Linq;
using HarmonyLib;
using Nautilus.Utility;
using UnityEngine;

namespace TheRedPlague.Patches;

[HarmonyPatch(typeof(WaterBiomeManager))]
public static class WaterBiomeManagerPatcherPatcher
{
    [HarmonyPatch(nameof(WaterBiomeManager.Start))]
    [HarmonyPostfix]
    public static void StartPostfix(WaterBiomeManager __instance)
    {
        foreach (var settings in __instance.biomeSettings)
        {
            if (settings.name != "dunes") continue;
            
            settings.settings = BiomeUtils.CreateBiomeSettings(
                new Vector3(8, 11f, 13f),
                0.5f,
                new Color(0.341f, 0.427f, 0.447f),
                0.2f,
                new Color(1, 0.906f, 0.96f),
                0.02f,
                25,
                1,
                1,
                20
            );
            return;
        }
    }
}