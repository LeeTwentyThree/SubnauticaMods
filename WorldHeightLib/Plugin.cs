using System.Reflection;
using System.IO;
using BepInEx;
using BepInEx.Logging;
using UnityEngine;

namespace WorldHeightLib;

[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
[BepInDependency("com.snmodding.nautilus")]
public class Plugin : BaseUnityPlugin
{
    public new static ManualLogSource Logger { get; private set; }

    private static Assembly Assembly { get; } = Assembly.GetExecutingAssembly();

    public static AssetBundle AssetBundle { get; private set; }

    private void Awake()
    {
        Logger = base.Logger;

        AssetBundle = AssetBundle.LoadFromFile(Path.Combine(Path.GetDirectoryName(Assembly.Location), "Assets", "worldheightlib"));

        HeightMap.Instance = new HeightMap(AssetBundle.LoadAsset<Texture2D>("WorldHeightmap"), 2048, 1000, 4096, 1026);
        HeightMap.Instance.ProcessMapData();
        
        NormalMap.Instance = new NormalMap(AssetBundle.LoadAsset<Texture2D>("WorldNormalMap"), 512, 4096);
        NormalMap.Instance.ProcessMapData();

        Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
    }
}