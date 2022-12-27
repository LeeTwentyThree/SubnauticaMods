using UnityEngine;
using System.Reflection;
using System.IO;

namespace DebugHelper
{
    public static class Helpers
    {
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

        public static string FormatVector3Constructor(Vector3 vector, int decimalPoints)
        {
            return $"new Vector3({FormatFloat(vector.x, decimalPoints)}, {FormatFloat(vector.y, decimalPoints)}, {FormatFloat(vector.z, decimalPoints)})";
        }

        public static string FormatFloat(float num, int decimalPoints)
        {
            return num.ToString($"F{decimalPoints}") + "f";
        }
    }
}
