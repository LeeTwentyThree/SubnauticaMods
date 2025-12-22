using System;
using UnityEngine;

namespace PodshellLeviathan.StructureFormat;

/// <summary>
/// Represents a single entity.
/// </summary>
[Serializable]
public class Entity
{
    public string classId;
    public string id;
    public Coord3 position;
    public Coord4 rotation;
    public Coord3 scale;
    public CellLevel cellLevel;
    public Priority priority;

    public Entity(string classId, string id, Vector3 position, Quaternion rotation, Vector3 scale, CellLevel cellLevel, Priority priority = Priority.Normal)
    {
        this.classId = classId;
        this.id = id;
        this.position = new Coord3(position);
        this.rotation = new Coord4(rotation);
        this.scale = new Coord3(scale);
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

    public override string ToString()
    {
        return $"{id} (Class ID: {classId})";
    }
}