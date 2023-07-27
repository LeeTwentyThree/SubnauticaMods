using BepInEx;
using BepInEx.Logging;
using DeathContainer.Items.Prefabs;
using HarmonyLib;
using System.Reflection;
using Nautilus.Handlers;
using UnityEngine;
using Nautilus.Utility;

namespace DeathContainer;

[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
[BepInDependency("com.snmodding.nautilus")]
public class Plugin : BaseUnityPlugin
{
    public new static ManualLogSource Logger { get; private set; }

    private static Assembly Assembly { get; } = Assembly.GetExecutingAssembly();

    internal static AssetBundle AssetBundle { get; } = AssetBundleLoadingUtils.LoadFromAssetsFolder(Assembly, "deathcontainerassets");

    internal static PingType DeathContainerPingType { get; } = EnumHandler.AddEntry<PingType>("DeathContainerPing")
        .WithIcon(new Atlas.Sprite(AssetBundle.LoadAsset<Sprite>("DeathContainerPing")));

    internal static Options Options { get; } = OptionsPanelHandler.RegisterModOptions<Options>();

    private void Awake()
    {
        
        // set project-scoped logger instance
        Logger = base.Logger;

        // Initialize custom prefabs
        InitializePrefabs();
        InitializeLanguage();

        // register harmony patches, if there are any
        Harmony.CreateAndPatchAll(Assembly, $"{PluginInfo.PLUGIN_GUID}");
        Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");

        SaveData.main = SaveDataHandler.RegisterSaveDataCache<SaveData>();
    }

    private void InitializePrefabs()
    {
        DeathContainerPrefab.Register();
    }

    private void InitializeLanguage()
    {
        LanguageHandler.SetLanguageLine("PingDeathContainerPing", "Death Container");
    }
}