using System.Collections;
using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using UnityEngine;

namespace EpicStructureLoader;

[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
[BepInDependency("com.snmodding.nautilus")]
public class Plugin : BaseUnityPlugin
{
    public new static ManualLogSource Logger { get; private set; }

    private static Assembly Assembly { get; } = Assembly.GetExecutingAssembly();

    private void Awake()
    {
        Logger = base.Logger;
        Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
    }

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(2);

        StructureLoading.RegisterStructures();
    }
}