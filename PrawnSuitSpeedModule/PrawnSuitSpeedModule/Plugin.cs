﻿using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using PrawnSuitSpeedModule.Items.Equipment;
using System.Reflection;

namespace PrawnSuitSpeedModule;
[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
[BepInDependency("com.snmodding.nautilus")]
public class Plugin : BaseUnityPlugin
{
    public new static ManualLogSource Logger { get; private set; }

    internal static Assembly Assembly { get; } = Assembly.GetExecutingAssembly();

    private void Awake()
    {
        // set project-scoped logger instance
        Logger = base.Logger;

        // Initialize custom prefabs
        InitializePrefabs();

        // register harmony patches, if there are any
        Harmony.CreateAndPatchAll(Assembly, $"{PluginInfo.PLUGIN_GUID}");
        Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
    }

    private void InitializePrefabs()
    {
        PrawnSuitSpeedUpgrade.Register();
    }
}