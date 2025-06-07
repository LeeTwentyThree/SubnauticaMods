using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using Nautilus.Assets;
using Nautilus.Handlers;
using Nautilus.Utility;
using PdaUpgradeCards.Data;
using PdaUpgradeCards.MonoBehaviours;
using PdaUpgradeCards.MonoBehaviours.Upgrades;
using PdaUpgradeCards.Prefabs;
using UnityEngine;

namespace PdaUpgradeCards;

[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
[BepInDependency("com.snmodding.nautilus", "1.0.0.37")]
[BepInDependency("com.lee23.theredplague", BepInDependency.DependencyFlags.SoftDependency)]
[BepInDependency("com.aci.hydra", BepInDependency.DependencyFlags.SoftDependency)]
[BepInDependency("com.aotu.returnoftheancients", BepInDependency.DependencyFlags.SoftDependency)]
public class Plugin : BaseUnityPlugin
{
    public new static ManualLogSource Logger { get; private set; }

    private static Assembly Assembly { get; } = Assembly.GetExecutingAssembly();

    internal static AssetBundle Bundle { get; } =
        AssetBundleLoadingUtils.LoadFromAssetsFolder(Assembly, "pdaupgradechips");
    
    internal static ModConfig ModConfig { get; private set; }
    
    private void Awake()
    {
        // set project-scoped logger instance
        Logger = base.Logger;

        LanguageHandler.RegisterLocalizationFolder();

        ModConfig = OptionsPanelHandler.RegisterModOptions<ModConfig>();
        
        // Initialize custom prefabs
        InitializePrefabs();

        // register harmony patches, if there are any
        Harmony.CreateAndPatchAll(Assembly, $"{PluginInfo.PLUGIN_GUID}");
        Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
        
        PdaUpgradesAPI.Register();
        PdaUpgradesManager.RegisterSaveData();
        
        PdaElements.RegisterAll();

        StartCoroutine(PdaMusicDatabase.RefreshMusicDatabase());
    }

    private static void InitializePrefabs()
    {
        PdaUpgradesContainerPrefab.Register();
        
        new UpgradeCardPrefab<MusicUpgrade>(PrefabInfo.WithTechType("PdaMusicUpgrade")
            .WithIcon(Bundle.LoadAsset<Sprite>("UpgradeIcon_MusicPlayer"))).Register();
        new UpgradeCardPrefab<ColorizerUpgrade>(PrefabInfo.WithTechType("PdaColorizerUpgrade")
            .WithIcon(Bundle.LoadAsset<Sprite>("UpgradeIcon_PDGay"))).Register();
    }
}