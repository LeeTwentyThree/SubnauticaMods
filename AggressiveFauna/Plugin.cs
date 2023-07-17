using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using Nautilus.Handlers;
using System.Reflection;
using UnityEngine;
using Nautilus.Utility;

namespace AggressiveFauna;

/* Light version of DeathRun, focusing on creature aggression.
 * Credit to Cattlesquat for original code
 * Modified by Lee23
 * Not compatible with DeathRun */

[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
[BepInDependency("com.snmodding.nautilus")]
public class Plugin : BaseUnityPlugin
{
    internal static Config config;

    internal static Assembly assembly = Assembly.GetExecutingAssembly();

    internal static AssetBundle bundle;

    public new static ManualLogSource Logger { get; private set; }

    private static Assembly Assembly { get; } = Assembly.GetExecutingAssembly();

    private void Awake()
    {
        // set project-scoped logger instance
        Logger = base.Logger;

        // register harmony patches, if there are any
        Harmony.CreateAndPatchAll(Assembly, $"{PluginInfo.PLUGIN_GUID}");
        Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");

        bundle = AssetBundleLoadingUtils.LoadFromAssetsFolder(assembly, "aggressivefauna");

        config = OptionsPanelHandler.RegisterModOptions<Config>();

        ModAudio.PatchAudio();
    }
}