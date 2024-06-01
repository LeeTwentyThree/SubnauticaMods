using System.Reflection;
using BepInEx;
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
public class Plugin : BaseUnityPlugin
{
    public new static ManualLogSource Logger { get; private set; }

    private static Assembly Assembly { get; } = Assembly.GetExecutingAssembly();

    public static AssetBundle AssetBundle { get; } = AssetBundleLoadingUtils.LoadFromAssetsFolder(Assembly, "bloopblaza");

    private void Awake()
    {
        // set project-scoped logger instance
        Logger = base.Logger;

        // Initialize custom prefabs
        InitializePrefabs();
        
        LanguageHandler.RegisterLocalizationFolder();

        // register harmony patches, if there are any
        Harmony.CreateAndPatchAll(Assembly, $"{PluginInfo.PLUGIN_GUID}");
        Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
        
        ModAudio.RegisterAllAudio();
    }

    private void InitializePrefabs()
    {
        var blazaInfo = PrefabInfo.WithTechType("BlazaLeviathan");
        CoordinatedSpawnsHandler.RegisterCoordinatedSpawnsForOneTechType(blazaInfo.TechType, new []
        {
            new SpawnLocation(new Vector3(-946, -315, -537)),
            new SpawnLocation(new Vector3(-757, -742, -93)),
            new SpawnLocation(new Vector3(-523, -782, 144)),
            new SpawnLocation(new Vector3(-666, -749, 510)),
            new SpawnLocation(new Vector3(-718, -1127, 123)),
            new SpawnLocation(new Vector3(-1144, -200f, 1147)),
            new SpawnLocation(new Vector3(64, -236, -292))
        });
        
        var blaza = new Blaza(blazaInfo);
        blaza.Register();

        var bloopInfo = PrefabInfo.WithTechType("Bloop");
        CoordinatedSpawnsHandler.RegisterCoordinatedSpawnsForOneTechType(bloopInfo.TechType, new []
        {
            new SpawnLocation(new Vector3(280, -60f, 205)),
            new SpawnLocation(new Vector3(-460, -60f, 540f)),
            new SpawnLocation(new Vector3(-574, -60f, -120)),
            new SpawnLocation(new Vector3(130, -60f, -850)),
            new SpawnLocation(new Vector3(650f, -70f, 390f))
        });
        var bloop = new Bloop(bloopInfo);
        bloop.Register();
    }
}