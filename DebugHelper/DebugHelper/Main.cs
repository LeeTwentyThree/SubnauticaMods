using QModManager.API.ModLoading;
using SMLHelper.V2.Handlers;
using DebugHelper.Commands;
using DebugHelper.Systems;
using UnityEngine;
using System.Reflection;
using HarmonyLib;

namespace DebugHelper
{
    [QModCore()]
    public static class Main
    {
        public static Config config;
        public static Assembly assembly = Assembly.GetExecutingAssembly();
        public static Harmony harmony;

        internal static AssetBundle assetBundle;

        [QModPatch()]
        public static void Patch()
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

            assetBundle = Helpers.LoadAssetBundleFromAssetsFolder(assembly, "debughelper");
            DebugIconManager.Icons.LoadIcons(assetBundle);

            harmony = new Harmony("Subnautica.DebugHelper");
            harmony.PatchAll(assembly);
        }

        [QModPostPatch()]
        public static void PostPatch()
        {
            DB.Setup();
        }
    }
}