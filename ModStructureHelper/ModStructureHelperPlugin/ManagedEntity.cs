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
    }
    
    public Quaternion Rotation
    {
        get
        {
            if (EntityInstance != null) return EntityInstance.transform.rotation;
            return EntityData.rotation.ToQuaternion();
        }
    }
    
    public Vector3 Scale
    {
        get
        {
            if (EntityInstance != null) return EntityInstance.transform.localScale;
            return EntityData.scale.ToVector3();
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

    public void SetEntityInstance(EntityInstance instance)
    {
        EntityInstance = instance;
    }
    
    public void RemoveCurrentEntityInstance()
    {
        if (EntityInstance != null)
        {
            EntityData = EntityInstance.GetEntityDataStruct();
        }
        else
        {
            ErrorMessage.AddMessage($"Entity with ID {Id} and Class ID {ClassId} failed to unload properly; object was removed before its data could be saved.");
        }
        EntityInstance = null;
    }
}