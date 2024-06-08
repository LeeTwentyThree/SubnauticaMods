using UnityEngine;

namespace ModStructureHelperPlugin.EntityHandling;

public class EntityInstance
{
    public string ClassId { get; }
    public string UniqueId { get; }
    public Vector3 Position { get; }
    public Quaternion Rotation { get; }
    public Vector3 Scale { get; }

    public EntityInstance(string classId, string uniqueId, Vector3 position, Quaternion rotation, Vector3 scale)
    {
        ClassId = classId;
        UniqueId = uniqueId;
        Position = position;
        Rotation = rotation;
        Scale = scale;
    }
}
