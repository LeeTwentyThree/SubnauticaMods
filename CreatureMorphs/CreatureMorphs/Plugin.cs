using System.Reflection;
namespace CreatureMorphs;
[BepInPlugin(PluginData.GUID, PluginData.Name, PluginData.Version)]
public class Plugin : BaseUnityPlugin
{
    public static AssetBundle bundle;

    private void Awake()
    {
        Assembly assembly = Assembly.GetExecutingAssembly();

        bundle = AssetBundle.LoadFromFile(Path.Combine(Path.GetDirectoryName(assembly.Location), "Assets", "creaturemorphs"));

        ModAudio.PatchAudio();
        MorphModeData.Setup();
        MorphDatabase.Setup();

        new Harmony("com.lee23.creaturemorphs").PatchAll(assembly);
    }
}
