using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using Nautilus.Assets;
using Nautilus.Crafting;
using Nautilus.Handlers;
using Nautilus.Utility;
using PdaUpgradeCards.Data;
using PdaUpgradeCards.MonoBehaviours;
using PdaUpgradeCards.MonoBehaviours.Upgrades;
using PdaUpgradeCards.Prefabs;
using UnityEngine;

namespace PdaUpgradeCards;

[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
[BepInDependency("com.snmodding.nautilus", "1.0.0.37")]
[BepInDependency("com.lee23.theredplague", BepInDependency.DependencyFlags.SoftDependency)]
[BepInDependency("com.aci.hydra", BepInDependency.DependencyFlags.SoftDependency)]
[BepInDependency("com.aotu.returnoftheancients", BepInDependency.DependencyFlags.SoftDependency)]
public class Plugin : BaseUnityPlugin
{
    public new static ManualLogSource Logger { get; private set; }

    private static Assembly Assembly { get; } = Assembly.GetExecutingAssembly();

    internal static AssetBundle Bundle { get; } =
        AssetBundleLoadingUtils.LoadFromAssetsFolder(Assembly, "pdaupgradechips");

    internal static TechType PocketDimensionTier1TechType { get; private set; }
    internal static TechType PocketDimensionTier2TechType { get; private set; }
    internal static TechType PocketDimensionTier3TechType { get; private set; }

    private void Awake()
    {
        // set project-scoped logger instance
        Logger = base.Logger;

        LanguageHandler.RegisterLocalizationFolder();

        // Initialize custom prefabs
        InitializePrefabs();

        // register harmony patches, if there are any
        Harmony.CreateAndPatchAll(Assembly, $"{PluginInfo.PLUGIN_GUID}");
        Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");

        RegisterAudio();
        PdaUpgradesAPI.Register();
        PdaUpgradesManager.RegisterSaveData();

        PdaElements.RegisterAll();

        StartCoroutine(PdaMusicDatabase.RefreshMusicDatabase());
    }

    private static void InitializePrefabs()
    {
        PdaUpgradesContainerPrefab.Register();

        new UpgradeCardPrefab<MusicUpgrade>(PrefabInfo.WithTechType("PdaMusicUpgrade", true)
                .WithIcon(Bundle.LoadAsset<Sprite>("UpgradeIcon_MusicPlayer")),
            new RecipeData(new CraftData.Ingredient(TechType.ComputerChip),
                new CraftData.Ingredient(TechType.FiberMesh), new CraftData.Ingredient(TechType.Magnetite))).Register();
        new UpgradeCardPrefab<ColorizerUpgrade>(PrefabInfo.WithTechType("PdaColorizerUpgrade", true)
                    .WithIcon(Bundle.LoadAsset<Sprite>("UpgradeIcon_PDGay")),
                new RecipeData(new CraftData.Ingredient(TechType.AluminumOxide),
                    new CraftData.Ingredient(TechType.UraniniteCrystal), new CraftData.Ingredient(TechType.Diamond)))
            .Register();
        new UpgradeCardPrefab<LeviathanDetectorUpgrade>(PrefabInfo.WithTechType("PdaLeviathanDetectorUpgrade", true)
                    .WithIcon(Bundle.LoadAsset<Sprite>("UpgradeIcon_LeviathanDetector")),
                new RecipeData(new CraftData.Ingredient(TechType.ComputerChip),
                    new CraftData.Ingredient(TechType.AdvancedWiringKit), new CraftData.Ingredient(TechType.Quartz)))
            .Register();
        new UpgradeCardPrefab<PocketDimensionUpgradeTier1>(PrefabInfo.WithTechType("PdaPocketDimensionUpgradeMk1", true)
                    .WithIcon(Bundle.LoadAsset<Sprite>("UpgradeIcon_PocketDimension")),
                new RecipeData(new CraftData.Ingredient(TechType.TitaniumIngot),
                    new CraftData.Ingredient(TechType.PrecursorIonCrystal),
                    new CraftData.Ingredient(TechType.AdvancedWiringKit),
                    new CraftData.Ingredient(TechType.Pipe, 2)))
            .Register();

        var pocketTier1 = new PocketDimensionPrefab(PrefabInfo.WithTechType("PdaPocketDimensionTier1"),
            "c794ac3f-d506-4338-9a8d-4b418a2e6741", new Vector3(-0.6f, 1.4f, 0.2f))
        {
            ModifyPrefab = PocketDimensionPrefab.ModifyPocketDimensionTier1
        };
        pocketTier1.Register();
        PocketDimensionTier1TechType = pocketTier1.Info.TechType;

        var pocketTier3 = new PocketDimensionPrefab(PrefabInfo.WithTechType("PdaPocketDimensionTier3"),
            "c80288ce-9522-45f5-b3c2-01fe459ae5fe", Vector3.zero)
        {
            ModifyPrefab = PocketDimensionPrefab.ModifyPocketDimensionTier3
        };
        pocketTier3.Register();
        PocketDimensionTier3TechType = pocketTier3.Info.TechType;
    }

    private static void RegisterAudio()
    {
        CustomSoundHandler.RegisterCustomSound("PdaDetectingLeviathan1",
            Bundle.LoadAsset<AudioClip>("DetectingLeviathan1"),
            AudioUtils.BusPaths.PDAVoice, AudioUtils.StandardSoundModes_2D);
        CustomSoundHandler.RegisterCustomSound("PdaDetectingLeviathan2",
            Bundle.LoadAsset<AudioClip>("DetectingLeviathan2"),
            AudioUtils.BusPaths.PDAVoice, AudioUtils.StandardSoundModes_2D);
        CustomSoundHandler.RegisterCustomSound("PdaDetectingLeviathan3",
            Bundle.LoadAsset<AudioClip>("DetectingLeviathan3"),
            AudioUtils.BusPaths.PDAVoice, AudioUtils.StandardSoundModes_2D);
    }
}