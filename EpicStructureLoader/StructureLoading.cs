using System;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using ModStructureFormat;
using Nautilus.Handlers;

namespace EpicStructureLoader;

public static class StructureLoading
{
    private static int _registeredEntities;

    private static string GetStructuresFolder()
    {
        var modFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        var structuresFolder = Path.Combine(modFolder, "Structures");
        if (!Directory.Exists(structuresFolder))
            Directory.CreateDirectory(structuresFolder);
        return structuresFolder;
    }
    
    public static void RegisterStructures()
    {
        Plugin.Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is registering structures...");
        
        var structureFiles = Directory.GetFiles(GetStructuresFolder(), "*.structure", SearchOption.AllDirectories);
        var successfulStructures = 0;

        _registeredEntities = 0;

        foreach (var file in structureFiles)
        {
            if (LoadAndRegisterStructureAtPath(file))
                successfulStructures++;
        }

        Plugin.Logger.LogInfo(
            $"Plugin {PluginInfo.PLUGIN_GUID} has successfully registered {successfulStructures} structure(s) with a total of {_registeredEntities} entities!");
    }

    private static bool LoadAndRegisterStructureAtPath(string structurePath)
    {
        try
        {
            var text = File.ReadAllText(structurePath);
            var structure = JsonConvert.DeserializeObject<Structure>(text);
            RegisterStructure(structure);
            return true;
        }
        catch (Exception e)
        {
            Plugin.Logger.LogError(e.ToString());
            return false;
        }
    }

    private static void RegisterStructure(Structure structure)
    {
        structure.SortByPriority();

        foreach (var entity in structure.Entities)
        {
            CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(new SpawnInfo(entity.classId, entity.position.ToVector3(),
                entity.rotation.ToQuaternion(), entity.scale.ToVector3(),
                obj => obj.GetComponent<UniqueIdentifier>().Id = entity.id));
            _registeredEntities++;
        }
    }
}