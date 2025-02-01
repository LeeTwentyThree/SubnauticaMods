using System.Linq;
using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using Nautilus.Handlers;
using UnityEngine;

namespace ChangeBiomeFog;

[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
[BepInDependency("com.snmodding.nautilus")]
public class Plugin : BaseUnityPlugin
{
    public new static ManualLogSource Logger { get; private set; }

    private static Assembly Assembly { get; } = Assembly.GetExecutingAssembly();

    private void Awake()
    {
        // set project-scoped logger instance
        Logger = base.Logger;

        ConsoleCommandsHandler.RegisterConsoleCommand("setbiomefog", SetBiomeFog);

        // register harmony patches, if there are any
        Harmony.CreateAndPatchAll(Assembly, $"{PluginInfo.PLUGIN_GUID}");
        Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
    }

    private static void SetBiomeFog(string biomeName, float absorptionR, float absorptionG, float absorptionB,
        float scattering, float scatteringR, float scatteringG, float scatteringB, float murkiness, float startDistance,
        float sunlightScale, float ambientScale, float emissiveR, float emissiveG, float emissiveB, float emissiveScale)
    {
        var biomeManager = WaterBiomeManager.main;
        var voidSettings = biomeManager.biomeSettings.SingleOrDefault(settings => settings.name == biomeName);
        if (voidSettings == null)
        {
            ErrorMessage.AddMessage($"Failed to find biome settings for biome of name '{biomeName}'!");
            return;
        }

        var settings = voidSettings.settings;
        settings.absorption = new Vector3(absorptionR, absorptionG, absorptionB);
        settings.scattering = scattering;
        settings.scatteringColor = new Color(scatteringR, scatteringG, scatteringB);
        settings.murkiness = murkiness;
        settings.startDistance = startDistance;
        settings.sunlightScale = sunlightScale;
        settings.ambientScale = ambientScale;
        settings.emissive = new Color(emissiveR, emissiveG, emissiveB);
        settings.emissiveScale = emissiveScale;
        WaterBiomeManager.main.Rebuild();
    }
}