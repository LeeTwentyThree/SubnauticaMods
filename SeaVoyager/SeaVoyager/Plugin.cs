using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System.Reflection;
using SeaVoyager.Prefabs.Fragments;
using UnityEngine;
using Nautilus.Utility;
using Nautilus.Handlers;
using System.Collections.Generic;
using SeaVoyager.Prefabs.Vehicles;

namespace SeaVoyager;
[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
[BepInDependency("com.snmodding.nautilus")]
public class Plugin : BaseUnityPlugin
{
    public new static ManualLogSource Logger { get; private set; }

    private static Assembly Assembly { get; } = Assembly.GetExecutingAssembly();

    public static AssetBundle assetBundle;

    public static SeaVoyagerConfig config = OptionsPanelHandler.RegisterModOptions<SeaVoyagerConfig>();

    private void Awake()
    {
        // set project-scoped logger instance
        Logger = base.Logger;

        assetBundle = AssetBundleLoadingUtils.LoadFromAssetsFolder(Assembly, "shipassets");

        // Initialize custom prefabs
        InitializePrefabs();

        // Translations
        LanguageHandler.RegisterLocalizationFolder();

        // register harmony patches, if there are any
        Harmony.CreateAndPatchAll(Assembly, $"{PluginInfo.PLUGIN_GUID}");
        Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
    }

    private void InitializePrefabs()
    {
        var seaVoyager = new SeaVoyagerPrefab().Register();

        var seaVoyagerDockFragment = new SeaVoyagerFragment("SeaVoyagerFragment1", "SeaVoyagerFragment1", 90).Register();
        var seaVoyagerPoleFragment = new SeaVoyagerFragment("SeaVoyagerFragment2", "SeaVoyagerFragment2", 150).Register();
        var seaVoyagerHullFragment = new SeaVoyagerFragment("SeaVoyagerFragment3", "SeaVoyagerFragment3", 500).Register();
        var seaVoyagerLadderFragment = new SeaVoyagerFragment("SeaVoyagerFragment4", "SeaVoyagerFragment4", 70).Register();
        var seaVoyagerPropellerFragment = new SeaVoyagerFragment("SeaVoyagerFragment5", "SeaVoyagerFragment5", 200).Register();

        CoordinatedSpawnsHandler.RegisterCoordinatedSpawns(new List<SpawnInfo>()
            {
                // Wreck 1
                new SpawnInfo(seaVoyagerHullFragment.ClassID, new Vector3(93.21509f, -43.46548f, -6.106212f), new Vector3(3, 359, 338)),
                new SpawnInfo(seaVoyagerLadderFragment.ClassID, new Vector3(104.19f, -45.66f, -0.48f), new Vector3(1, 0, 333)),
                new SpawnInfo(seaVoyagerPropellerFragment.ClassID, new Vector3(92.58f, -39.59f, 7.39f), new Vector3(28, 0, 137)),
                new SpawnInfo(seaVoyagerDockFragment.ClassID, new Vector3(96.32f, -51.44f, -17.64f), new Vector3(0, 0, 189)),

                // Wreck 2
                new SpawnInfo(seaVoyagerHullFragment.ClassID, new Vector3(-71.41f, -35.79f, 416f), new Vector3(0, 34, 0)),
                new SpawnInfo(seaVoyagerDockFragment.ClassID, new Vector3(-72.00f, -35.20f, 402.00f), new Vector3(17, 69, 103)),
                new SpawnInfo(seaVoyagerDockFragment.ClassID, new Vector3(-62.54f, -27.83f, 429.26f), new Vector3(0, 358, 15)),
                new SpawnInfo(seaVoyagerPropellerFragment.ClassID, new Vector3(-68.14f, -37.00f, 404.32f), new Vector3(30, 232, 180)),
                new SpawnInfo(seaVoyagerPoleFragment.ClassID, new Vector3(-50.17f, -23.18f, 423.00f), new Vector3(279, 193, 180)),
                new SpawnInfo(seaVoyagerLadderFragment.ClassID, new Vector3(-56.70f, -26.63f, 426.50f), new Vector3(347, 359, 12)),
                new SpawnInfo(seaVoyagerLadderFragment.ClassID, new Vector3(-45.51f, -24.08f, 411.84f), new Vector3(2, 0, 358)),
        });

        PDAHandler.AddCustomScannerEntry(SeaVoyagerFragment.SeaVoyagerFragmentTechType, seaVoyager.Info.TechType, true, 10, 4);
    }
}