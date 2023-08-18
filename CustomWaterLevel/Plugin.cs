using BepInEx;
using BepInEx.Logging;
using CustomWaterLevel;
using HarmonyLib;
using Nautilus.Handlers;
using System.Collections.Generic;
using System.Reflection;

namespace CustomWaterLevel;
[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
[BepInDependency("com.snmodding.nautilus")]
public class Plugin : BaseUnityPlugin
{
    public new static ManualLogSource Logger { get; private set; }

    private static Assembly Assembly { get; } = Assembly.GetExecutingAssembly();

    public static WaterLevelConfig config = OptionsPanelHandler.RegisterModOptions<WaterLevelConfig>();

    public static float DefaultFogDistance = 0f;

    public static float UndergroundFogDistance = 0.6f;

    public static float DefaultColorDecay = 0.1f;

    public static float UndergroundColorDecay = 2f;

    public static float CaveDepth = -320;

    private void Awake()
    {
        // set project-scoped logger instance
        Logger = base.Logger;

        // register harmony patches, if there are any
        Harmony.CreateAndPatchAll(Assembly, $"{PluginInfo.PLUGIN_GUID}");
        Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
    }

    public static float WaterLevel
    {
        get
        {
            var wm = WaterMove.main;
            if (wm != null && wm.InUse)
            {
                return wm.waterLevel;
            }
            return config.WaterLevel;
        }
    }

    public static readonly List<TechType> NonSuffocatingCreatures = new List<TechType>()
    {
        TechType.RockPuncher,
        TechType.Skyray,
        TechType.Rockgrub,
        TechType.CaveCrawler,
        TechType.Shuttlebug,
        TechType.Jumper,
        TechType.CrabSquid,
        TechType.Sandshark,
        TechType.Cutefish,
        TechType.RabbitRay,
        TechType.LavaLarva
    };


    public static float PlayerY
    {
        get
        {
            if (MainCamera.camera == null)
            {
                return 0f;
            }
            return MainCamera.camera.transform.position.y;
        }
    }

    public static bool PlayerWalkingInCave
    {
        get
        {
            if (PlayerY < WaterLevel)
            {
                return false;
            }
            return PlayerY < CaveDepth;
        }
    }
}