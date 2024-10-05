using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using SNCreationKitData.OptionGeneration;
using SNCreationKitPlugin.OptionsImplementation;
using UnityEngine;

namespace SNCreationKitPlugin;

[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
[BepInDependency("com.snmodding.nautilus")]
public class Plugin : BaseUnityPlugin
{
    public new static ManualLogSource Logger { get; private set; }

    private static Assembly Assembly { get; } = Assembly.GetExecutingAssembly();

    private void Awake()
    {
        // set project-scoped logger instance
        Logger = base.Logger;
        
        // register harmony patches, if there are any
        Harmony.CreateAndPatchAll(Assembly, $"{PluginInfo.PLUGIN_GUID}");
        Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");

        var optionsGenerationContainer = new OptionGenerationContainer<RectTransform>(
            new UserInterfaceManager(),
            new 
            );
    }
}