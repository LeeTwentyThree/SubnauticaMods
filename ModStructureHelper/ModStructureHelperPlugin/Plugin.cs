using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using Nautilus.Handlers;
using Nautilus.Utility;
using UnityEngine;
using UnityFx.Outline;

/*
 * TO-DO
 * PATCH IN-GAME MENU TO ADD BUTTON TO OPEN THE MOD STRUCTURE HELPER
 * MAKE SURE THE CONVERSION TO "ENTITY GROUPS" TO "STRUCTURES" WAS DONE CORRECTLY
 * IMPLEMENT ENTITY VIEWER, SORTING BY MODS, ETC. (take from void editor?)
 */
namespace ModStructureHelperPlugin;

[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
[BepInDependency("com.snmodding.nautilus")]
public class Plugin : BaseUnityPlugin
{
    public new static ManualLogSource Logger { get; private set; }

    public static Assembly Assembly { get; } = Assembly.GetExecutingAssembly();

    public static AssetBundle AssetBundle { get; } =
        AssetBundleLoadingUtils.LoadFromAssetsFolder(Assembly, "modstructurehelper");

    public static ModConfig ModConfig { get; } = OptionsPanelHandler.RegisterModOptions<ModConfig>();
    
    public static OutlineResources OutlineResources { get; private set; }

    private void Awake()
    {
        // set project-scoped logger instance
        Logger = base.Logger;
        
        // register harmony patches, if there are any
        Harmony.CreateAndPatchAll(Assembly, $"{PluginInfo.PLUGIN_GUID}");
        
        OutlineResources = ScriptableObject.CreateInstance<OutlineResources>();
        OutlineResources.ResetToDefaults();
        
        Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
    }
}