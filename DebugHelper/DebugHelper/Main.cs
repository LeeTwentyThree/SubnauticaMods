using SMLHelper.V2.Handlers;
using DebugHelper.Commands;
using DebugHelper.Systems;
using UnityEngine;
using System.Reflection;
using HarmonyLib;
using BepInEx;

namespace DebugHelper
{
    [BepInPlugin("DebugHelper", "Debug Helper", "1.0.0")]
    public class Main : BaseUnityPlugin
    {
        public static Config config;
        public static Assembly assembly = Assembly.GetExecutingAssembly();
        public static Harmony harmony;

        internal static AssetBundle assetBundle;

        private void Awake()
        {
            config = OptionsPanelHandler.RegisterModOptions<Config>();

            ConsoleCommandsHandler.Main.RegisterConsoleCommands(typeof(PrefabCommands));
            ConsoleCommandsHandler.Main.RegisterConsoleCommands(typeof(AudioCommands));
            ConsoleCommandsHandler.Main.RegisterConsoleCommands(typeof(ColliderCommands));
            ConsoleCommandsHandler.Main.RegisterConsoleCommands(typeof(LightCommands));
            ConsoleCommandsHandler.Main.RegisterConsoleCommands(typeof(SkyApplierCommands));
            ConsoleCommandsHandler.Main.RegisterConsoleCommands(typeof(GeneralCommands));
            ConsoleCommandsHandler.Main.RegisterConsoleCommands(typeof(CreatureCommands));
            ConsoleCommandsHandler.Main.RegisterConsoleCommands(typeof(LiveMixinCommands));
            ConsoleCommandsHandler.Main.RegisterConsoleCommands(typeof(RigidbodyCommands));
            ConsoleCommandsHandler.Main.RegisterConsoleCommands(typeof(PlayerCommands));
            ConsoleCommandsHandler.Main.RegisterConsoleCommands(typeof(EntgalCommands));

            assetBundle = Helpers.LoadAssetBundleFromAssetsFolder(assembly, "debughelper");
            DebugIconManager.Icons.LoadIcons(assetBundle);

            harmony = new Harmony("Subnautica.DebugHelper");
            harmony.PatchAll(assembly);

            gameObject.EnsureComponent<SceneCleanerPreserve>();

            DB.Setup();
        }
    }
}