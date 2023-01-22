using HarmonyLib;
using System.Reflection;
using InventoryColorCustomization;
using SMLHelper.V2.Handlers;
using System.IO;
using BepInEx;

[BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
[BepInDependency("com.ahk1221.smlhelper")]
public class Main : BaseUnityPlugin
{
    internal static Assembly assembly = Assembly.GetExecutingAssembly();
    internal static string assetFolderPath = Path.Combine(Path.GetDirectoryName(assembly.Location), "Assets");
    internal static Options modConfig;

    private void Awake()
    {
        CustomColorChoiceManager.LoadCustomFiles(); // must be loaded before the color choices are created
        ColorChoiceManager.Initialize();
        modConfig = new Options(); // must be initialized after the color choices are finalized
        OptionsPanelHandler.RegisterModOptions(modConfig);
        var harmony = new Harmony(PluginInfo.GUID);
        harmony.PatchAll(assembly);
    }

    internal static string GetPathInAssetsFolder(string pathRelativeToAssetsFolder)
    {
        return Path.Combine(assetFolderPath, pathRelativeToAssetsFolder);
    }
}