using System;
using System.Collections;
using System.IO;
using System.Reflection;
using AuroraDecalFixMod.Data;
using AuroraDecalFixMod.DevTools;
using AuroraDecalFixMod.PrefabModification;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using Nautilus.Handlers;
using Newtonsoft.Json;

namespace AuroraDecalFixMod;

[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
[BepInDependency("com.snmodding.nautilus")]
public class Plugin : BaseUnityPlugin
{
    public new static ManualLogSource Logger { get; private set; }

    internal static Assembly Assembly { get; } = Assembly.GetExecutingAssembly();

    private void Awake()
    {
        Logger = base.Logger;
        
        RegisterModFeatures();
        
        Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
    }

    private void RegisterModFeatures()
    {
        ConsoleCommandsHandler.RegisterConsoleCommand("generate_decal_data", typeof(ModCommands), "GenerateEssentialDataCommand", new Type[0]);
        WaitScreenHandler.RegisterEarlyAsyncLoadTask(PluginInfo.PLUGIN_NAME, MainWaitScreenTask);
    }

    private static IEnumerator MainWaitScreenTask(WaitScreenHandler.WaitScreenTask task)
    {
        var filePath = Path.Combine(Path.GetDirectoryName(Assembly.Location), "ShadowsDisableData.json");
        if (!File.Exists(filePath))
        {
            Logger.LogError($"ShadowsDisableData.json file could not be found at path {filePath}");
            yield break;
        }
        
        var modifier = new AuroraPrefabModifier(JsonConvert.DeserializeObject<AllDecalData>(File.ReadAllText(filePath)));
        yield return modifier.ModifyAll(task);
    }
}