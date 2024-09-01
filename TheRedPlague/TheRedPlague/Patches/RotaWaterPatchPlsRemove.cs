using HarmonyLib;
using UnityEngine;

namespace TheRedPlague.Patches;

[HarmonyPatch]
public static class RotaWaterPatchPlsRemove
{
    private const string VoidBiomeName = "void";

    [HarmonyPostfix]
    [HarmonyPatch(typeof(WaterBiomeManager), nameof(WaterBiomeManager.Start))]
    public static void WaterBiomeManager_Start_Postfix(WaterBiomeManager __instance)
    {
        if (__instance.biomeSkies.Count <
            3) //a check to see if the main menu water biome manager is loaded, rather than the main one. if we don't end the method here, the game will throw an exception.
        {
            return;
        }

        var voidWaterscapeSettings = new WaterscapeVolume.Settings()
        {
            absorption = new Vector3(8, 5, 3),
            scattering = 0.075f,
            scatteringColor = new Color(0.2f, 0.3f, 0.5f),
            murkiness = 0.8f,
            startDistance = 350f,
            sunlightScale = 0.3f,
            ambientScale = 0.5f,
            emissiveScale = 0f,
            temperature = 15f
        };

        PatchBiomeFog(__instance, VoidBiomeName, voidWaterscapeSettings, __instance.biomeSkies[37]);
    }

    private static void PatchBiomeFog(WaterBiomeManager waterBiomeManager, string biomeName,
        WaterscapeVolume.Settings waterScapeSettings, mset.Sky sky)
    {
        if (!waterBiomeManager.biomeLookup.ContainsKey(biomeName))
        {
            GameObject skyPrefab = null;
            if (sky)
            {
                skyPrefab = sky.gameObject;
            }

            WaterBiomeManager.BiomeSettings biomeSettings = new WaterBiomeManager.BiomeSettings()
            {
                name = biomeName,
                skyPrefab = skyPrefab,
                settings = waterScapeSettings
            };
            waterBiomeManager.biomeSkies.Add(sky);
            waterBiomeManager.biomeSettings.Add(biomeSettings);
            var biomeIndex = waterBiomeManager.biomeSettings.Count - 1;

            waterBiomeManager.biomeLookup.Add(biomeName.ToLower(), biomeIndex);
        }
    }
}