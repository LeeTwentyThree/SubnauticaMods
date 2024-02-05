using System;
using System.Collections;
using Nautilus.Assets;
using Nautilus.Assets.Gadgets;
using Nautilus.Assets.PrefabTemplates;
using Nautilus.Crafting;
using Nautilus.Handlers;
using Nautilus.Utility;
using TheRedPlague.Creatures;
using TheRedPlague.Mono;
using TheRedPlague.PrefabFiles;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TheRedPlague;

public static class ModPrefabs
{
    private static PrefabInfo InfectionDomeInfo { get; } = PrefabInfo.WithTechType("InfectionDome");
    private static PrefabInfo InfectionLaserInfo { get; } = PrefabInfo.WithTechType("InfectionLaser");
    private static PrefabInfo LaserReceptacleInfo { get; } = PrefabInfo.WithTechType("InfectionLaserReceptacle");
    public static PrefabInfo DeadSeaEmperorInfo { get; } = PrefabInfo.WithTechType("DeadSeaEmperor", "Deceased Sea Emperor", "A deceased Sea Emperor.");
    public static PrefabInfo DeadSeaEmperorSpawnerInfo { get; } = PrefabInfo.WithTechType("DeadSeaEmperorSpawner");
    public static PrefabInfo InfectionTimerInfo { get; } = PrefabInfo.WithTechType("InfectionTimer");
    public static PrefabInfo InfectedCorpseInfo { get; } = PrefabInfo.WithTechType("InfectedCorpse");
    public static PrefabInfo SkeletonCorpse { get; } = PrefabInfo.WithTechType("SkeletonCorpse");
    public static PrefabInfo NpcSurvivorManager { get; } = PrefabInfo.WithTechType("NpcSurvivorManager");
    public static PrefabInfo WarperHeart { get; } = PrefabInfo.WithTechType("WarperHeart", "Warper heart", "A warper's bio-mechanical heart. While unstable due to the infection, the power can still be utilized for your own purposes.");
    public static PrefabInfo PlagueKnife { get; } = PrefabInfo.WithTechType("PlagueKnife", "Plague knife", "A more powerful upgrade of the survival knife, with a connection to the infected defense network.");
    public static PrefabInfo MutantDiver1 { get; } = PrefabInfo.WithTechType("MutantDiver1", "Mutant diver", "A mutated diver.");
    public static PrefabInfo MutantDiver2 { get; } = PrefabInfo.WithTechType("MutantDiver2", "Mutant diver", "A mutated diver.");
    public static PrefabInfo MutantDiver3 { get; } = PrefabInfo.WithTechType("MutantDiver3", "Heavily mutated diver", "A heavily mutated diver.");
    public static PrefabInfo MutantDiver4 { get; } = PrefabInfo.WithTechType("MutantDiver4", "Heavily mutated diver", "DEATH IS JUST A BREATH AWAY");

    public static PrefabInfo PlagueHeart { get; } = PrefabInfo.WithTechType("PlagueHeart", "Heart of the plague",
        "DISEASE CONCENTRATION: LETHAL. FIND A CURE AS QUICKLY AS POSSIBLE.");

    public static PrefabInfo EnzymeContainer { get; } = PrefabInfo.WithTechType("ConcentratedEnzymeContainer", "Concentrated Enzyme 42",
        "A container of concentrated enzyme 42, ready to be placed into a machine for mass distribution.");

    public static PrefabInfo EnzymeParticleInfo { get; } = PrefabInfo.WithTechType("InfectedEnzyme", "Impure Enzyme 42",
        "A small concentration of Enzyme 42. Not very useful until fabricated into a concentrated solution.");

    private static PrefabInfo InfectionTrackerInfo { get; } = PrefabInfo.WithTechType("InfectionTracker",
        "Tracker tablet", "A tablet that directs its user to a certain location.");
    
    private static PrefabInfo PrecursorPhoneInfo { get; } = PrefabInfo.WithTechType("PrecursorPhone",
        "Precursor iPhone", "An unusual alien device.");

    public static void RegisterPrefabs()
    {
        // Add new biome
        var infectedZoneSettings = BiomeUtils.CreateBiomeSettings(new Vector3(7f, 4.5f, 3f), 0.4f,
            new Color(1.05f, 1f, 1f, 1), 2f, new Color(1f, 0.3f, 0.3f), 0f, 10f, 0.5f, 0.9f, 20f);
        BiomeHandler.RegisterBiome("infectedzone", infectedZoneSettings,
            new BiomeHandler.SkyReference("SkyGrassyPlateaus"));
        var infectedZonePrefab = new CustomPrefab(PrefabInfo.WithTechType("InfectedZoneVolume"));
        var infectedZoneTemplate = new AtmosphereVolumeTemplate(infectedZonePrefab.Info,
            AtmosphereVolumeTemplate.VolumeShape.Sphere, "infectedzone");
        infectedZonePrefab.SetGameObject(infectedZoneTemplate);
        infectedZonePrefab.Register();
        
        var plagueHeartBiomeSettings = BiomeUtils.CreateBiomeSettings(new Vector3(14, 9, 9), 1.3f,
            new Color(1.05f, 1f, 1f, 1), 2f, new Color(1f, 0.3f, 0.3f), 0.05f, 10f, 0.5f, 0.5f, 18f);
        BiomeHandler.RegisterBiome("plagueheart", plagueHeartBiomeSettings,
            new BiomeHandler.SkyReference("SkyGrassyPlateaus"));

        var infectionDome = new CustomPrefab(InfectionDomeInfo);
        infectionDome.SetGameObject(GetInfectionDomePrefab);
        infectionDome.SetSpawns(new SpawnLocation(Vector3.zero, Vector3.zero, Vector3.one * 1000));
        infectionDome.Register();

        var islandElevator = new CustomPrefab(PrefabInfo.WithTechType("IslandElevator"));
        var elevatorTemplate = new CloneTemplate(islandElevator.Info, "51e58608-a80b-4135-9143-add4ce77a42f");
        elevatorTemplate.ModifyPrefab += (go) =>
        {
            go.transform.Find("precursor_gun_Elevator_shell").gameObject.SetActive(false);
            go.transform.Find("mesh").gameObject.SetActive(false);
            go.transform.Find("Elevator_Collision").gameObject.SetActive(false);
            go.transform.Find("CullVolumeManager").gameObject.SetActive(false);
            go.transform.Find("Occluder_precursor_gun_Elevator_shell").gameObject.SetActive(false);
            var fxParent = go.transform.Find("FX");
            foreach (var renderer in fxParent.GetComponentsInChildren<Renderer>(true))
            {
                if (renderer.gameObject.name.Contains("SquareLights"))
                {
                    renderer.material.color = Color.red;
                }
                else if (renderer.gameObject.name.Contains("Tube"))
                {
                    var materials = renderer.materials;
                    materials[0].color = new Color(5, 2, 0.27f);
                    materials[1].color = new Color(2, 0.38f, 0.43f);
                    renderer.materials = materials;
                }
            }

            go.GetComponent<LargeWorldEntity>().cellLevel = LargeWorldEntity.CellLevel.Global;

            go.transform.Find("FX/Point light").GetComponent<Light>().color = Color.red;
        };
        islandElevator.SetGameObject(elevatorTemplate);
        islandElevator.SetSpawns(new SpawnLocation(new Vector3(-48.000f, 56.000f, -40.000f), Vector3.up * 90,
            new Vector3(1.5f, 3.45f, 1.5f)));
        islandElevator.Register();

        var infectionCubePlatform = new CustomPrefab(PrefabInfo.WithTechType("InfectionCubePlatform"));
        var infectionCubeTemplate =
            new CloneTemplate(infectionCubePlatform.Info, "6b0104e8-979e-46e5-bc17-57c4ac2e6e39");
        infectionCubeTemplate.ModifyPrefab += go =>
        {
            Object.DestroyImmediate(go.GetComponent<DisableEmissiveOnStoryGoal>());
        };
        infectionCubePlatform.SetGameObject(infectionCubeTemplate);
        infectionCubePlatform.SetSpawns(
            new SpawnLocation(new Vector3(-54.508f, -3.523f, -42.000f), new Vector3(340.496f, 30.000f, 270.000f),
                Vector3.one * 0.4f),
            new SpawnLocation(new Vector3(-60.932f, 308.929f, -52.616f), new Vector3(0, 103, 0),
                new Vector3(0.4f, 0.2f, 0.4f)),
            new SpawnLocation(new Vector3(2.306f, 341.463f, -25.395f), new Vector3(90, 68.57f, 0),
                new Vector3(0.9f, 0.9f, 0.9f)),
            new SpawnLocation(new Vector3(-55.196f, 386.394f, -1.448f), Vector3.zero, Vector3.one),
            new SpawnLocation(new Vector3(-71.367f, 314.375f, -92.013f), new Vector3(15.802f, 359.521f, 356.553f),
                Vector3.one * 0.7f));
        infectionCubePlatform.Register();

        var lowQualityForceFieldIsland = new CustomPrefab(PrefabInfo.WithTechType("ForceFieldIslandLowQuality"));
        lowQualityForceFieldIsland.SetGameObject(GetLowQualityForceFieldIslandPrefab);
        lowQualityForceFieldIsland.SetSpawns(new SpawnLocation(new Vector3(-128, 160, -128), new Vector3(0, 180, 0),
            new Vector3(1, 1, -1)));
        lowQualityForceFieldIsland.Register();

        var forceFieldIslandLightPrefab = new CustomPrefab(PrefabInfo.WithTechType("ForceFieldIslandLight"));
        var forceFieldIslandLightTemplate =
            new CloneTemplate(forceFieldIslandLightPrefab.Info, "081ef6c1-aa78-46fd-a20f-a6b63ca5c5f3");
        forceFieldIslandLightTemplate.ModifyPrefab += (go) =>
        {
            go.GetComponents<DisableEmissiveOnStoryGoal>().ForEach(c => c.enabled = false);
            go.GetComponents<LightIntensityOnStoryGoal>().ForEach(c => c.enabled = false);
            go.GetComponent<SkyApplier>().customSkyPrefab = null;
        };
        forceFieldIslandLightPrefab.SetGameObject(forceFieldIslandLightTemplate);
        forceFieldIslandLightPrefab.Register();
        
        var precursorPhone = new CustomPrefab(PrecursorPhoneInfo);
        var precursorPhoneTemplate =
            new CloneTemplate(precursorPhone.Info, "081ef6c1-aa78-46fd-a20f-a6b63ca5c5f3");
        precursorPhoneTemplate.ModifyPrefab += (go) =>
        {
            go.GetComponents<DisableEmissiveOnStoryGoal>().ForEach(c => c.enabled = false);
            go.GetComponents<LightIntensityOnStoryGoal>().ForEach(c => c.enabled = false);
            go.GetComponentInChildren<Light>().enabled = false;
            go.GetComponentInChildren<VFXVolumetricLight>().gameObject.SetActive(false);
            go.GetComponent<SkyApplier>().customSkyPrefab = null;
            go.transform.localScale = new Vector3(0.2f, 0.1f, 0.3f);
            go.gameObject.AddComponent<Pickupable>();
        };
        precursorPhone.SetGameObject(precursorPhoneTemplate);
        precursorPhone.SetSpawns(new SpawnLocation(new Vector3(-1324.541f, -206.655f, 266.916f), new Vector3(45.373f, 276.516f, 230.206f), new Vector3(0.2f, 0.1f, 0.3f)));
        precursorPhone.Register();
        
        var forceFieldIslandLight2Prefab = new CustomPrefab(PrefabInfo.WithTechType("ForceFieldIslandLight2"));
        var forceFieldIslandLight2Template =
            new CloneTemplate(forceFieldIslandLight2Prefab.Info, "081ef6c1-aa78-46fd-a20f-a6b63ca5c5f3");
        forceFieldIslandLight2Template.ModifyPrefab += (go) =>
        {
            go.GetComponents<DisableEmissiveOnStoryGoal>().ForEach(c => c.enabled = false);
            go.GetComponents<LightIntensityOnStoryGoal>().ForEach(c => c.enabled = false);
            go.GetComponentInChildren<Light>().enabled = false;
            go.GetComponent<SkyApplier>().customSkyPrefab = null;
        };
        forceFieldIslandLight2Prefab.SetGameObject(forceFieldIslandLight2Template);
        forceFieldIslandLight2Prefab.Register();

        var infectionLaserColumnPrefab = new CustomPrefab(PrefabInfo.WithTechType("InfectionLaserColumn"));
        var infectionLaserColumnTemplate =
            new CloneTemplate(infectionLaserColumnPrefab.Info, "3d625dbb-d15a-4351-bca0-0a0526f01e6e");
        infectionLaserColumnTemplate.ModifyPrefab += go =>
        {
            go.GetComponents<DisableEmissiveOnStoryGoal>().ForEach(c => c.enabled = false);
            go.GetComponents<LightIntensityOnStoryGoal>().ForEach(c => c.enabled = false);
            go.GetComponents<PrecursorGunCentralColumn>().ForEach(c => c.enabled = false);
            go.transform
                .Find(
                    "precursor_column_maze_08_06_08_hlpr/precursor_column_maze_08_06_08_ctrl/precursor_column_maze_08_06_08/light")
                .GetComponent<Light>().color = Color.red;
            go.transform
                .Find(
                    "precursor_column_maze_08_06_08_hlpr/precursor_column_maze_08_06_08_ctrl/precursor_column_maze_08_06_08/precursor_column_maze_08_06_08_glass_01")
                .GetComponent<Renderer>().material.color = Color.red;
            var renderer = go.transform
                .Find(
                    "precursor_column_maze_08_06_08_hlpr/precursor_column_maze_08_06_08_ctrl/precursor_column_maze_08_06_08/precursor_column_maze_08_06_08_glass_02_hlpr/precursor_column_maze_08_06_08_glass_02_ctrl/precursor_column_maze_08_06_08_glass_02")
                .GetComponent<Renderer>();
            var materials = renderer.materials;
            materials[1].color = Color.red;
            renderer.materials = materials;
            go.AddComponent<InfectAnything>();
        };
        infectionLaserColumnPrefab.SetGameObject(infectionLaserColumnTemplate);
        infectionLaserColumnPrefab.SetSpawns(new SpawnLocation(new Vector3(-59.640f, 301.000f, -24.840f),
            Vector3.up * 343, Vector3.one * 0.7f));
        infectionLaserColumnPrefab.Register();

        var infectionLaserTerminalPrefab = new CustomPrefab(PrefabInfo.WithTechType("InfectionLaserTerminal"));
        var infectionLaserTerminalTemplate =
            new CloneTemplate(infectionLaserTerminalPrefab.Info, "b1f54987-4652-4f62-a983-4bf3029f8c5b");
        infectionLaserTerminalTemplate.ModifyPrefab += go =>
        {
            go.AddComponent<InfectAnything>();
            Object.DestroyImmediate(go.GetComponent<DisableEmissiveOnStoryGoal>());
            var trigger = go.transform.Find("triggerArea");
            var terminalObj = go.transform.Find("terminal");
            var disable = trigger.GetComponent<PrecursorDisableGunTerminalArea>();
            var originalTerminal = terminalObj.GetComponent<PrecursorDisableGunTerminal>();
            Object.DestroyImmediate(disable);
            var terminal = terminalObj.gameObject.AddComponent<DisableDomeTerminal>();
            terminal.accessGrantedSound = originalTerminal.accessGrantedSound;
            terminal.accessDeniedSound = originalTerminal.accessDeniedSound;
            terminal.cinematic = originalTerminal.cinematic;
            terminal.useSound = originalTerminal.useSound;
            terminal.openLoopSound = originalTerminal.openLoopSound;
            terminal.onPlayerCuredGoal = originalTerminal.onPlayerCuredGoal;
            terminal.glowRing = originalTerminal.glowRing;
            terminal.glowMaterial = originalTerminal.glowMaterial;
            
            Object.DestroyImmediate(originalTerminal);
            trigger.gameObject.AddComponent<DisableDomeArea>().terminal = terminal;
            // implement custom modifications here
        };
        infectionLaserTerminalPrefab.SetGameObject(infectionLaserTerminalTemplate);
        infectionLaserTerminalPrefab.SetSpawns(new SpawnLocation(new Vector3(-60.053f, 301.044f, -23.278f),
            Vector3.up * 163));
        infectionLaserTerminalPrefab.Register();

        var infectionControlRoomPrefab = new CustomPrefab(PrefabInfo.WithTechType("InfectionControlRoom"));
        var infectionControlRoomTemplate =
            new CloneTemplate(infectionControlRoomPrefab.Info, "963fa3a3-9192-4912-8c8d-d0d98f22ed13");
        infectionControlRoomTemplate.ModifyPrefab += go =>
        {
            go.transform.GetChild(0).gameObject.SetActive(false);
            go.transform.GetChild(3).gameObject.SetActive(false);
            var colliders = go.transform.Find("Control_Room_Collision/Cube").GetComponents<Collider>();
            var colliderIndicesToDisable = new int[] {11, 12, 19, 49};
            foreach (var index in colliderIndicesToDisable)
            {
                colliders[index].enabled = false;
            }

            var modelParent = go.transform.Find("Precursor_Gun_ControlRoom");
            var modelIndicesToDisable = new int[] {66, 67, 69, 70, 78, 100, 110, 111, 112, 113};
            foreach (var index in modelIndicesToDisable)
            {
                modelParent.GetChild(index).gameObject.SetActive(false);
            }
        };
        infectionControlRoomPrefab.SetGameObject(infectionControlRoomTemplate);
        infectionControlRoomPrefab.SetSpawns(new SpawnLocation(new Vector3(-65.050f, 302.850f, -20.260f),
            Vector3.up * 343, Vector3.one * 0.42f));
        infectionControlRoomPrefab.Register();

        var infectionLaserDevicePrefab = new CustomPrefab(PrefabInfo.WithTechType("InfectionLaserDevice"));
        var infectionLaserDeviceTemplate =
            new CloneTemplate(infectionLaserTerminalPrefab.Info, "22fb9ee9-690d-426c-844f-a80e527b5fe6");
        infectionLaserDeviceTemplate.ModifyPrefab += go =>
        {
            go.GetComponent<LargeWorldEntity>().cellLevel = LargeWorldEntity.CellLevel.Global;
            go.GetComponents<DisableEmissiveOnStoryGoal>().ForEach(c => c.enabled = false);
            go.GetComponents<LightIntensityOnStoryGoal>().ForEach(c => c.enabled = false);
            Object.DestroyImmediate(go.GetComponent<PrecursorGunStoryEvents>());
            Object.DestroyImmediate(go.GetComponent<PrecursorGunAim>());
            Object.DestroyImmediate(go.GetComponent<AnimateOnStoryGoal>());
            var modelParent = go.transform.Find("precursor_base/Instances");
            modelParent.Find("precursor_base_22").gameObject.SetActive(false);
            modelParent.Find("precursor_base_23").gameObject.SetActive(false);
            modelParent.Find("precursor_base_24").gameObject.SetActive(false);
            modelParent.Find("precursor_base_25").gameObject.SetActive(false);
        };
        infectionLaserDevicePrefab.SetGameObject(infectionLaserDeviceTemplate);
        infectionLaserDevicePrefab.SetSpawns(new SpawnLocation(new Vector3(-80.000f, 304, -47.790f),
            new Vector3(0, 0, 353), Vector3.one * 0.3f));
        infectionLaserDevicePrefab.Register();

        var infectionLaserPrefab = new CustomPrefab(InfectionLaserInfo);
        infectionLaserPrefab.SetGameObject(GetInfectionLaserPrefab);
        infectionLaserPrefab.SetSpawns(new SpawnLocation(Vector3.zero));
        infectionLaserPrefab.Register();

        var infectionTrackerPrefab = new CustomPrefab(InfectionTrackerInfo);
        var infectionTrackerTemplate = new CloneTemplate(InfectionTrackerInfo, "b98da0ef-29d4-4571-9a82-53a6e6706153");
        infectionTrackerTemplate.ModifyPrefabAsync += ModifyInfectionTrackerPrefab;
        infectionTrackerPrefab.SetGameObject(infectionTrackerTemplate);
        infectionTrackerPrefab.SetEquipment(EquipmentType.Hand);
        infectionTrackerPrefab.SetSpawns(new SpawnLocation(new Vector3(-52.575f, 312.560f, -68.644f),
            new Vector3(4.3f, 0.68f, 18f)));
        infectionTrackerPrefab.Info.WithIcon(Plugin.AssetBundle.LoadAsset<Sprite>("InfectionTrackerIcon"));
        infectionTrackerPrefab.Register();

        var infectedReaperSkeleton = MakeInfectedClone(PrefabInfo.WithTechType("InfectedReaperSkeleton"), "8fe779a5-e907-4e9e-b748-1eee25589b34", 4f);
        infectedReaperSkeleton.SetSpawns(
            new LootDistributionData.BiomeData { biome = BiomeType.Dunes_SandDune, count = 1, probability = 0.04f }
        );
        infectedReaperSkeleton.Register();

        var reaperWithoutSkullModification = (GameObject go) =>
        {
            foreach (Transform child in go.transform)
            {
                var name = child.gameObject.name;
                if (name.Contains("bone") || name.Contains("skull"))
                {
                    child.gameObject.SetActive(false);
                }
            }
        };
        
        var infectedReaperSkeletonNoSkull = MakeInfectedClone(PrefabInfo.WithTechType("InfectedReaperSkeletonNoSkull"), "8fe779a5-e907-4e9e-b748-1eee25589b34", 4f, reaperWithoutSkullModification);
        infectedReaperSkeletonNoSkull.SetSpawns(
            new LootDistributionData.BiomeData { biome = BiomeType.SafeShallows_SandFlat, count = 1, probability = 0.04f },
            new LootDistributionData.BiomeData { biome = BiomeType.GrassyPlateaus_Sand, count = 1, probability = 0.04f },
            new LootDistributionData.BiomeData { biome = BiomeType.Kelp_GrassSparse, count = 1, probability = 0.02f }
        );
        infectedReaperSkeletonNoSkull.Register();
        
        var infectedGenericSkeleton1 = MakeInfectedClone(PrefabInfo.WithTechType("InfectedSkeleton1"), "0b6ea118-1c0b-4039-afdb-2d9b26401ad2", 7f);
        infectedGenericSkeleton1.SetSpawns(
            new LootDistributionData.BiomeData { biome = BiomeType.Dunes_SandDune, count = 1, probability = 0.02f },
            new LootDistributionData.BiomeData { biome = BiomeType.Kelp_GrassDense, count = 1, probability = 0.08f },
            new LootDistributionData.BiomeData { biome = BiomeType.MushroomForest_Sand, count = 1, probability = 0.08f }
        );
        infectedGenericSkeleton1.Register();

        var infectedGenericSkeleton2 = MakeInfectedClone(PrefabInfo.WithTechType("InfectedSkeleton2"), "e10ff9a1-5f1e-4c4d-bf5f-170dba9e321b", 8f);
        infectedGenericSkeleton2.SetSpawns(
            new LootDistributionData.BiomeData { biome = BiomeType.Dunes_Rock, count = 1, probability = 0.02f },
            new LootDistributionData.BiomeData { biome = BiomeType.MushroomForest_Grass, count = 1, probability = 0.1f }
        );
        infectedGenericSkeleton2.Register();
        
        var infectedRib2 = MakeInfectedClone(PrefabInfo.WithTechType("InfectedRib"), "33c31a89-9d3b-4717-ad26-4cc8106a1f24", 2f);
        infectedRib2.SetSpawns(new LootDistributionData.BiomeData
            { biome = BiomeType.Dunes_Rock, count = 1, probability = 0.1f },
            new LootDistributionData.BiomeData
                { biome = BiomeType.Dunes_Grass, count = 1, probability = 0.1f }
        );
        infectedRib2.Register();

        var plagueHeart = new CustomPrefab(PlagueHeart);
        plagueHeart.SetGameObject(GetPlagueHeartPrefab);
        PlagueHeart.WithIcon(Plugin.AssetBundle.LoadAsset<Sprite>("PlagueHeartIcon"));
        plagueHeart.SetSpawns(new SpawnLocation(new Vector3(-1319.740f, -223.150f, 280)));
        plagueHeart.Register();

        var laserReceptacle = new CustomPrefab(LaserReceptacleInfo);
        laserReceptacle.SetGameObject(GetLaserReceptaclePrefab);
        laserReceptacle.SetSpawns(new SpawnLocation(new Vector3(-66.123f, 302.018f, -30.484f), new Vector3(0, 343, 0)));
        laserReceptacle.Register();

        var enzymeParticle = new CustomPrefab(EnzymeParticleInfo);
        enzymeParticle.SetGameObject(GetEnzymeParticlePrefab);
        EnzymeParticleInfo.WithIcon(Plugin.AssetBundle.LoadAsset<Sprite>("InfectedEnzyme42"));
        enzymeParticle.Register();

        var enzymeContainer = new CustomPrefab(EnzymeContainer);
        EnzymeContainer.WithIcon(SpriteManager.Get(TechType.LabContainer3)).WithSizeInInventory(new Vector2int(2, 2));
        var enzymeContainerTemplate = new CloneTemplate(enzymeContainer.Info, TechType.LabContainer3);
        enzymeContainerTemplate.ModifyPrefab += (go) =>
        {
            var renderer = go.transform.Find("biodome_lab_containers_tube_01/biodome_lab_containers_tube_01_glass").GetComponent<Renderer>();
            var material = renderer.material;
            material.color = new Color(1, 1, 1, 0.19f);
            material.SetColor(ShaderPropertyID._SpecColor, new Color(20, 12, 0, 1));
            material.SetFloat("_Shininess", 3);
            if (go.GetComponent<VFXFabricating>() == null)
            {
                PrefabUtils.AddVFXFabricating(go, null, -0.05f, 0.3f);
            }
        };
        enzymeContainer.SetGameObject(enzymeContainerTemplate);
        enzymeContainer.SetRecipe(new RecipeData(new CraftData.Ingredient(enzymeParticle.Info.TechType, 16),
                new CraftData.Ingredient(TechType.Titanium, 2), new CraftData.Ingredient(TechType.Glass, 1)))
            .WithStepsToFabricatorTab("Resources", "AdvancedMaterials").WithCraftingTime(6)
            .WithFabricatorType(CraftTree.Type.Fabricator);
        enzymeContainer.AddGadget(new ScanningGadget(enzymeContainer, TechType.None))
            .WithPdaGroupCategory(TechGroup.Resources, TechCategory.AdvancedMaterials)
            .WithAnalysisTech(Plugin.AssetBundle.LoadAsset<Sprite>("InfectedEnzymeStorageContainer_Popup"),
                KnownTechHandler.DefaultUnlockData.BlueprintUnlockSound,
                KnownTechHandler.DefaultUnlockData.BlueprintUnlockMessage);
        enzymeContainer.Register();

        var deadEmperor = new CustomPrefab(DeadSeaEmperorInfo);
        deadEmperor.SetGameObject(GetDeadEmperorPrefab);
        deadEmperor.Register();

        var deadEmperorSpawner = new CustomPrefab(DeadSeaEmperorSpawnerInfo);
        deadEmperorSpawner.SetGameObject(() =>
        {
            var go = new GameObject(DeadSeaEmperorSpawnerInfo.ClassID);
            PrefabUtils.AddBasicComponents(go, DeadSeaEmperorSpawnerInfo.ClassID, DeadSeaEmperorSpawnerInfo.TechType, LargeWorldEntity.CellLevel.Global);
            go.AddComponent<DeadSeaEmperorSpawner>();
            go.SetActive(false);
            return go;
        });
        deadEmperorSpawner.Register();

        var infectionTimer = new CustomPrefab(InfectionTimerInfo);
        infectionTimer.SetGameObject(GetInfectionTimerPrefab);
        infectionTimer.Register();
        
        var npcSurvivorManager = new CustomPrefab(NpcSurvivorManager);
        npcSurvivorManager.SetGameObject(GetNPCSurvivorManagerPrefab);
        npcSurvivorManager.SetSpawns(new SpawnLocation(Vector3.zero));
        npcSurvivorManager.Register();

        var infectedCorpse = new CorpsePrefab(InfectedCorpseInfo, "DiverCorpse", true);
        infectedCorpse.Register();
        
        var skeletonCorpse = new CorpsePrefab(SkeletonCorpse, "SkeletonRagdoll", true);
        skeletonCorpse.Register();

        var warperHeart = new CustomPrefab(WarperHeart);
        warperHeart.SetGameObject(GetWarperInnardsPrefab);
        warperHeart.Info.WithIcon(Plugin.AssetBundle.LoadAsset<Sprite>("WarperHeartIcon"));
        warperHeart.Register();

        var plagueKnife = new CustomPrefab(PlagueKnife);
        var plagueKnifeTemplate = new CloneTemplate(PlagueKnife, TechType.Knife);
        plagueKnifeTemplate.ModifyPrefab += (go) =>
        {
            var renderer = go.GetComponentInChildren<Renderer>();
            var material = new Material(MaterialUtils.IonCubeMaterial);
            material.SetColor(ShaderPropertyID._Color, Color.black);
            material.SetColor(ShaderPropertyID._SpecColor, Color.black);
            material.SetColor(ShaderPropertyID._SpecColor, Color.black);
            material.SetColor(ShaderPropertyID._GlowColor, Color.red);
            material.SetFloat(ShaderPropertyID._GlowStrength, 2.2f);
            material.SetFloat(ShaderPropertyID._GlowStrengthNight, 2.2f);
            material.SetColor("_DetailsColor", Color.red);
            material.SetColor("_SquaresColor", new Color(3, 2, 1));
            material.SetFloat("_SquaresTile", 5.6f);
            material.SetFloat("_SquaresSpeed", 8.8f);
            material.SetVector("_NoiseSpeed", new Vector4(0.5f, 0.3f, 0f));
            material.SetVector("_FakeSSSParams", new Vector4(0.2f, 1f, 1f));
            material.SetVector("_FakeSSSSpeed", new Vector4(0.5f, 0.5f, 1.37f));
            renderer.material = material;

            var oldKnifeComponent = go.GetComponent<Knife>();
            var newKnifeComponent = go.AddComponent<PlagueKnifeTool>();
            newKnifeComponent.attackSound = AudioUtils.GetFmodAsset("event:/creature/warper/swipe");
            newKnifeComponent.underwaterMissSound = AudioUtils.GetFmodAsset("event:/creature/warper/swipe");
            newKnifeComponent.surfaceMissSound = oldKnifeComponent.surfaceMissSound;
            newKnifeComponent.damageType = oldKnifeComponent.damageType;
            newKnifeComponent.damage = 50;
            newKnifeComponent.attackDist = 4;
            newKnifeComponent.vfxEventType = VFXEventTypes.knife;
            newKnifeComponent.mainCollider = oldKnifeComponent.mainCollider;
            newKnifeComponent.drawSound = oldKnifeComponent.drawSound;
            newKnifeComponent.firstUseSound = oldKnifeComponent.firstUseSound;
            newKnifeComponent.hitBleederSound = oldKnifeComponent.hitBleederSound;
            newKnifeComponent.bleederDamage = 50;
            newKnifeComponent.socket = oldKnifeComponent.socket;
            newKnifeComponent.ikAimRightArm = true;
            newKnifeComponent.drawTime = 0;
            newKnifeComponent.holsterTime = 0.1f;
            newKnifeComponent.pickupable = oldKnifeComponent.pickupable;
            newKnifeComponent.hasFirstUseAnimation = true;
            newKnifeComponent.hasBashAnimation = true;
            Object.DestroyImmediate(oldKnifeComponent);
        };
        plagueKnife.SetGameObject(plagueKnifeTemplate);
        plagueKnife.SetEquipment(EquipmentType.Hand);
        plagueKnife.SetRecipe(new RecipeData(new CraftData.Ingredient(TechType.Knife, 1),
                new CraftData.Ingredient(WarperHeart.TechType, 3)))
            .WithCraftingTime(5)
            .WithFabricatorType(CraftTree.Type.Workbench);
        plagueKnife.Register();
        
        var mutantDiver1 = new Mutant(MutantDiver1, "MutatedDiver1", false);
        mutantDiver1.Register();
        
        var mutantDiver2 = new Mutant(MutantDiver2, "MutatedDiver2", false);
        mutantDiver2.Register();

        var mutantDiver3 = new Mutant(MutantDiver3, "MutatedDiver3", true);
        mutantDiver3.Register();

        var mutantDiver4 = new Mutant(MutantDiver4, "MutatedDiver4", true);
        mutantDiver4.Register();

    }

    private static CustomPrefab MakeInfectedClone(PrefabInfo info, string cloneClassID, float scale, Action<GameObject> modifyPrefab = null)
    {
        var prefab = new CustomPrefab(info);
        var template = new CloneTemplate(prefab.Info, cloneClassID);
        if (modifyPrefab != null)
        {
            template.ModifyPrefab += modifyPrefab;
        }
        template.ModifyPrefab += go =>
        {
            go.AddComponent<InfectAnything>();
            if (Math.Abs(scale - 1f) > 0.001f)
            {
                var scaler = new GameObject("Scaler").transform;
                scaler.parent = go.transform;
                scaler.localPosition = Vector3.zero;
                while (go.transform.childCount > 1)
                {
                    go.transform.GetChild(0).parent = scaler;
                }

                scaler.transform.localScale = Vector3.one * scale;
            }
        };
        prefab.SetGameObject(template);
        return prefab;
    }

    private static IEnumerator ModifyInfectionTrackerPrefab(GameObject go)
    {
        go.SetActive(false);
        go.GetComponentInChildren<Renderer>(true).material.SetColor(ShaderPropertyID._GlowColor, new Color(3, 0, 0));
        go.GetComponentsInChildren<Collider>(true).ForEach(c => c.enabled = false);
        go.GetComponentsInChildren<Animator>(true).ForEach(c => c.enabled = false);
        var collider = go.AddComponent<BoxCollider>();
        collider.size = new Vector3(0.7f, 0.07f, 0.7f);
        var tool = go.AddComponent<InfectionTrackerTool>();
        tool.mainCollider = collider;
        tool.drawSound = AudioUtils.GetFmodAsset("event:/interface/off_long");
        tool.hasAnimations = false;
        tool.pickupable = go.AddComponent<Pickupable>();
        tool.renderers = go.GetComponentsInChildren<Renderer>();
        tool.hasAnimations = true;

        var worldModel = go.GetComponentInChildren<Animator>().gameObject;

        var viewModel = Object.Instantiate(worldModel, go.transform);
        viewModel.SetActive(false);
        viewModel.transform.localScale = Vector3.one * 0.6f;
        viewModel.transform.localPosition = new Vector3(-0.15f, 0.01f, 0.2f);
        viewModel.transform.localEulerAngles = new Vector3(275, 180, 0);
            
        tool.ikAimRightArm = true;

        var fpModel = go.EnsureComponent<FPModel>();
        fpModel.propModel = worldModel;
        fpModel.viewModel = viewModel;

        var diveReelTask = CraftData.GetPrefabForTechTypeAsync(TechType.DiveReel);
        yield return diveReelTask;
        var arrow = Object.Instantiate(diveReelTask.GetResult().GetComponent<DiveReel>().nodePrefab.transform.Find("Arrow").gameObject, go.transform);
        arrow.SetActive(false);
        var arrowCenter = new GameObject("ArrowCenter").transform;
        arrowCenter.parent = viewModel.transform.GetChild(0).GetChild(0);
        arrowCenter.localPosition = Vector3.forward * 0.15f;
        arrow.GetComponentInChildren<Renderer>(true).material.color = Color.red;
        arrow.GetComponentInChildren<Renderer>(true).material.SetColor(ShaderPropertyID._ColorStrength, new Color(1.5f, 0, 0));
        arrow.GetComponentInChildren<Renderer>(true).material.SetColor(ShaderPropertyID._ColorStrengthAtNight, new Color(1.5f, 0, 0));

        tool.arrowPrefab = arrow;
        tool.arrowRoot = arrowCenter;
        tool.viewModel = viewModel;

        var rb = go.EnsureComponent<Rigidbody>();
        rb.mass = 200;
        rb.useGravity = false;
        go.EnsureComponent<WorldForces>();
        go.SetActive(true);
    }

    private static IEnumerator GetInfectionDomePrefab(IOut<GameObject> prefab)
    {
        var obj = Object.Instantiate(Plugin.AssetBundle.LoadAsset<GameObject>("InfectionDome"));
        MaterialUtils.ApplySNShaders(obj);
        PrefabUtils.AddBasicComponents(obj, InfectionDomeInfo.ClassID, InfectionDomeInfo.TechType,
            LargeWorldEntity.CellLevel.Global);
        var renderer = obj.GetComponentInChildren<Renderer>();
        var material = new Material(MaterialUtils.ForceFieldMaterial);
        material.SetColor("_Color", new Color(1, 0, 0, 0.95f));
        material.SetVector("_MainTex2_Speed", new Vector4(-0.01f, 0.1f, 0, 0));
        var materials = renderer.materials;
        materials[2] = material;
        renderer.materials = materials;
        
        var solarPanelRequest = CraftData.GetPrefabForTechTypeAsync(TechType.SolarPanel);
        yield return solarPanelRequest;
        var solarPanelPrefab = solarPanelRequest.GetResult();
        
        var linePrefab = Object.Instantiate(solarPanelPrefab.GetComponent<PowerFX>().vfxPrefab, obj.transform);
        linePrefab.SetActive(false);
        var line = linePrefab.GetComponent<LineRenderer>();
        var newMaterial = new Material(line.material);
        newMaterial.color = new Color(4, 0, 0);
        line.material = newMaterial;
        line.widthMultiplier = 1;

        var lightning = obj.AddComponent<InfectionDomeController>();
        lightning.linePrefab = linePrefab;
        
        prefab.Set(obj);
    }

    private static IEnumerator GetLowQualityForceFieldIslandPrefab(IOut<GameObject> prefab)
    {
        var obj = Object.Instantiate(Plugin.AssetBundle.LoadAsset<GameObject>("ForcefieldIslandLowQuality"));
        MaterialUtils.ApplySNShaders(obj);
        PrefabUtils.AddBasicComponents(obj, InfectionDomeInfo.ClassID, InfectionDomeInfo.TechType,
            LargeWorldEntity.CellLevel.Global);
        obj.AddComponent<LowQualityIslandMesh>();
        // var renderer = obj.GetComponentInChildren<Renderer>();
        yield return null;
        prefab.Set(obj);
    }
    
    private static IEnumerator GetInfectionTimerPrefab(IOut<GameObject> prefab)
    {
        var go = new GameObject("InfectionTimer");
        go.SetActive(false);
        PrefabUtils.AddBasicComponents(go, InfectionTimerInfo.ClassID, InfectionTimerInfo.TechType, LargeWorldEntity.CellLevel.Global);
        go.AddComponent<PlayerInfectionDeath>();
        yield return null;
        prefab.Set(go);
    }

    private static IEnumerator GetPlagueHeartPrefab(IOut<GameObject> prefab)
    {
        var obj = Object.Instantiate(Plugin.AssetBundle.LoadAsset<GameObject>("PlagueHeartPrefab"));
        obj.SetActive(false);
        var renderer = obj.GetComponentInChildren<Renderer>();
        renderer.material = new Material(MaterialUtils.IonCubeMaterial);
        renderer.material.SetColor(ShaderPropertyID._Color, Color.black);
        renderer.material.SetColor(ShaderPropertyID._SpecColor, Color.black);
        renderer.material.SetColor(ShaderPropertyID._SpecColor, Color.black);
        renderer.material.SetColor(ShaderPropertyID._GlowColor, Color.red);
        renderer.material.SetFloat(ShaderPropertyID._GlowStrength, 2.2f);
        renderer.material.SetFloat(ShaderPropertyID._GlowStrengthNight, 2.2f);
        renderer.material.SetColor("_DetailsColor", Color.red);
        renderer.material.SetColor("_SquaresColor", new Color(3, 2, 1));
        renderer.material.SetFloat("_SquaresTile", 78);
        renderer.material.SetFloat("_SquaresSpeed", 8.8f);
        renderer.material.SetVector("_NoiseSpeed", new Vector4(0.5f, 0.3f, 0f));
        renderer.material.SetVector("_FakeSSSParams", new Vector4(0.2f, 1f, 1f));
        renderer.material.SetVector("_FakeSSSSpeed", new Vector4(0.5f, 0.5f, 1.37f));
        
        PrefabUtils.AddBasicComponents(obj, PlagueHeart.ClassID, PlagueHeart.TechType,
            LargeWorldEntity.CellLevel.Global);
        var wf = obj.EnsureComponent<WorldForces>();
        wf.underwaterGravity = 0;
        var rb = obj.EnsureComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.mass = 300;
        rb.useGravity = false;
        var freezeWhenFar = obj.AddComponent<FreezeRigidbodyWhenFar>();
        freezeWhenFar.freezeDist = 4f;
        obj.AddComponent<Pickupable>();
        obj.AddComponent<PlagueHeartBehavior>();

        var emitter = obj.AddComponent<FMOD_CustomLoopingEmitter>();
        emitter.followParent = true;
        emitter.SetAsset(AudioUtils.GetFmodAsset("PlagueHeartAmbience"));
        emitter.playOnAwake = true;

        /*
        var volumeObj = new GameObject("AtmosphereVolume");
        var volume = volumeObj.AddComponent<AtmosphereVolume>();
        volumeObj.AddComponent<SphereCollider>().isTrigger = true;
        volumeObj.layer = LayerID.Trigger;
        volume.overrideBiome = "plagueheart";
        volumeObj.transform.parent = obj.transform;
        volumeObj.transform.localPosition = Vector3.zero;
        volumeObj.transform.localScale = Vector3.one * 12;
        volumeObj.AddComponent<Rigidbody>().isKinematic = true;
        */
        yield return null;
        prefab.Set(obj);
    }
    
    private static IEnumerator GetInfectionLaserPrefab(IOut<GameObject> prefab)
    {
        var solarPanelRequest = CraftData.GetPrefabForTechTypeAsync(TechType.SolarPanel);
        yield return solarPanelRequest;
        var solarPanelPrefab = solarPanelRequest.GetResult();
        var obj = Object.Instantiate(solarPanelPrefab.GetComponent<PowerFX>().vfxPrefab);
        obj.SetActive(false);
        PrefabUtils.AddBasicComponents(obj, InfectionLaserInfo.ClassID, InfectionLaserInfo.TechType,
            LargeWorldEntity.CellLevel.Global);
        var line = obj.GetComponent<LineRenderer>();
        var newMaterial = new Material(line.material);
        newMaterial.color = new Color(5, 0.474790f, 0.977941f);
        line.material = newMaterial;
        line.widthMultiplier = 15;
        line.endWidth = 100;
        line.SetPositions(new[] {new Vector3(-78.393f, 341.175f, -57.684f), new Vector3(0, 2000, 0)});
        obj.AddComponent<LaserMaterialManager>();
        prefab.Set(obj);
    }

    private static IEnumerator GetLaserReceptaclePrefab(IOut<GameObject> prefab)
    {
        var request = UWE.PrefabDatabase.GetPrefabAsync("63e69987-7d34-41f0-aab9-1187ea06e740");
        yield return request;
        request.TryGetPrefab(out var reference);
        var go = Object.Instantiate(reference.transform.Find("Precursor_Teleporter_Activation_Terminal").gameObject);
        go.SetActive(false);
        PrefabUtils.AddBasicComponents(go, LaserReceptacleInfo.ClassID, LaserReceptacleInfo.TechType, LargeWorldEntity.CellLevel.Near);
        var boxCollider = go.AddComponent<BoxCollider>();
        boxCollider.size = new Vector3(1.2f, 2f, 1.3f);
        boxCollider.center = Vector3.up;
        go.AddComponent<LaserReceptacleController>();
        var trigger = new GameObject("Trigger");
        trigger.transform.parent = go.transform;
        trigger.transform.localPosition = Vector3.zero;
        var collider = trigger.AddComponent<SphereCollider>();
        collider.isTrigger = true;
        collider.radius = 5;
        trigger.AddComponent<PrecursorKeyTerminalTrigger>();
        trigger.AddComponent<Rigidbody>().isKinematic = true;
        prefab.Set(go);
    }
    
    private static IEnumerator GetDeadEmperorPrefab(IOut<GameObject> prefab)
    {
        var request = UWE.PrefabDatabase.GetPrefabAsync("871a5ca9-1f2e-4124-8f1e-fac967a464b8");
        yield return request;
        request.TryGetPrefab(out var reference);
        var go = Object.Instantiate(reference.transform.Find("precursor_prison/Leviathan_prison_rig").gameObject);
        go.SetActive(false);
        PrefabUtils.AddBasicComponents(go, DeadSeaEmperorInfo.ClassID, DeadSeaEmperorInfo.TechType, LargeWorldEntity.CellLevel.Global);
        Object.DestroyImmediate(go.GetComponent<EnzymeEmitter>());
        Object.DestroyImmediate(go.GetComponent<SeaEmperor>());
        Object.DestroyImmediate(go.GetComponent<FMOD_CustomEmitter>());
        Object.DestroyImmediate(go.GetComponent<VFXController>());
        foreach (var occludee in go.GetComponentsInChildren<CullingOccludee>())
        {
            Object.DestroyImmediate(occludee);
        }

        go.AddComponent<PlayEmperorDeathAnimation>();
        go.AddComponent<UninfectSeaEmperorOnEnzymeRain>();
        ZombieManager.InfectSeaEmperor(go);
        prefab.Set(go);
    }
    
    private static IEnumerator GetEnzymeParticlePrefab(IOut<GameObject> prefab)
    {
        var request = UWE.PrefabDatabase.GetPrefabAsync("505e7eff-46b3-4ad2-84e1-0fadb7be306c");
        yield return request;
        request.TryGetPrefab(out var reference);
        var go = Object.Instantiate(reference);
        PrefabUtils.AddBasicComponents(go, EnzymeParticleInfo.ClassID, EnzymeParticleInfo.TechType, LargeWorldEntity.CellLevel.VeryFar);
        Object.DestroyImmediate(go.GetComponent<EnzymeBall>());
        Object.DestroyImmediate(go.transform.Find("collider").GetComponent<GenericHandTarget>());
        var renderer = go.transform.Find("Leviathan_enzymeBall_anim/enzymeBall_geo").GetComponent<Renderer>();
        var material = renderer.material;
        material.color = Color.red;
        material.SetColor(ShaderPropertyID._SpecColor, new Color(0.877f, 1f, 0.838f));
        go.AddComponent<Pickupable>();
        prefab.Set(go);
    }

    private static IEnumerator GetNPCSurvivorManagerPrefab(IOut<GameObject> prefab)
    {
        var go = new GameObject(NpcSurvivorManager.ClassID);
        PrefabUtils.AddBasicComponents(go, NpcSurvivorManager.ClassID, NpcSurvivorManager.TechType,
            LargeWorldEntity.CellLevel.Global);
        go.AddComponent<Mono.NpcSurvivorManager>();
        var johnKyle = go.AddComponent<NpcSurvivor>();
        johnKyle.survivorName = "JohnKyle";
        var sylvie = go.AddComponent<NpcSurvivor>();
        sylvie.survivorName = "Sylvie";
        sylvie.model = NpcSurvivor.ModelType.PrawnSuit;
        var simon = go.AddComponent<NpcSurvivor>();
        simon.survivorName = "Simon";
        prefab.Set(go);
        yield break;
    }
    
    private static IEnumerator GetWarperInnardsPrefab(IOut<GameObject> prefab)
    {
        var go = Object.Instantiate(Plugin.AssetBundle.LoadAsset<GameObject>("WarperInnards_Prefab"));
        go.SetActive(false);
        PrefabUtils.AddBasicComponents(go, WarperHeart.ClassID, WarperHeart.TechType,
            LargeWorldEntity.CellLevel.Near);
        go.AddComponent<Pickupable>();
        var rb = go.AddComponent<Rigidbody>();
        rb.mass = 10;
        rb.useGravity = false;
        var wf = go.AddComponent<WorldForces>();
        wf.useRigidbody = rb;
        var warperTask = CraftData.GetPrefabForTechTypeAsync(TechType.Warper);
        yield return warperTask;
        var heartMaterial = new Material(warperTask.GetResult().transform.Find("warper_anim/warper_geos/Warper_geo").gameObject.GetComponent<Renderer>().materials[1]);
        heartMaterial.color = Color.red;
        heartMaterial.SetColor("_SpecColor", Color.red * 4);
        heartMaterial.SetColor("_GlowColor", Color.red * 4);
        heartMaterial.SetFloat("_Shininess", 8);
        go.GetComponentInChildren<Renderer>().material = heartMaterial;
        prefab.Set(go);
    }
}