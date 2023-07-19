using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using LiveMinimap.Items.Equipment;
using Nautilus.Handlers;
using Nautilus.Utility;
using System.Reflection;
using UnityEngine;

namespace LiveMinimap;
[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
[BepInDependency("com.snmodding.nautilus")]
public class Plugin : BaseUnityPlugin
{
    public new static ManualLogSource Logger { get; private set; }

    private static Assembly Assembly { get; } = Assembly.GetExecutingAssembly();

    internal static AssetBundle bundle;

    internal static Config config = OptionsPanelHandler.RegisterModOptions<Config>();

    private void Awake()
    {
        // set project-scoped logger instance
        Logger = base.Logger;

        // Initialize custom prefabs
        InitializePrefabs();

        bundle = AssetBundleLoadingUtils.LoadFromAssetsFolder(Assembly, "liveminimap");

        // register harmony patches, if there are any
        Harmony.CreateAndPatchAll(Assembly, $"{PluginInfo.PLUGIN_GUID}");
        Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
    }

    private void InitializePrefabs()
    {
        MinimapChipPrefab.Register();
    }
}