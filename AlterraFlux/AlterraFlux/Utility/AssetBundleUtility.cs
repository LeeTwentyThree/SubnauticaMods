using System.Reflection;

namespace AlterraFlux.Utility;

public static class AssetBundleUtility
{
    public static AssetBundle LoadAssetBundleFromAssetsFolder(Assembly modAssembly, string assetsFileName)
    {
        return AssetBundle.LoadFromFile(Path.Combine(Path.GetDirectoryName(modAssembly.Location), "Assets", assetsFileName));
    }
}
