using System.Collections;
using ModStructureFormat;
using ModStructureHelperPlugin.UI;
using ModStructureHelperPlugin.UndoSystem;
using UnityEngine;

namespace ModStructureHelperPlugin.StructureHandling;

public class ManagedEntity : IOriginator
{
    public Entity EntityData { get; private set; }
    public EntityInstance EntityInstance { get; private set; }
    public bool WasDeleted { get; private set; }

    public string ClassId
    {
        get
        {
            if (EntityInstance != null && !string.IsNullOrEmpty(EntityInstance.ClassId)) return EntityInstance.ClassId;
            return EntityData.classId;
        }
    }
    
    public string Id
    {
        get
        {
            if (EntityInstance != null && !string.IsNullOrEmpty(EntityInstance.Id)) return EntityInstance.Id;
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
        EntityInstance = null;
    }
    
    public void MarkAsDeleted() => WasDeleted = true;

    public IMemento GetSnapshot()
    {
        return new Memento(this, Id, Position, Rotation, Scale, Time.frameCount);
    }

    private void SetState(Memento memento)
    {
        if (EntityData == null)
        {
            if (EntityInstance == null)
            {
                var errorMessage = $"Cannot undo; the entity of ID '{memento.Id}' does not have data or an existing instance in the world.";
                ErrorMessage.AddMessage(errorMessage);
                Plugin.Logger.LogError(errorMessage);
                return;
            }
            EntityData = EntityInstance.GetEntityDataStruct();
        }
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
        public bool Invalid => false;

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
            ManagedEntity originator = Originator;
            
            // If the original originator instance was destroyed, try to find a new one:
            if (originator == null || originator.WasDeleted)
            {
                var managedEntities = StructureInstance.Main.GetAllManagedEntities();
                foreach (var entity in managedEntities)
                {
                    if (entity.Id != Id) continue;
                    originator = entity;
                    break;
                }
            }

            if (originator == null)
            {
                var warningMessage = "Transformation undo failed; the object to be reverted cannot be found!";
                ErrorMessage.AddMessage(warningMessage);
                Plugin.Logger.LogWarning(warningMessage);
                yield break;
            }
            
            originator.SetState(this);
        }
    }
}