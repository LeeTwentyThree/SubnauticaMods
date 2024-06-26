using ModStructureFormat;
using UnityEngine;

namespace ModStructureHelperPlugin;

public class ManagedEntity
{
    public Entity EntityData { get; private set; }
    public EntityInstance EntityInstance { get; private set; }

    public string ClassId
    {
        get
        {
            if (EntityInstance != null) return EntityInstance.ClassId;
            return EntityData.classId;
        }
    }
    
    public string Id
    {
        get
        {
            if (EntityInstance != null) return EntityInstance.Id;
            return EntityData.id;
        }
    }

    public Vector3 Position
    {
        get
        {
            if (EntityInstance != null) return EntityInstance.transform.position;
            return EntityData.position.ToVector3();
        }
        set
        {
            if (EntityInstance != null) EntityInstance.transform.position = value;
            else EntityData.position = new Coord3(value);
        }
    }
    
    public Quaternion Rotation
    {
        get
        {
            if (EntityInstance != null) return EntityInstance.transform.rotation;
            return EntityData.rotation.ToQuaternion();
        }
        set
        {
            if (EntityInstance != null) EntityInstance.transform.rotation = value;
            else EntityData.rotation = new Coord4(value);
        }
    }
    
    public Vector3 Scale
    {
        get
        {
            if (EntityInstance != null) return EntityInstance.transform.localScale;
            return EntityData.scale.ToVector3();
        }
        set
        {
            if (EntityInstance != null) EntityInstance.transform.localScale = value;
            else EntityData.scale = new Coord3(value));
        }
    }

    public ManagedEntity(Entity entityData)
    {
        EntityData = entityData;
    }

    public ManagedEntity(EntityInstance entityInstance)
    {
        EntityInstance = entityInstance;
    }

    public void AssignEntityInstance(EntityInstance instance)
    {
        EntityInstance = instance;
    }
    
    public void RemoveAssignedEntityInstance()
    {
        if (EntityInstance != null)
        {
            EntityData = EntityInstance.GetEntityDataStruct();
        }
        else
        {
            var warningMessage =
                $"Entity with ID {Id} and Class ID {ClassId} failed to unload properly; object was removed before its data could be saved.";
            ErrorMessage.AddMessage(warningMessage);
            Plugin.Logger.LogWarning(warningMessage);
        }
        EntityInstance = null;
    }
}