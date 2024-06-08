using UnityEngine;

namespace ModStructureFormat;

/// <summary>
/// Represents a single entity.
/// </summary>
[Serializable]
public struct Entity
{
    public string classId;
    public string id;
    public Vector3 position;
    public Vector3 eulerAngles;
    public Vector3 scale;
    public CellLevel cellLevel;
    public Priority priority;

    public Entity(string classId, string id, Vector3 position, Vector3 eulerAngles, Vector3 scale, CellLevel cellLevel, Priority priority)
    {
        this.classId = classId;
        this.id = id;
        this.position = position;
        this.eulerAngles = eulerAngles;
        this.scale = scale;
        this.cellLevel = cellLevel;
        this.priority = priority;
    }

    /// <summary>
    /// Copy of the LargeWorldEntity.CellLevel enum in Subnautica.
    /// </summary>
    public enum CellLevel
    {
        Unknown = -1,
        Near = 0,
        Medium = 1,
        Far = 2,
        VeryFar = 3,
        Batch = 10,
        Global = 100
    }
    
    public enum Priority
    {
        Lowest = -128,
        VeryLow = -64,
        Low = -32,
        Normal = 0,
        High = 32,
        VeryHigh = 64,
        Highest = 128,
    }
}