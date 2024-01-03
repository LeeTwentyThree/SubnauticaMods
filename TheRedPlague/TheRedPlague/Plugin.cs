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

    private static PrefabInfo InfectionDomeInfo { get; } = PrefabInfo.WithTechType("InfectionDome"); 
    private static PrefabInfo InfectionLaserInfo { get; } = PrefabInfo.WithTechType("InfectionLaser"); 

    private void Awake()
    {
        AssetBundle = AssetBundleLoadingUtils.LoadFromAssetsFolder(Assembly, "theredplague");

        ZombieInfectionTexture = AssetBundle.LoadAsset<Texture2D>("zombie_infection_bloody");
            
        // set project-scoped logger instance
        Logger = base.Logger;
        
        // register harmony patches, if there are any
        Harmony.CreateAndPatchAll(Assembly, $"{PluginInfo.PLUGIN_GUID}");
        Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");

        // Add new biome
        var infectedZoneSettings = BiomeUtils.CreateBiomeSettings(new Vector3(10, 4.5f, 3f), 0.4f,
            new Color(1.05f, 1f, 1f, 1), 2f, new Color(1f, 0.3f, 0.3f), 0f, 10f, 0.5f, 0.9f, 20f);
        BiomeHandler.RegisterBiome("infectedzone", infectedZoneSettings, new BiomeHandler.SkyReference("SkyGrassyPlateaus"));
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
            DestroyImmediate(go.GetComponent<DisableEmissiveOnStoryGoal>());
        };
        infectionCubePlatform.SetGameObject(infectionCubeTemplate);
        infectionCubePlatform.SetSpawns(new SpawnLocation(new Vector3(-54.508f, -3.523f, -42.000f), new Vector3(340.496f, 30.000f, 270.000f), Vector3.one * 0.4f),
            new SpawnLocation(new Vector3(-60.932f, 308.929f, -52.616f), new Vector3(0, 103, 0), new Vector3(0.4f, 0.2f, 0.4f)),
            new SpawnLocation(new Vector3(2.306f, 341.463f, -25.395f), new Vector3(90, 68.57f, 0), new Vector3(0.9f, 0.9f, 0.9f)),
            new SpawnLocation(new Vector3(-55.196f, 386.394f, -1.448f), Vector3.zero, Vector3.one),
            new SpawnLocation(new Vector3(-71.367f, 314.375f, -92.013f), new Vector3(15.802f, 359.521f, 356.553f), Vector3.one * 0.7f));
        infectionCubePlatform.Register();
        
        var lowQualityForceFieldIsland = new CustomPrefab(PrefabInfo.WithTechType("ForceFieldIslandLowQuality"));
        lowQualityForceFieldIsland.SetGameObject(GetLowQualityForceFieldIslandPrefab);
        lowQualityForceFieldIsland.SetSpawns(new SpawnLocation(new Vector3(-128, 160, -128), new Vector3(0, 180, 0), new Vector3(1, 1, -1)));
        lowQualityForceFieldIsland.Register();
        
        var forceFieldIslandLightPrefab = new CustomPrefab(PrefabInfo.WithTechType("ForceFieldIslandLight"));
        var forceFieldIslandLightTemplate = new CloneTemplate(forceFieldIslandLightPrefab.Info, "081ef6c1-aa78-46fd-a20f-a6b63ca5c5f3");
        forceFieldIslandLightTemplate.ModifyPrefab += (go) =>
        {
            go.GetComponents<DisableEmissiveOnStoryGoal>().ForEach(c => c.enabled = false);
            go.GetComponents<LightIntensityOnStoryGoal>().ForEach(c => c.enabled = false);
        };
        forceFieldIslandLightPrefab.SetGameObject(forceFieldIslandLightTemplate);
        forceFieldIslandLightPrefab.Register();

        var infectionLaserColumnPrefab = new CustomPrefab(PrefabInfo.WithTechType("InfectionLaserColumn"));
        var infectionLaserColumnTemplate = new CloneTemplate(infectionLaserColumnPrefab.Info, "3d625dbb-d15a-4351-bca0-0a0526f01e6e");
        infectionLaserColumnTemplate.ModifyPrefab += go =>
        {
            go.GetComponents<DisableEmissiveOnStoryGoal>().ForEach(c => c.enabled = false);
            go.GetComponents<LightIntensityOnStoryGoal>().ForEach(c => c.enabled = false);
            go.GetComponents<PrecursorGunCentralColumn>().ForEach(c => c.enabled = false);
            go.transform.Find("precursor_column_maze_08_06_08_hlpr/precursor_column_maze_08_06_08_ctrl/precursor_column_maze_08_06_08/light").GetComponent<Light>().color = Color.red;
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
        infectionLaserColumnPrefab.SetSpawns(new SpawnLocation(new Vector3(-59.640f, 301.000f, -24.840f), Vector3.up * 343, Vector3.one * 0.7f));
        infectionLaserColumnPrefab.Register();
        
        var infectionLaserTerminalPrefab = new CustomPrefab(PrefabInfo.WithTechType("InfectionLaserTerminal"));
        var infectionLaserTerminalTemplate = new CloneTemplate(infectionLaserTerminalPrefab.Info, "b1f54987-4652-4f62-a983-4bf3029f8c5b");
        infectionLaserTerminalTemplate.ModifyPrefab += go =>
        {
            // implement custom modifications here
        };
        infectionLaserTerminalPrefab.SetGameObject(infectionLaserTerminalTemplate);
        infectionLaserTerminalPrefab.SetSpawns(new SpawnLocation(new Vector3(-60.053f, 301.044f, -23.278f), Vector3.up * 163));
        infectionLaserTerminalPrefab.Register();
        
        var infectionControlRoomPrefab = new CustomPrefab(PrefabInfo.WithTechType("InfectionControlRoom"));
        var infectionControlRoomTemplate = new CloneTemplate(infectionControlRoomPrefab.Info, "963fa3a3-9192-4912-8c8d-d0d98f22ed13");
        infectionControlRoomTemplate.ModifyPrefab += go =>
        {
            go.transform.GetChild(0).gameObject.SetActive(false);
            go.transform.GetChild(3).gameObject.SetActive(false);
            var colliders = go.transform.Find("Control_Room_Collision/Cube").GetComponents<Collider>();
            var colliderIndicesToDisable = new int[] { 11, 12, 19, 49 };
            foreach (var index in colliderIndicesToDisable)
            {
                colliders[index].enabled = false;
            }

            var modelParent = go.transform.Find("Precursor_Gun_ControlRoom");
            var modelIndicesToDisable = new int[] { 66 ,67, 69, 70, 78, 100, 110, 111, 112, 113 };
            foreach (var index in modelIndicesToDisable)
            {
                modelParent.GetChild(index).gameObject.SetActive(false);
            }
        };
        infectionControlRoomPrefab.SetGameObject(infectionControlRoomTemplate);
        infectionControlRoomPrefab.SetSpawns(new SpawnLocation(new Vector3(-65.050f, 302.850f, -20.260f), Vector3.up * 343, Vector3.one * 0.42f));
        infectionControlRoomPrefab.Register();
        
        var infectionLaserDevicePrefab = new CustomPrefab(PrefabInfo.WithTechType("InfectionLaserDevice"));
        var infectionLaserDeviceTemplate = new CloneTemplate(infectionLaserTerminalPrefab.Info, "22fb9ee9-690d-426c-844f-a80e527b5fe6");
        infectionLaserDeviceTemplate.ModifyPrefab += go =>
        {
            go.GetComponent<LargeWorldEntity>().cellLevel = LargeWorldEntity.CellLevel.Global;
            go.GetComponents<DisableEmissiveOnStoryGoal>().ForEach(c => c.enabled = false);
            go.GetComponents<LightIntensityOnStoryGoal>().ForEach(c => c.enabled = false);
            DestroyImmediate(go.GetComponent<PrecursorGunStoryEvents>());
            DestroyImmediate(go.GetComponent<PrecursorGunAim>());
            DestroyImmediate(go.GetComponent<AnimateOnStoryGoal>());
            var modelParent = go.transform.Find("precursor_base/Instances");
            modelParent.Find("precursor_base_22").gameObject.SetActive(false);
            modelParent.Find("precursor_base_23").gameObject.SetActive(false);
            modelParent.Find("precursor_base_24").gameObject.SetActive(false);
            modelParent.Find("precursor_base_25").gameObject.SetActive(false);
        };
        infectionLaserDevicePrefab.SetGameObject(infectionLaserDeviceTemplate);
        infectionLaserDevicePrefab.SetSpawns(new SpawnLocation(new Vector3(-80.000f,  304, -47.790f), new Vector3(0, 0, 353), Vector3.one * 0.3f));
        infectionLaserDevicePrefab.Register();

        var infectionLaserPrefab = new CustomPrefab(InfectionLaserInfo);
        infectionLaserPrefab.SetGameObject(GetInfectionLaserPrefab);
        infectionLaserPrefab.SetSpawns(new SpawnLocation(Vector3.zero));
        infectionLaserPrefab.Register();
        
        IslandProps.AddIslandPropSpawns();
        
        ConsoleCommandsHandler.AddGotoTeleportPosition("forcefieldisland", new Vector3(-78.1f, 315.5f, -68.7f));
    }

    private static IEnumerator GetInfectionDomePrefab(IOut<GameObject> prefab)
    {
        var obj = Instantiate(AssetBundle.LoadAsset<GameObject>("InfectionDome"));
        MaterialUtils.ApplySNShaders(obj);
        PrefabUtils.AddBasicComponents(obj, InfectionDomeInfo.ClassID, InfectionDomeInfo.TechType, LargeWorldEntity.CellLevel.Global);
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
        var obj = Instantiate(AssetBundle.LoadAsset<GameObject>("ForcefieldIslandLowQuality"));
        MaterialUtils.ApplySNShaders(obj);
        PrefabUtils.AddBasicComponents(obj, InfectionDomeInfo.ClassID, InfectionDomeInfo.TechType, LargeWorldEntity.CellLevel.Global);
        obj.AddComponent<LowQualityIslandMesh>();
        // var renderer = obj.GetComponentInChildren<Renderer>();
        yield return null;
        prefab.Set(obj);
    }
    
    private static IEnumerator GetInfectionLaserPrefab(IOut<GameObject> prefab)
    {
        var solarPanelRequest = CraftData.GetPrefabForTechTypeAsync(TechType.SolarPanel);
        yield return solarPanelRequest;
        var solarPanelPrefab = solarPanelRequest.GetResult();
        var obj = Instantiate(solarPanelPrefab).GetComponent<PowerFX>().vfxPrefab;
        PrefabUtils.AddBasicComponents(obj, InfectionLaserInfo.ClassID, InfectionLaserInfo.TechType, LargeWorldEntity.CellLevel.Global);
        var line = obj.GetComponent<LineRenderer>();
        line.material.color = new Color(5, 0.474790f, 0.977941f);
        line.widthMultiplier = 15;
        line.endWidth = 100;
        line.SetPositions(new []{new Vector3(-78.393f,341.175f, -57.684f), new Vector3(0, 2000, 0)});
        prefab.Set(obj);
    }
}