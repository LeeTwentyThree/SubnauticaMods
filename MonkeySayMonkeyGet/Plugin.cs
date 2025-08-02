using System;
using System.Collections;
using System.Collections.Generic;
using BepInEx;
using System.IO;
using HarmonyLib;
using Nautilus.Handlers;
using System.Reflection;
using UnityEngine;

namespace MonkeySayMonkeyGet;

[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
[BepInDependency("com.snmodding.nautilus")]
public class Plugin : BaseUnityPlugin
{
    public static readonly string LanguageModelDirPath = "Assets/model/english_small";

    public static Assembly Assembly { get; } = Assembly.GetExecutingAssembly();

    public static AssetBundle AssetBundle { get; private set; }

    public static Config ModConfig { get; } = OptionsPanelHandler.RegisterModOptions<Config>();

    private static readonly string[] RequiredPluginsInDataFolder =
        { "flang.dll", "flangrti.dll", "libomp.dll", "SRS.dll" };

    public static bool TestingModeActive => ModConfig.EnableDebugMode;

    private void Awake()
    {
        AssetBundle =
            AssetBundle.LoadFromFile(Path.Combine(Path.GetDirectoryName(Assembly.Location), "Assets",
                "monkeysaymonkeyget"));

        PhraseManager.Initialize();

        var harmony = new Harmony("Lee23.MonkeySayMonkeyGet");
        harmony.PatchAll(Assembly);
    }

    private IEnumerator Start()
    {
        yield return new WaitUntil(() => ErrorMessage.main != null);
        WarnForMissingPlugIn();
    }

    private static void WarnForMissingPlugIn()
    {
        var dataPluginFolder = Path.Combine(Paths.GameRootPath, "Subnautica_Data", "Plugins");
        string prefix = "[<color=#8034eb>MonkeySayMonkeyGet</color>] ";
        int missing = 0;
        foreach (var fileName in RequiredPluginsInDataFolder)
        {
            if (!File.Exists(Path.Combine(dataPluginFolder, fileName)))
            {
                ErrorMessage.AddMessage(prefix + $"Missing file '{fileName}' in the SubnauticaData\\Plugins folder.");
                missing++;
            }
        }

        if (missing > 0)
        {
            ErrorMessage.AddMessage(
                "Please ensure you have correctly installed the mod and moved all files to the correct location.");
        }
    }
}