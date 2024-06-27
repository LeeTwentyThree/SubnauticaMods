using System.Collections;
using UnityEngine;
using UWE;

namespace ModStructureHelperPlugin.UndoSystem;

public readonly struct DeleteEntityMemento : IMemento
{
    public int SaveFrame { get; }

    private string ClassId { get; }
    private IMemento LastEntityMemento { get; }

    public DeleteEntityMemento(string classId, IMemento lastEntityMemento, int saveFrame)
    {
        ClassId = classId;
        LastEntityMemento = lastEntityMemento;
        SaveFrame = saveFrame;
    }

    private IEnumerator Restore()
    {
        var request = PrefabDatabase.GetPrefabAsync(ClassId);
        yield return request;
        if (!request.TryGetPrefab(out var prefab))
        {
            ErrorMessage.AddMessage($"Failed to load prefab by Class ID {ClassId}!");
            yield break;
        }

        var structureInstance = StructureInstance.Main;
        if (structureInstance == null)
        {
            ErrorMessage.AddMessage("Current structure was destroyed before undo operation could be complete!");
            yield break;
        }

        var obj = Object.Instantiate(prefab);
        var prefabIdentifier = obj.GetComponent<PrefabIdentifier>();
        var managedEntity = structureInstance.RegisterNewEntity(prefabIdentifier, false);
        var entityMementoCast = (ManagedEntity.Memento) LastEntityMemento;
        var alteredMemento = new ManagedEntity.Memento(managedEntity, entityMementoCast.Id, entityMementoCast.Position,
            entityMementoCast.Rotation, entityMementoCast.Scale, Time.frameCount);
        yield return alteredMemento.Restore();
    }
    
    IEnumerator IMemento.Restore()
    {
        yield return Restore();
    }
}