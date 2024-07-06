using System;
using System.IO;
using System.Reflection;
using ModStructureFormat;
using Nautilus.Handlers;
using Newtonsoft.Json;

namespace TheRedPlague;

public static class StructureLoading
{
    public static void RegisterStructures()
    {
        Plugin.Logger.LogInfo($"Registering structures...");
        
        var structureFiles = Directory.GetFiles(GetStructuresFolderPath(), "*.structure", SearchOption.AllDirectories);
        var successfulStructures = 0;
        
        foreach (var file in structureFiles)
        {
            if (LoadAndRegisterStructureAtPath(file))
                successfulStructures++;
        }

        Plugin.Logger.LogInfo(
            $"Successfully registered {successfulStructures} structure(s)!");
    }
    
    private static string GetStructuresFolderPath()
    {
        var modFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        var structuresFolder = Path.Combine(modFolder, "Structures");
        if (!Directory.Exists(structuresFolder)) Directory.CreateDirectory(structuresFolder);
        return structuresFolder;
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
        }
    }
}