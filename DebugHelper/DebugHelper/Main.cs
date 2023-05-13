using Nautilus.Handlers;
using DebugHelper.Commands;
using DebugHelper.Systems;
using UnityEngine;
using System.Reflection;
using HarmonyLib;
using BepInEx;

namespace DebugHelper
{
    [BepInPlugin("DebugHelper", "Debug Helper", "1.1.0")]
    [BepInDependency("com.snmodding.nautilus")]
    public class Main : BaseUnityPlugin
    {
        public static Config config;
        public static Assembly assembly = Assembly.GetExecutingAssembly();
        public static Harmony harmony;

        internal static AssetBundle assetBundle;

        private void Awake()
        {
            config = OptionsPanelHandler.RegisterModOptions<Config>();

            ConsoleCommandsHandler.RegisterConsoleCommands(typeof(PrefabCommands));
            ConsoleCommandsHandler.RegisterConsoleCommands(typeof(AudioCommands));
            ConsoleCommandsHandler.RegisterConsoleCommands(typeof(ColliderCommands));
            ConsoleCommandsHandler.RegisterConsoleCommands(typeof(LightCommands));
            ConsoleCommandsHandler.RegisterConsoleCommands(typeof(SkyApplierCommands));
            ConsoleCommandsHandler.RegisterConsoleCommands(typeof(GeneralCommands));
            ConsoleCommandsHandler.RegisterConsoleCommands(typeof(CreatureCommands));
            ConsoleCommandsHandler.RegisterConsoleCommands(typeof(LiveMixinCommands));
            ConsoleCommandsHandler.RegisterConsoleCommands(typeof(RigidbodyCommands));
            ConsoleCommandsHandler.RegisterConsoleCommands(typeof(PlayerCommands));
            ConsoleCommandsHandler.RegisterConsoleCommands(typeof(EntgalCommands));

            assetBundle = Helpers.LoadAssetBundleFromAssetsFolder(assembly, "debughelper");
            DebugIconManager.Icons.LoadIcons(assetBundle);

            harmony = new Harmony("Subnautica.DebugHelper");
            harmony.PatchAll(assembly);

            gameObject.EnsureComponent<SceneCleanerPreserve>();

            DB.Setup();
        }
    }
}