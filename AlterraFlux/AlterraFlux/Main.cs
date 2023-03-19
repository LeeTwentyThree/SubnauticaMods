using System.Reflection;

namespace AlterraFlux;

[BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
[BepInDependency("com.snmodding.smlhelper")]
public class Main : BaseUnityPlugin
{
    public static Assembly assembly = Assembly.GetExecutingAssembly();
    public static AssetBundle assetBundle;

    private void Awake()
    {
        gameObject.EnsureComponent<SceneCleanerPreserve>();

        assetBundle = AssetBundleUtility.LoadAssetBundleFromAssetsFolder(assembly, "alterraflux");
        PrefabPatcher.Register();
    }
}
