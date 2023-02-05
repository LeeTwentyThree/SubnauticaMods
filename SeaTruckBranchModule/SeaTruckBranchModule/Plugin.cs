namespace SeaTruckExpansionModule;

using SMLHelper.V2.Handlers;
using System.Reflection;

[BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
public class Plugin : BaseUnityPlugin
{
    internal static BranchModulePrefab branchModulePrefab = new();
    internal static ManualLogSource Log;
    internal static Assembly assembly = Assembly.GetExecutingAssembly();
    internal static AssetBundle assetBundle;


    private void Awake()
    {
        Log = Logger;

        branchModulePrefab.Patch();

        var harmony = new Harmony(PluginInfo.GUID);
        harmony.PatchAll(assembly);

        assetBundle = AssetBundle.LoadFromFile(Path.Combine(assembly.Location, "..", "Assets", "seatruckbranchmodule"));

        LanguageHandler.SetLanguageLine("PilotSeaTruckBranchModule", "Pilot Branch Module");
    }
}
