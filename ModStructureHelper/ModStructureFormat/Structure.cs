using UnityEngine;
using Newtonsoft.Json;

namespace ModStructureFormat;

/// <summary>
/// Represents a group of entities.
/// </summary>
[Serializable]
public class Structure
{
    public Entity[] Entities { get; private set; }
    
    public bool IsSorted { get; private set; }
    
    /// <summary>
    /// Creates a new Entity Group with the given entities.
    /// </summary>
    /// <seealso cref="Structure.LoadFromFile"/>
    public Structure(Entity[] entities) => Entities = entities;
    
    public static Structure LoadFromFile(string jsonFilePath)
    {
        return JsonConvert.DeserializeObject<Structure>(File.ReadAllText(jsonFilePath));
    }
    
    public void SaveToFile(string jsonFilePath)
    {
        File.WriteAllText(jsonFilePath, JsonConvert.SerializeObject(this));
    }

    public void SortByPriority()
    {
        Entities = Entities.OrderBy(entity => entity.priority).ToArray();
        IsSorted = true;
    }
}