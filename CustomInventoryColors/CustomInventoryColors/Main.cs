using HarmonyLib;
using System.Reflection;
using InventoryColorCustomization;
using SMLHelper.Handlers;
using System.IO;
using BepInEx;
using BepInEx.Logging;

namespace InventoryColorCustomization
{
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    [BepInDependency("com.snmodding.smlhelper")]
    public class Main : BaseUnityPlugin
    {
        internal static Assembly assembly = Assembly.GetExecutingAssembly();
        internal static string assetFolderPath = Path.Combine(Path.GetDirectoryName(assembly.Location), "Assets");
        internal static Options modConfig;

        public static ManualLogSource logger;

        private void Awake()
        {
            logger = Logger;

            logger.LogInfo($"Patching {PluginInfo.Name} ({PluginInfo.GUID}) v{PluginInfo.Version}");
            CustomColorChoiceManager.LoadCustomFiles(); // must be loaded before the color choices are created
            ColorChoiceManager.Initialize();
            modConfig = new Options(); // must be initialized after the color choices are finalized
            OptionsPanelHandler.RegisterModOptions(modConfig);
            var harmony = new Harmony(PluginInfo.GUID);
            harmony.PatchAll(assembly);
            modConfig.InitOptionItems();
            logger.LogInfo($"Finished patching successfully!");
        }

        internal static string GetPathInAssetsFolder(string pathRelativeToAssetsFolder)
        {
            return Path.Combine(assetFolderPath, pathRelativeToAssetsFolder);
        }
    }
}