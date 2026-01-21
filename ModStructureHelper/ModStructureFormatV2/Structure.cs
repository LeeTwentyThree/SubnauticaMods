using System.Runtime.Serialization;
using Newtonsoft.Json;
using UnityEngine;

namespace ModStructureFormatV2;

/// <summary>
/// Represents a group of entities.
/// </summary>
[Serializable]
public class Structure
{
    public Entity[]? Entities { get; private set; }

    public Dictionary<string, string>? Metadata { get; private set; } = new();

    /// <summary>
    /// Creates a new Entity Group with the given entities.
    /// </summary>
    /// <seealso cref="Structure.LoadFromFile"/>
    public Structure(Entity[] entities) => Entities = entities;

    /// <summary>
    /// Creates a new Entity Group with the given entities.
    /// </summary>
    /// <seealso cref="Structure.LoadFromFile"/>
    [JsonConstructor]
    public Structure(Entity[] entities, Dictionary<string, string> metadata)
    {
        Entities = entities;
        Metadata = metadata;
    }
    
    public static Structure LoadFromFile(string jsonFilePath)
    {   
        return JsonConvert.DeserializeObject<Structure>(File.ReadAllText(jsonFilePath));
    }
    
    public void SaveToFile(string jsonFilePath)
    {
        File.WriteAllText(jsonFilePath, JsonConvert.SerializeObject(this, Formatting.Indented));
    }

    public void SaveMetadata<T>(string key, T value)
    {
        Metadata[key] = SerializeVariable(value);
    }

    public bool TryGetMetadata<T>(string key, out T? value)
    {
        value = default;
        if (!Metadata.TryGetValue(key, out var json))
            return false;

        value = JsonConvert.DeserializeObject<T>(json);
        return true;
    }


    private string SerializeVariable(object? value)
    {
        if (value is Vector3 vector3)
            value = new Coord3(vector3);
        
        return JsonConvert.SerializeObject(value);
    }

    private T? DeserializeVariable<T>(string json)
    {
        if (string.IsNullOrWhiteSpace(json))
            return default;

        return JsonConvert.DeserializeObject<T>(json);
    }
    
    [OnDeserialized]
    private void OnDeserialized(StreamingContext context)
    {
        Entities ??= Array.Empty<Entity>();
        Metadata ??= new Dictionary<string, string>();
    }
}