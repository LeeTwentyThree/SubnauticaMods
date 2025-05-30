using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using Nautilus.Assets;
using Nautilus.Utility;
using PdaUpgradeChips.MonoBehaviours.Upgrades;
using PdaUpgradeChips.Prefabs;
using UnityEngine;

namespace PdaUpgradeChips;

[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
[BepInDependency("com.snmodding.nautilus")]
public class Plugin : BaseUnityPlugin
{
    public new static ManualLogSource Logger { get; private set; }

    private static Assembly Assembly { get; } = Assembly.GetExecutingAssembly();

    internal static AssetBundle Bundle { get; } =
        AssetBundleLoadingUtils.LoadFromAssetsFolder(Assembly, "pdaupgradechips");
    
    private void Awake()
    {
        // set project-scoped logger instance
        Logger = base.Logger;

        // Initialize custom prefabs
        InitializePrefabs();

        // register harmony patches, if there are any
        Harmony.CreateAndPatchAll(Assembly, $"{PluginInfo.PLUGIN_GUID}");
        Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
        
        PdaUpgradesAPI.Register();
    }

    private void InitializePrefabs()
    {
        PdaUpgradesContainerPrefab.Register();
        
        new UpgradeChipPrefab<TestUpgrade>(PrefabInfo.WithTechType("TestChip")).Register();
    }
}