using Newtonsoft.Json;

namespace ModStructureFormat;

/// <summary>
/// Represents a group of object IDs to be removed from the world.
/// </summary>
[Serializable]
public class AntiStructure
{
    public AntiStructure(List<string> uniqueIds)
    {
        UniqueIds = uniqueIds;
    }
    
    public List<string> UniqueIds { get; }
    
    public static Structure LoadFromFile(string jsonFilePath)
    {
        return JsonConvert.DeserializeObject<Structure>(File.ReadAllText(jsonFilePath));
    }
    
    public void SaveToFile(string jsonFilePath)
    {
        File.WriteAllText(jsonFilePath, JsonConvert.SerializeObject(this));
    }
}