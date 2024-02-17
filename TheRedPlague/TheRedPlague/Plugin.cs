using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using Nautilus.Assets;
using Nautilus.Handlers;
using Nautilus.Utility;
using UnityEngine;

namespace TheRedPlague;

[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
[BepInDependency("com.snmodding.nautilus")]
[BepInDependency("com.aci.thesilence", BepInDependency.DependencyFlags.SoftDependency)]
public class Plugin : BaseUnityPlugin
{
    public new static ManualLogSource Logger { get; private set; }

    private static Assembly Assembly { get; } = Assembly.GetExecutingAssembly();
    
    public static AssetBundle AssetBundle { get; set; }
    public static Texture2D ZombieInfectionTexture { get; set; }
    public static ModConfig ModConfig { get; private set; }
    
    private void Awake()
    {
        AssetBundle = AssetBundleLoadingUtils.LoadFromAssetsFolder(Assembly, "theredplague");

        ZombieInfectionTexture = AssetBundle.LoadAsset<Texture2D>("zombie_infection_bloody");
            
        // set project-scoped logger instance
        Logger = base.Logger;

        ModConfig = OptionsPanelHandler.RegisterModOptions<ModConfig>();
        
        // register harmony patches, if there are any
        Harmony.CreateAndPatchAll(Assembly, $"{PluginInfo.PLUGIN_GUID}");
        Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");

        CraftTreeHandler.AddTabNode(CraftTree.Type.Workbench, "PlagueEquipment", "Plague equipment", AssetBundle.LoadAsset<Sprite>("WarperHeartIcon"));

        ModPrefabs.RegisterPrefabs();
        
        CoordinatedSpawns.RegisterCoordinatedSpawns();
        
        ConsoleCommandsHandler.AddGotoTeleportPosition("forcefieldisland", new Vector3(-78.1f, 315.5f, -68.7f));
        ConsoleCommandsHandler.AddGotoTeleportPosition("plagueheartisland", new Vector3(-1327, -193, 283));
        ConsoleCommandsHandler.RegisterConsoleCommands(typeof(Commands));
        
        ModAudio.RegisterAudio();
        
        StoryUtils.RegisterStory();
        StoryUtils.RegisterLanguageLines();
        
        ModCompatibility.PatchCompatibility();
    }
}