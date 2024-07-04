using System;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using ModStructureFormat;
using Nautilus.Handlers;

namespace EpicStructureLoader;

public static class StructureLoading
{
    private static string GetStructuresFolder()
    {
        var modFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        var structuresFolder = Path.Combine(modFolder, "Structures");
        if (!Directory.Exists(structuresFolder))
            Directory.CreateDirectory(structuresFolder);
        return structuresFolder;
    }
    
    internal static void RegisterStructures()
    {
        Plugin.Logger.LogDebug($"Plugin {PluginInfo.PLUGIN_GUID} is registering structures...");
        
        var structureFiles = Directory.GetFiles(GetStructuresFolder(), "*.structure", SearchOption.AllDirectories);
        var successfulStructures = 0;

        int registeredEntities = 0;

        foreach (var file in structureFiles)
        {
            if (LoadAndRegisterStructureAtPath(file, ref registeredEntities))
                successfulStructures++;
        }

        Plugin.Logger.LogInfo(
            $"Plugin {PluginInfo.PLUGIN_GUID} has successfully registered {successfulStructures} structure(s) with a total of {registeredEntities} entities!");
    }

    public static bool LoadAndRegisterStructureAtPath(string structurePath, ref int registeredEntities)
    {
        try
        {
            var text = File.ReadAllText(structurePath);
            var structure = JsonConvert.DeserializeObject<Structure>(text);
            RegisterStructure(structure, ref registeredEntities);
            return true;
        }
        catch (Exception e)
        {
            Plugin.Logger.LogError(e.ToString());
            return false;
        }
    }

    public static void RegisterStructure(Structure structure, ref int registeredEntities)
    {
        structure.SortByPriority();

        foreach (var entity in structure.Entities)
        {
            CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(new SpawnInfo(entity.classId, entity.position.ToVector3(),
                entity.rotation.ToQuaternion(), entity.scale.ToVector3(),
                obj => obj.GetComponent<UniqueIdentifier>().Id = entity.id));
            registeredEntities++;
        }
    }
}