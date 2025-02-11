using System;
using BepInEx;
using BepInEx.Logging;
using Nautilus.Handlers;

namespace GenerateBasePrefabCommand;

[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
[BepInDependency("com.snmodding.nautilus")]
public class Plugin : BaseUnityPlugin
{
    public new static ManualLogSource Logger { get; private set; }
    
    private void Awake()
    {
        // set project-scoped logger instance
        Logger = base.Logger;

        ConsoleCommandsHandler.RegisterConsoleCommand("generatebaseprefabcode", GenerateBasePrefabCodeCommand);

        Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
    }

    private static void GenerateBasePrefabCodeCommand()
    {
        try
        {
            GenerateBasePrefabCode.ConvertBaseToPrefabConstructionCode(Player.main.GetCurrentSub()
                .GetComponent<Base>());
        }
        catch (Exception e)
        {
            ErrorMessage.AddMessage("Process failed: <color=#FF0000>" + e + "</color>");
            ErrorMessage.AddMessage("Make sure you are inside of a base made with the habitat builder.");
            Logger.LogError("Failed to generate base prefab code: " + e);
        }
    }
}