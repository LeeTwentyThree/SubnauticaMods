using System.Collections;
using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using Nautilus.Assets;
using Nautilus.Assets.Gadgets;
using Nautilus.Assets.PrefabTemplates;
using Nautilus.Handlers;
using Nautilus.Utility;
using TheRedPlague.Mono;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace TheRedPlague;

[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
[BepInDependency("com.snmodding.nautilus")]
public class Plugin : BaseUnityPlugin
{
    public new static ManualLogSource Logger { get; private set; }

    private static Assembly Assembly { get; } = Assembly.GetExecutingAssembly();
    
    public static AssetBundle AssetBundle { get; set; }
    public static Texture2D ZombieInfectionTexture { get; set; }
    
    private void Awake()
    {
        AssetBundle = AssetBundleLoadingUtils.LoadFromAssetsFolder(Assembly, "theredplague");

        ZombieInfectionTexture = AssetBundle.LoadAsset<Texture2D>("zombie_infection_bloody");
            
        // set project-scoped logger instance
        Logger = base.Logger;
        
        // register harmony patches, if there are any
        Harmony.CreateAndPatchAll(Assembly, $"{PluginInfo.PLUGIN_GUID}");
        Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");

        ModPrefabs.RegisterPrefabs();
        
        IslandProps.AddIslandPropSpawns();
        
        ConsoleCommandsHandler.AddGotoTeleportPosition("forcefieldisland", new Vector3(-78.1f, 315.5f, -68.7f));
    }
}