using System.Collections;
using ModStructureFormat;
using ModStructureHelperPlugin.UI;
using ModStructureHelperPlugin.UndoSystem;
using UnityEngine;

namespace ModStructureHelperPlugin;

public class ManagedEntity : IOriginator
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
        set
        {
            if (EntityInstance != null) EntityInstance.ForceIdChange(value);
            EntityData.id = value;
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
            EntityData.position = new Coord3(value);
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
            EntityData.rotation = new Coord4(value);
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
            EntityData.scale = new Coord3(value);
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

    public IMemento GetSnapshot()
    {
        return new Memento(this, Id, Position, Rotation, Scale, Time.frameCount);
    }

    private void SetState(Memento memento)
    {
        EntityData = EntityInstance.GetEntityDataStruct();
        Position = memento.Position;
        Rotation = memento.Rotation;
        Scale = memento.Scale;
        if (Id != memento.Id) Id = memento.Id;    
    }

    public void CreateAndSaveSnapshot()
    {
        StructureHelperUI.main.toolManager.undoHistory.Snapshot(GetSnapshot());
    }

    public readonly struct Memento : IMemento
    {
        private ManagedEntity Originator { get; }
        public string Id { get; }
        public Vector3 Position { get; }
        public Quaternion Rotation { get; }
        public Vector3 Scale { get; }
        public int SaveFrame { get; }

        public Memento(ManagedEntity originator, string id, Vector3 position, Quaternion rotation, Vector3 scale, int saveFrame)
        {
            Originator = originator;
            Id = id;
            Position = position;
            Rotation = rotation;
            Scale = scale;
            SaveFrame = saveFrame;
        }

        public IEnumerator Restore()
        {
            Originator?.SetState(this);
            yield break;
        }
    }
}