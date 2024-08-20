using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using BloopAndBlaza.Creatures;
using HarmonyLib;
using Nautilus.Assets;
using Nautilus.Handlers;
using Nautilus.Utility;
using UnityEngine;

namespace BloopAndBlaza;

[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
[BepInDependency("com.snmodding.nautilus")]
[BepInDependency("com.lee23.ecclibrary")]
public class Plugin : BaseUnityPlugin
{
    public new static ManualLogSource Logger { get; private set; }

    private static Assembly Assembly { get; } = Assembly.GetExecutingAssembly();

    public static AssetBundle AssetBundle { get; } =
        AssetBundleLoadingUtils.LoadFromAssetsFolder(Assembly, "bloopblaza");
    
    public static ConfigEntry<bool> registerBlazaSpawns;
    public static ConfigEntry<bool> registerShallowBloopSpawns;
    public static ConfigEntry<bool> registerDeepBloopSpawns;

    private void Awake()
    {
        // set project-scoped logger instance
        Logger = base.Logger;
        
        InitializeConfig();

        // Initialize custom prefabs
        InitializePrefabs();

        LanguageHandler.RegisterLocalizationFolder();

        // register harmony patches, if there are any
        Harmony.CreateAndPatchAll(Assembly, $"{PluginInfo.PLUGIN_GUID}");
        Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");

        ModAudio.RegisterAllAudio();
    }

    private void InitializeConfig()
    {
        registerBlazaSpawns = Config.Bind(
            "Spawns",
            "Enable blaza spawns",
            true,
            "Enable blaza spawns");

        registerShallowBloopSpawns = Config.Bind(
            "Spawns",
            "Enable shallow bloop spawns",
            true,
            "Enable shallow bloop spawns");

        registerDeepBloopSpawns = Config.Bind(
            "Spawns",
            "Enable deep bloop spawns",
            true,
            "Enable deep bloop spawns");
        
        OptionsPanelHandler.RegisterModOptions(new BloopBlazaOptions());
    }

    private void InitializePrefabs()
    {
        var blazaInfo = PrefabInfo.WithTechType("BlazaLeviathan");

        if (registerBlazaSpawns.Value)
        {
            CoordinatedSpawnsHandler.RegisterCoordinatedSpawnsForOneTechType(blazaInfo.TechType, new[]
            {
                new SpawnLocation(new Vector3(-946, -315, -537)),
                new SpawnLocation(new Vector3(-757, -742, -93)),
                new SpawnLocation(new Vector3(-523, -782, 144)),
                new SpawnLocation(new Vector3(-666, -749, 510)),
                new SpawnLocation(new Vector3(-718, -1127, 123)),
                new SpawnLocation(new Vector3(-1144, -200f, 1147)),
                new SpawnLocation(new Vector3(64, -236, -292))
            });
        }

        var blaza = new Blaza(blazaInfo);
        blaza.Register();

        var bloopInfo = PrefabInfo.WithTechType("Bloop");

        var deepBloopInfo = PrefabInfo.WithTechType("DeepBloop");

        if (registerShallowBloopSpawns.Value)
        {
            CoordinatedSpawnsHandler.RegisterCoordinatedSpawnsForOneTechType(bloopInfo.TechType, new[]
            {
                new SpawnLocation(new Vector3(280, -60f, 205)),
                new SpawnLocation(new Vector3(-460, -60f, 540f)),
                new SpawnLocation(new Vector3(-574, -60f, -120)),
                new SpawnLocation(new Vector3(130, -60f, -850)),
            });
        }

        if (registerDeepBloopSpawns.Value)
        {
            CoordinatedSpawnsHandler.RegisterCoordinatedSpawnsForOneTechType(bloopInfo.TechType, new[]
            {
                new SpawnLocation(new Vector3(650, -70, 390))
            });
            CoordinatedSpawnsHandler.RegisterCoordinatedSpawnsForOneTechType(deepBloopInfo.TechType, new[]
            {
                new SpawnLocation(new Vector3(-880, -280, -1340)),
                new SpawnLocation(new Vector3(-1650, -60, -690)),
                new SpawnLocation(new Vector3(1260, -65, -715)),
                new SpawnLocation(new Vector3(-50, -1150, -255)),
                new SpawnLocation(new Vector3(-188, -233, 1540))
            });
        }

        var bloop = new Bloop(bloopInfo, AssetBundle.LoadAsset<GameObject>("Bloop_Prefab"), false);
        bloop.Register();

        var deepBloop = new Bloop(deepBloopInfo, AssetBundle.LoadAsset<GameObject>("DeepBloop_Prefab"), true);
        deepBloop.Register();
    }
}