using System.Reflection;
using TMPro;

namespace CreatureMorphs;
internal class Helpers
{
    public static Transform CameraTransform => MainCamera.camera.transform;

    public static AssetBundle LoadAssetBundleFromAssetsFolder(Assembly modAssembly, string assetsFileName)
    {
        return AssetBundle.LoadFromFile(Path.Combine(Path.GetDirectoryName(modAssembly.Location), "Assets", assetsFileName));
    }

    public static FMODAsset GetFmodAsset(string audioPath)
    {
        FMODAsset asset = ScriptableObject.CreateInstance<FMODAsset>();
        asset.path = audioPath;
        return asset;
    }

    public static void FixText(TextMeshProUGUI text, FontManager.FontType font = FontManager.FontType.Aller_Rg)
    {
        text.font = FontManager.GetFontAsset(font);
    }

    public static void FixFonts(GameObject obj)
    {
        var texts = obj.GetComponentsInChildren<TextMeshProUGUI>(true);
        foreach (var text in texts)
            FixText(text);
    }
}