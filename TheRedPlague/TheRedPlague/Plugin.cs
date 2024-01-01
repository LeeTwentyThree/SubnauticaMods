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

    private static PrefabInfo InfectedZoneInfo { get; } = PrefabInfo.WithTechType("InfectionDome"); 

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

        var infectionDome = new CustomPrefab(InfectedZoneInfo);
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
                    renderer.material.SetColor();
                }
                else if (renderer.gameObject.name.Contains("Tube"))
                {
                    var materials = renderer.materials;
                    materials[0].color = new Color(5, 2, 0.27f);
                    materials[1].color = new Color(2, 0.38f, 0.43f);
                    renderer.materials = materials;
                }
            }

            go.transform.Find("Point light").GetComponent<Light>().color = Color.red;
        };
        islandElevator.SetGameObject(elevatorTemplate);
        islandElevator.SetSpawns(new SpawnLocation(new Vector3(-55.000f, 50.000f, -41.000f), Vector3.zero,
            new Vector3(1, 3.7f, 1)));
        islandElevator.Register();
        
        IslandProps.AddIslandPropSpawns();
    }

    private static IEnumerator GetInfectionDomePrefab(IOut<GameObject> prefab)
    {
        var obj = Instantiate(AssetBundle.LoadAsset<GameObject>("InfectionDome"));
        MaterialUtils.ApplySNShaders(obj);
        PrefabUtils.AddBasicComponents(obj, InfectedZoneInfo.ClassID, InfectedZoneInfo.TechType, LargeWorldEntity.CellLevel.Global);
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
}