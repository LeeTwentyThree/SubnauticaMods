using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using Nautilus.Assets;
using Nautilus.Assets.PrefabTemplates;
using Nautilus.Handlers;
using Nautilus.Utility;
using UnityEngine;

namespace TheRedPlague;

[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
[BepInDependency("com.snmodding.nautilus")]
public class Plugin : BaseUnityPlugin
{
    public new static ManualLogSource Logger { get; private set; }

    private static Assembly Assembly { get; } = Assembly.GetExecutingAssembly();
    
    public static AssetBundle AssetBundle { get; set; }
    public static Texture2D ZombieInfectionTexture { get; set; }

    private void Awake()
    {
        AssetBundle = AssetBundleLoadingUtils.LoadFromAssetsFolder(Assembly, "theredplague");

        ZombieInfectionTexture = AssetBundle.LoadAsset<Texture2D>("zombie_infection_bloody");
            
        // set project-scoped logger instance
        Logger = base.Logger;
        
        // register harmony patches, if there are any
        Harmony.CreateAndPatchAll(Assembly, $"{PluginInfo.PLUGIN_GUID}");
        Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");

        // Add new biome
        var infectedZoneSettings = BiomeUtils.CreateBiomeSettings(new Vector3(40, 30, 30), 0.5f,
            new Color(1, 0.5f, 0.5f, 1), 1f, new Color(1f, 0.1f, 0.1f), 0.05f, 25f, 0.5f, 0.5f, 20f);
        BiomeHandler.RegisterBiome("InfectedZone", infectedZoneSettings, new BiomeHandler.SkyReference("SkyGrassyPlateaus"));
        var infectedZonePrefab = new CustomPrefab(PrefabInfo.WithTechType("InfectedZoneVolume"));
        var infectedZoneTemplate = new AtmosphereVolumeTemplate(infectedZonePrefab.Info,
            AtmosphereVolumeTemplate.VolumeShape.Sphere, "InfectedZone");
        infectedZonePrefab.SetGameObject(infectedZoneTemplate);
        infectedZonePrefab.Register();
    }
}