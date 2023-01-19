namespace ResourceBonanza;

[BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
public class Plugin : BaseUnityPlugin
{
    internal static ManualLogSource log;

    private void Awake()
    {
        log = Logger;
        var harmony = new Harmony(PluginInfo.GUID);
        harmony.PatchAll();
    }
}
