using System.Collections;
using Nautilus.Assets;
using Nautilus.Assets.Gadgets;
using Nautilus.Assets.PrefabTemplates;
using Nautilus.Handlers;
using Nautilus.Utility;
using TheRedPlague.Mono;
using UnityEngine;

namespace TheRedPlague;

public static class ModPrefabs
{
    private static PrefabInfo InfectionDomeInfo { get; } = PrefabInfo.WithTechType("InfectionDome");
    private static PrefabInfo InfectionLaserInfo { get; } = PrefabInfo.WithTechType("InfectionLaser");
    private static PrefabInfo PlagueHeart { get; } = PrefabInfo.WithTechType("PlagueHeart", "Heart of the plague", "DISEASE CONCENTRATION: LETHAL. FIND A CURE AS QUICKLY AS POSSIBLE.");

    private static PrefabInfo InfectionTrackerInfo { get; } = PrefabInfo.WithTechType("InfectionTracker",
        "Tracker tablet", "A tablet that directs its user to a certain location.");

    public static void RegisterPrefabs()
    {
        // Add new biome
        var infectedZoneSettings = BiomeUtils.CreateBiomeSettings(new Vector3(10, 4.5f, 3f), 0.4f,
            new Color(1.05f, 1f, 1f, 1), 2f, new Color(1f, 0.3f, 0.3f), 0f, 10f, 0.5f, 0.9f, 20f);
        BiomeHandler.RegisterBiome("infectedzone", infectedZoneSettings,
            new BiomeHandler.SkyReference("SkyGrassyPlateaus"));
        var infectedZonePrefab = new CustomPrefab(PrefabInfo.WithTechType("InfectedZoneVolume"));
        var infectedZoneTemplate = new AtmosphereVolumeTemplate(infectedZonePrefab.Info,
            AtmosphereVolumeTemplate.VolumeShape.Sphere, "infectedzone");
        infectedZonePrefab.SetGameObject(infectedZoneTemplate);
        infectedZonePrefab.Register();

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
        };
        forceFieldIslandLightPrefab.SetGameObject(forceFieldIslandLightTemplate);
        forceFieldIslandLightPrefab.Register();

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
        infectionTrackerTemplate.ModifyPrefab += go =>
        {
            go.GetComponentInChildren<Renderer>().material.SetColor(ShaderPropertyID._GlowColor, new Color(3, 0, 0));
            go.GetComponentsInChildren<Collider>().ForEach(c => c.enabled = false);
            go.GetComponentsInChildren<Animator>().ForEach(c => c.enabled = false);
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

            /*
            var leftHandSocket = new GameObject("LeftHandSocket").transform;
            leftHandSocket.SetParent(go.transform, false);
            leftHandSocket.localPosition = Vector3.left * 0.1f;
            var rightHandSocket = new GameObject("RightHandSocket").transform;
            rightHandSocket.SetParent(go.transform, false);
            rightHandSocket.localPosition = Vector3.right * 0.1f;
            */
            // tool.leftHandIKTarget = leftHandSocket;
            // tool.rightHandIKTarget = rightHandSocket;
            tool.ikAimRightArm = true;

            var fpModel = go.EnsureComponent<FPModel>();
            fpModel.propModel = worldModel;
            fpModel.viewModel = viewModel;
        };
        infectionTrackerPrefab.SetGameObject(infectionTrackerTemplate);
        infectionTrackerPrefab.SetEquipment(EquipmentType.Hand);
        infectionTrackerPrefab.SetSpawns(new SpawnLocation(new Vector3(-52.575f, 312.560f, -68.644f),
            new Vector3(4.3f, 0.68f, 18f)));
        infectionTrackerPrefab.Info.WithIcon(Plugin.AssetBundle.LoadAsset<Sprite>("InfectionTrackerIcon"));
        infectionTrackerPrefab.Register();

        var infectedReaperSkeleton = MakeInfectedClone(PrefabInfo.WithTechType("InfectedReaperSkeleton"), "8fe779a5-e907-4e9e-b748-1eee25589b34");
        infectedReaperSkeleton.SetSpawns(new LootDistributionData.BiomeData
            { biome = BiomeType.Dunes_SandDune, count = 1, probability = 0.02f }
        );
        infectedReaperSkeleton.Register();
        
        var infectedGenericSkeleton1 = MakeInfectedClone(PrefabInfo.WithTechType("InfectedSkeleton1"), "0b6ea118-1c0b-4039-afdb-2d9b26401ad2");
        infectedGenericSkeleton1.SetSpawns(new LootDistributionData.BiomeData
            { biome = BiomeType.Dunes_SandDune, count = 1, probability = 0.02f }
        );
        infectedGenericSkeleton1.Register();

        var infectedGenericSkeleton2 = MakeInfectedClone(PrefabInfo.WithTechType("InfectedSkeleton2"), "e10ff9a1-5f1e-4c4d-bf5f-170dba9e321b");
        infectedGenericSkeleton2.SetSpawns(new LootDistributionData.BiomeData
            { biome = BiomeType.Dunes_Rock, count = 1, probability = 0.02f }
        );
        infectedGenericSkeleton2.Register();

        var plagueHeart = new CustomPrefab(PlagueHeart);
        plagueHeart.SetGameObject(GetPlagueHeartPrefab);
        plagueHeart.Register();
    }

    private static CustomPrefab MakeInfectedClone(PrefabInfo info, string cloneClassID)
    {
        var prefab = new CustomPrefab(info);
        var template = new CloneTemplate(prefab.Info, cloneClassID);
        template.ModifyPrefab += go => { go.AddComponent<InfectAnything>(); };
        prefab.SetGameObject(template);
        return prefab;
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
        yield return null;
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

    private static IEnumerator GetPlagueHeartPrefab(IOut<GameObject> prefab)
    {
        var obj = Object.Instantiate(Plugin.AssetBundle.LoadAsset<GameObject>("PlagueHeartPrefab"));
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
        obj.EnsureComponent<WorldForces>();
        var rb = obj.EnsureComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.mass = 300;
        rb.useGravity = false;
        obj.AddComponent<FreezeRigidbodyWhenFar>();
        obj.AddComponent<Pickupable>();
        yield return null;
        prefab.Set(obj);
    }
    
    private static IEnumerator GetInfectionLaserPrefab(IOut<GameObject> prefab)
    {
        var solarPanelRequest = CraftData.GetPrefabForTechTypeAsync(TechType.SolarPanel);
        yield return solarPanelRequest;
        var solarPanelPrefab = solarPanelRequest.GetResult();
        var obj = Object.Instantiate(solarPanelPrefab.GetComponent<PowerFX>()).vfxPrefab;
        PrefabUtils.AddBasicComponents(obj, InfectionLaserInfo.ClassID, InfectionLaserInfo.TechType,
            LargeWorldEntity.CellLevel.Global);
        var line = obj.GetComponent<LineRenderer>();
        var newMaterial = new Material(line.material);
        newMaterial.color = new Color(5, 0.474790f, 0.977941f);
        line.material = newMaterial;
        line.widthMultiplier = 15;
        line.endWidth = 100;
        line.SetPositions(new[] {new Vector3(-78.393f, 341.175f, -57.684f), new Vector3(0, 2000, 0)});
        prefab.Set(obj);
    }
}