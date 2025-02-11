using System.Collections.Generic;
using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using Nautilus.Handlers;

namespace GetCreatureIdsCommand;

[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
[BepInDependency("com.snmodding.nautilus")]
[BepInDependency("com.lee23.ecclibrary")]
public class Plugin : BaseUnityPlugin
{
    public new static ManualLogSource Logger { get; private set; }
    
    private static Assembly Assembly { get; } = Assembly.GetExecutingAssembly();

    private void Awake()
    {
        // set project-scoped logger instance
        Logger = base.Logger;
        
        // Initialize custom prefabs
        ConsoleCommandsHandler.RegisterConsoleCommand("getcreatureids", GetCreatureIDs);

        // register harmony patches, if there are any
        Harmony.CreateAndPatchAll(Assembly, $"{PluginInfo.PLUGIN_GUID}");
        Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
    }
    
    private static void GetCreatureIDs()
    {
       var eccAssembly = Assembly.GetAssembly(typeof(ECCLibrary.ECCConfig));
       if (eccAssembly == null)
       {
           ErrorMessage.AddMessage("ECCLibrary assembly not found!");
           return;
       }
       var sanityCheckingType = eccAssembly.GetType("ECCLibrary.SanityChecking");
       if (sanityCheckingType == null)
       {
           ErrorMessage.AddMessage("ECCLibrary types not found!");
           return;
       }

       var creatureTechTypesField = sanityCheckingType.GetField("RegisteredTechTypes", BindingFlags.NonPublic | BindingFlags.Static);
       if (creatureTechTypesField == null)
       {
           ErrorMessage.AddMessage("RegisteredTechTypes HashSet not found!");
           return;
       }

       var creatureTechTypes = (HashSet<TechType>)creatureTechTypesField.GetValue(null);;
       const string infoMessage = "Outputting all known modded CreatureTechTypes to log...";
       
       ErrorMessage.AddMessage(infoMessage);
       Logger.LogInfo(infoMessage);
       foreach (var type in creatureTechTypes)
       {
           var formatted = $"{type.ToString()} ({Language.main.Get(type)})";
           ErrorMessage.AddMessage(formatted);
           Logger.LogInfo(formatted);
       }
    }
}