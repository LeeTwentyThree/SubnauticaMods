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
    public static string LanguageModelDirPath = "Assets/model/english_small";

    public static Assembly assembly = Assembly.GetExecutingAssembly();

    public static AssetBundle assetBundle;

    public static Config config = OptionsPanelHandler.RegisterModOptions<Config>();

    public static bool TestingModeActive
    {
        get
        {
            return config.EnableDebugMode;
        }
    }

    private void Awake()
    {
        assetBundle = AssetBundle.LoadFromFile(Path.Combine(Path.GetDirectoryName(assembly.Location), "Assets", "monkeysaymonkeyget"));

        PhraseManager.Initialize();

        var harmony = new Harmony("Lee23.MonkeySayMonkeyGet");
        harmony.PatchAll(assembly);
    }
}