using UnityEngine;

namespace SubnauticaEntityRipper.Data.Implementation;

public class CoreEntityData
{
    public string ClassId { get; }
    public string UniqueId { get; }
    public Vector3 Position { get; }
    public Quaternion Rotation { get; }
    public Vector3 Scale { get; }

    public CoreEntityData(string classId, string uniqueId, Vector3 position, Quaternion rotation, Vector3 scale)
    {
        ClassId = classId;
        UniqueId = uniqueId;
        Position = position;
        Rotation = rotation;
        Scale = scale;
    }

    public override string ToString()
    {
        return $"cid: {ClassId}, uid: {UniqueId}, pos: {Position}, rot: {Rotation}, scl: {Scale}";
    }
}