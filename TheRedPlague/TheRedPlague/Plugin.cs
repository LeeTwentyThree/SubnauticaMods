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

        ModPrefabs.RegisterPrefabs();
        
        CoordinatedSpawns.RegisterCoordinatedSpawns();
        
        ConsoleCommandsHandler.AddGotoTeleportPosition("forcefieldisland", new Vector3(-78.1f, 315.5f, -68.7f));
        ConsoleCommandsHandler.AddGotoTeleportPosition("plagueheartisland", new Vector3(-1327, -193, 283));
        
        ModAudio.RegisterAudio();

        // Add CragField_Deep to the BiomeType enum (the name doesn't really matter)
        BiomeType deepCragFieldCreaturesBiomeType = EnumHandler.AddEntry<BiomeType>("CragField_Deep_Creatures");

        // Create a prefab for the biome slot
        var deepCragFieldCreaturesSlotInfo = PrefabInfo.WithTechType("DeepCragFieldCreaturesSlot");
        var deepCragFieldCreaturesSlotPrefab = new CustomPrefab(deepCragFieldCreaturesSlotInfo);
        deepCragFieldCreaturesSlotPrefab.SetGameObject(() =>
        {
            // This name really doesn't matter either
            var prefab = new GameObject(deepCragFieldCreaturesSlotInfo.ClassID);
            // Add the most necessary component - a prefab identifier
            prefab.AddComponent<PrefabIdentifier>().ClassId = deepCragFieldCreaturesSlotInfo.ClassID;
            // Cells seem to use "Near", so they don't load in until you get close
            prefab.AddComponent<LargeWorldEntity>().cellLevel = LargeWorldEntity.CellLevel.Near;
            // Add the entity slot component which actually links the BiomeType to this prefab
            // I am NOT certain these fields will actually get set - a lot of these fields are "proto members" which often need special treatment for custom prefabs
            var entitySlot = prefab.AddComponent<EntitySlot>();
            entitySlot.biomeType = deepCragFieldCreaturesBiomeType;
            entitySlot.allowedTypes = new List<EntitySlot.Type> {EntitySlot.Type.Creature};
            return prefab;
        });
        deepCragFieldCreaturesSlotPrefab.Register();
        
        // Add spawns to that biome type. When spawning the slot, one of these will appear
        LootDistributionHandler.AddLootDistributionData(CraftData.GetClassIdForTechType(TechType.Peeper),
            new LootDistributionData.BiomeData { biome = deepCragFieldCreaturesBiomeType, count = 1, probability = 0.5f } );
        
        LootDistributionHandler.AddLootDistributionData(CraftData.GetClassIdForTechType(TechType.Jellyray),
            new LootDistributionData.BiomeData { biome = deepCragFieldCreaturesBiomeType, count = 1, probability = 0.2f } );
        
        StoryUtils.RegisterStory();
        StoryUtils.RegisterLanguageLines();
    }
}