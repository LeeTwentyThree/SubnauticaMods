using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using Nautilus.Handlers;
using Nautilus.Utility;
using UnityEngine;
using YoutoozGargStatue.Items.Equipment;

namespace YoutoozGargStatue;

[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
[BepInDependency("com.snmodding.nautilus", "1.0.0.36")]
public class Plugin : BaseUnityPlugin
{
    public new static ManualLogSource Logger { get; private set; }

    private static Assembly Assembly { get; } = Assembly.GetExecutingAssembly();
    
    public static AssetBundle Bundle { get; } = AssetBundleLoadingUtils.LoadFromAssetsFolder(Assembly, "youtoozgarg");

    private void Awake()
    {
        LanguageHandler.RegisterLocalizationFolder();
        
        // set project-scoped logger instance
        Logger = base.Logger;

        // Initialize custom prefabs
        InitializePrefabs();

        Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
    }

    private void InitializePrefabs()
    {
        GargStatuePrefab.Register();
    }
}