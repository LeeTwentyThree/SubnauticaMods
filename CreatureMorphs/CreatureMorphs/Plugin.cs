using System.Reflection;

namespace CreatureMorphs;

[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
internal class Plugin : BaseUnityPlugin
{
    public static AssetBundle bundle;

    public static ManualLogSource logger;

    new public static Config Config = OptionsPanelHandler.RegisterModOptions<Config>();

    private void Awake()
    {
        logger = Logger;

        Assembly assembly = Assembly.GetExecutingAssembly();

        bundle = AssetBundleLoadingUtils.LoadFromAssetsFolder(assembly, "creaturemorphs");

        ModAudio.PatchAudio();
        MorphModeData.Setup();
        MorphDatabase.Setup();

        new Harmony(PluginInfo.PLUGIN_GUID).PatchAll(assembly);
    }
}
