using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using KallieʼsPropPack.PrefabLoading;
using KallieʼsPropPack.Prefabs.Grasses;
using KallieʼsPropPack.Prefabs.Lab;
using KallieʼsPropPack.Prefabs.Plants;
using KallieʼsPropPack.Prefabs.SingleCellLandscape;
using KallieʼsPropPack.Prefabs.Trees;
using Nautilus.Assets;
using Nautilus.Assets.PrefabTemplates;
using Nautilus.Handlers;
using Nautilus.Utility;
using Nautilus.Utility.MaterialModifiers;
using Newtonsoft.Json;
using UnityEngine;

namespace KallieʼsPropPack;

[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
[BepInDependency("com.snmodding.nautilus")]
public class Plugin : BaseUnityPlugin
{
    public new static ManualLogSource Logger { get; private set; }

    private static Assembly Assembly { get; } = Assembly.GetExecutingAssembly();

    private bool _initializedPrefabs;
    
    internal static AssetBundle Bundle { get; private set; }

    private void Awake()
    {
        Logger = base.Logger;

        WaitScreenHandler.RegisterEarlyLoadTask(PluginInfo.PLUGIN_NAME, InitializePrefabs);

        Harmony.CreateAndPatchAll(Assembly, $"{PluginInfo.PLUGIN_GUID}");
        Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
    }

    private void InitializePrefabs(WaitScreenHandler.WaitScreenTask task)
    {
        if (_initializedPrefabs)
        {
            return;
        }

        _initializedPrefabs = true;

        Bundle = AssetBundleLoadingUtils.LoadFromAssetsFolder(Assembly, "kalliesproppack");
        
        PropPackAudio.RegisterAudio();

        // Register purple pine tree
        PurplePineTree.Register();

        // Register kelp roots
        (string classId, string originalClassId)[] kelpRootPrefabs =
        {
            ("Kallies_KelpRoot_Small_01", "4bfe1877-1b83-4d5d-9470-3bb2d5f389cc"),
            ("Kallies_KelpRoot_Large_03_Light", "5beba896-bccf-4993-8bcb-1cdabb68e706"),
            ("Kallies_KelpRoot_Small_02", "a0c5b949-22a4-4899-9c51-64ccce6956bc"),
            ("Kallies_KelpRoot_Small_03", "a0cbac2e-f86d-4ab0-a090-8115f5196f7c"),
            ("Kallies_KelpRoot_Large_01", "abe572e9-126b-43eb-bf5c-4edf9ec365c1"),
            ("Kallies_KelpRoot_Large_02_Light", "b0cae640-b155-4bac-9ed5-29ba64a1ee9f"),
            ("Kallies_KelpRoot_Small_05", "cd004d89-f798-40d0-bf65-ee4c1c48700c"),
            ("Kallies_KelpRoot_Small_04", "d8efe522-5355-48b8-b4fb-4d077bbc621d"),
            ("Kallies_KelpRoot_Large_01_Light", "da7341c3-e6a3-4cd3-ad57-49a4dc732ac9"),
            ("Kallies_KelpRoot_Large_04_Light", "db79ee0b-65e9-4ea1-8b8b-948bbae128f7"),
            ("Kallies_KelpRoot_Large_03", "e3fd373d-6ecc-497a-b396-816f3cb5f9b6"),
            ("Kallies_KelpRoot_Large_04", "e40daa31-8eb8-463a-b91a-d3aedb631744"),
            ("Kallies_KelpRoot_Large_02", "f0a54d9a-7717-473f-8450-5ff24824ed7e")
        };

        foreach (var kelpRootData in kelpRootPrefabs)
        {
            var kelpRootPrefab = new KelpRoot(kelpRootData.classId, kelpRootData.originalClassId);
            kelpRootPrefab.Register();
        }

        // Register grasses

        const string coralGrassPrefix = "Kallie_Grass_CoralGrass_";
        const string coralGrassClone = "aa1abbb9-716c-44b8-a2b8-cb4d9d0f22bb";
        const string kelpGrass1Prefix = "Kallie_Grass_Kelp_01_";
        const string kelpGrass1Clone = "880b59b7-8fd6-412f-bbcb-a4260b263124";
        const string kelpGrass2Prefix = "Kallie_Grass_Kelp_02_";
        const string kelpGrass2Clone = "bac42c90-8995-439f-be2f-29a6d164c82a";
        const string anemoneGrassPrefix = "Kallie_Grass_Anemone_";
        const string anemoneGrassClone = "72e64ce2-eb55-4e09-955e-684c809f9038";
        const string bush1Prefix = "Kallie_Grass_Bush1_";
        const string bush1Clone = "b41f71d6-d9d5-411d-8726-e8433b181480";
        const string bush2Prefix = "Kallie_Grass_Bush2_";
        const string bush2Clone = "eb5ea858-930d-4272-91b5-e9ebe2286ca8";

        var coloredGrasses = new[]
        {
            new ColoredGrass(coralGrassPrefix + "BloodOrange", coralGrassClone)
                .WithColor(new Color(0.816f, 0, 0), new Color(0.86f, 1, 0.476f)),
            new ColoredGrass(coralGrassPrefix + "Red", coralGrassClone)
                .WithColor(new Color(0.816f, 0, 0.33f), new Color(0.857f, 0.1f, 0.38f)),
            new ColoredGrass(coralGrassPrefix + "Green", coralGrassClone)
                .WithColor(new Color(0.2f, 0.52f, 0.62f), new Color(0.2f, 0.67f, 0.38f)),
            new ColoredGrass(coralGrassPrefix + "Lime", coralGrassClone)
                .WithColor(new Color(0.34f, 0.57f, 0.57f), new Color(0.63f, 1, 0.38f)),
            new ColoredGrass(coralGrassPrefix + "Orange", coralGrassClone)
                .WithColor(new Color(1.5f, 0.57f, 0.57f), new Color(0.96f, 0.19f, 0.38f)),
            new ColoredGrass(coralGrassPrefix + "Purple", coralGrassClone)
                .WithColor(new Color(0.71f, 0, 1.8f), new Color(0.95f, 0, 1)),
            new ColoredGrass(kelpGrass1Prefix + "Faded", kelpGrass1Clone)
                .WithColor(new Color(1.3f, 0.71f, 1), new Color(1.2f, 0.71f, 1)),
            new ColoredGrass(kelpGrass1Prefix + "Amber", kelpGrass1Clone)
                .WithColor(new Color(1.8f, 0.71f, 1), new Color(2, 0.7f, 1)),
            new ColoredGrass(kelpGrass1Prefix + "Red", kelpGrass1Clone)
                .WithColor(new Color(2.0f, 0.2f, 1.0f), new Color(1.5f, 0.2f, 1)),
            new ColoredGrass(kelpGrass1Prefix + "Withered", kelpGrass1Clone)
                .WithColor(new Color(0.95f, 0.285f, 1), new Color(1, 1.3f, 1)),
            new ColoredGrass(kelpGrass1Prefix + "Yellow", kelpGrass1Clone)
                .WithColor(new Color(3, 1, 1), new Color(3, 1, 1)),
            new ColoredGrass(kelpGrass2Prefix + "Faded", kelpGrass2Clone)
                .WithColor(new Color(1.3f, 0.71f, 1), new Color(1.2f, 0.71f, 1)),
            new ColoredGrass(kelpGrass2Prefix + "Amber", kelpGrass2Clone)
                .WithColor(new Color(1.8f, 0.71f, 1), new Color(2, 0.7f, 1)),
            new ColoredGrass(kelpGrass2Prefix + "Red", kelpGrass2Clone)
                .WithColor(new Color(2, 0.2f, 1.0f), new Color(1.5f, 0.2f, 1)),
            new ColoredGrass(kelpGrass2Prefix + "Withered", kelpGrass2Clone)
                .WithColor(new Color(0.95f, 0.285f, 1), new Color(1, 1.3f, 1)),
            new ColoredGrass(kelpGrass2Prefix + "Yellow", kelpGrass2Clone)
                .WithColor(new Color(3, 1, 1), new Color(3, 1, 1)),
            new ColoredGrass(anemoneGrassPrefix + "Red", anemoneGrassClone)
                .WithColor(new Color(0.45f, 0.05f, 0.25f)),
            new ColoredGrass(anemoneGrassPrefix + "DarkBlue", anemoneGrassClone)
                .WithColor(new Color(0, 0.1f, 1)),
            new ColoredGrass(anemoneGrassPrefix + "Purple", anemoneGrassClone)
                .WithColor(new Color(0.2f, 0.05f, 1)),
            new ColoredGrass(anemoneGrassPrefix + "Pink", anemoneGrassClone)
                .WithColor(new Color(0.5f, 0.05f, 0.5f)),
            new ColoredGrass(anemoneGrassPrefix + "Salmon", anemoneGrassClone)
                .WithColor(new Color(0.5f, 0.17f, 0.5f)),
            new ColoredGrass(anemoneGrassPrefix + "Orange", anemoneGrassClone)
                .WithColor(new Color(0.5f, 0.31f, 0.5f)),
            new ColoredGrass(anemoneGrassPrefix + "Realistic", anemoneGrassClone)
                .WithColor(new Color(0.23f, 0.28f, 0.05f)),
            new ColoredGrass(anemoneGrassPrefix + "Turquoise", anemoneGrassClone)
                .WithColor(new Color(0, 0.5f, 1)),
            new ColoredGrass(anemoneGrassPrefix + "LightBlue", anemoneGrassClone)
                .WithColor(new Color(0, 0.4f, 1.5f)),
            new ColoredGrass(anemoneGrassPrefix + "Green", anemoneGrassClone)
                .WithColor(new Color(0.12f, 0.4f, 0.5f)),
            new ColoredGrass(anemoneGrassPrefix + "White", anemoneGrassClone)
                .WithColor(new Color(0.38f, 0.4f, 1.25f)),
            new ColoredGrass(anemoneGrassPrefix + "Gray", anemoneGrassClone)
                .WithColor(new Color(0.19f, 0.19f, 0.5f)),
            new ColoredGrass(bush1Prefix + "Pink", bush1Clone)
                .WithColor(new Color(1, 1, 1)),
            new ColoredGrass(bush1Prefix + "Lime", bush1Clone)
                .WithColor(new Color(1, 2, 1)),
            new ColoredGrass(bush1Prefix + "Purple", bush1Clone)
                .WithColor(new Color(1, 1, 2)),
            new ColoredGrass(bush1Prefix + "RoyalPurple", bush1Clone)
                .WithColor(new Color(0.42f, 0.29f, 0.71f)),
            new ColoredGrass(bush1Prefix + "Red", bush1Clone)
                .WithColor(new Color(1, 0.2f, 0.2f)),
            new ColoredGrass(bush1Prefix + "Orange", bush1Clone)
                .WithColor(new Color(1, 0.277f, 0.2f)),
            new ColoredGrass(bush1Prefix + "Blue", bush1Clone)
                .WithColor(new Color(0.14f, 0.57f, 1.4f)),
            new ColoredGrass(bush1Prefix + "Green", bush1Clone)
                .WithColor(new Color(0.48f, 1.5f, 0.86f)),
            new ColoredGrass(bush1Prefix + "Seafoam", bush1Clone)
                .WithColor(new Color(0.48f, 1, 0.86f)),
            new ColoredGrass(bush2Prefix + "Red", bush2Clone)
                .WithColor(new Color(1, 1, 1)),
            new ColoredGrass(bush2Prefix + "DarkGreen", bush2Clone)
                .WithColor(new Color(0.29f, 1, 0.5f)),
            new ColoredGrass(bush2Prefix + "Green", bush2Clone)
                .WithColor(new Color(0.33f, 1, 0.24f)),
            new ColoredGrass(bush2Prefix + "LushGreen", bush2Clone)
                .WithColor(new Color(0.3f, 2, 0.3f)),
            new ColoredGrass(bush2Prefix + "Yellow", bush2Clone)
                .WithColor(new Color(1, 2, 0.3f)),
            new ColoredGrass(bush2Prefix + "Orange", bush2Clone)
                .WithColor(new Color(0.95f, 1, 0.24f)),
            new ColoredGrass(bush2Prefix + "DarkRed", bush2Clone)
                .WithColor(new Color(0.8f, 0.05f, 0.24f)),
            new ColoredGrass(bush2Prefix + "Purple", bush2Clone)
                .WithColor(new Color(0.76f, 0.04f, 1)),
            new ColoredGrass(bush2Prefix + "Gray", bush2Clone)
                .WithColor(new Color(0.48f, 0.86f, 1)),
            new ColoredGrass(bush2Prefix + "Amber", bush2Clone)
                .WithColor(new Color(0.62f, 1, 0)),
            new ColoredGrass(bush2Prefix + "Blue", bush2Clone)
                .WithColor(new Color(0.19f, 0.8f, 3)),
            new ColoredGrass(bush2Prefix + "GrandReefBlue", bush2Clone)
                .WithColor(new Color(0.19f, 0.8f, 1.4f))
        };

        foreach (var grass in coloredGrasses)
        {
            grass.Register();
        }

        // Register single cell landscape entities

        new SingleCellGround("Kallies_SingleCellGround", LargeWorldEntity.CellLevel.Medium, false).Register();
        new SingleCellGround("Kallies_SingleCellGround_LoadFar", LargeWorldEntity.CellLevel.Far, false).Register();
        new SingleCellGround("Kallies_SingleCellGround_Rock", LargeWorldEntity.CellLevel.Medium, true).Register();
        new SingleCellGround("Kallies_SingleCellGround_Rock_LoadFar", LargeWorldEntity.CellLevel.Far, true).Register();

        new SingleCellRib("Kallies_SingleCellRib_01", "4404f4f2-3d65-4338-adb3-a1a2e1f8fac5").Register();
        new SingleCellRib("Kallies_SingleCellRib_02", "6e37459e-d880-4da8-8dad-0cc10ff07f00").Register();
        new SingleCellRib("Kallies_SingleCellRib_03", "ee1807bf-6744-4fee-a66f-c71edc9e7fb6").Register();
        new SingleCellRib("Kallies_SingleCellRib_04", "33c31a89-9d3b-4717-ad26-4cc8106a1f24").Register();
        
        new SingleCellBlob("Kallies_SingleCell_Blob_Round", "SCL_Blob_Round").Register();
        new SingleCellBlob("Kallies_SingleCell_Blob_Flat", "SLC_Blob_Flat").Register();
        new SingleCellBlob("Kallies_SingleCell_Blob_Oblong", "SLC_Blob_Oblong").Register();
        new SingleCellBlob("Kallies_SingleCell_Blob_Tetrahedron", "SLC_Blob_Tetrahedron").Register();
        
        new SingleCellTentacle("Kallies_SingleCell_Tentacle_1", "SCL_Tentacle_Prefab_1").Register();
        new SingleCellTentacle("Kallies_SingleCell_Tentacle_2", "SCL_Tentacle_Prefab_2").Register();
        
        var singleCellBiomeSettings = BiomeUtils.CreateBiomeSettings(new Vector3(2.4f, 2.1f, 1.2f), 1f,
            new Color(0.5f, 1, 1, 1), 1.5f, new Color(0.9f, 0.9f, 0.9f),
            0.015f, 30, 2, 2f, 16f);
        BiomeHandler.RegisterBiome("singlecell", singleCellBiomeSettings,
            new BiomeHandler.SkyReference("SkyMountains"));
        var sclBiomePrefab = new CustomPrefab(PrefabInfo.WithTechType("SCLVolume"));
        var sclBiomeTemplate = new AtmosphereVolumeTemplate(sclBiomePrefab.Info,
            AtmosphereVolumeTemplate.VolumeShape.Cube, "singlecell");
        BiomeHandler.AddBiomeAmbience("singlecell", AudioUtils.GetFmodAsset("SCL_Ambience"), FMODGameParams.InteriorState.Always);
        sclBiomePrefab.SetGameObject(sclBiomeTemplate);
        sclBiomePrefab.Register();
        
        // Register lab entities

        var prefabLoader = new EpicPrefabLoader(new[] { Assembly }, Bundle);
        prefabLoader.LoadPrefabs(JsonConvert.DeserializeObject<LoadedPrefabRegistrationData>(
            Bundle.LoadAsset<TextAsset>("LabPrefabs").text));
        
        // Register coral entities
        prefabLoader.LoadPrefabs(JsonConvert.DeserializeObject<LoadedPrefabRegistrationData>(
            Bundle.LoadAsset<TextAsset>("CoralPrefabs").text));
    }
}