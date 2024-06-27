using System.Collections;
using System.Collections.Generic;
using ModStructureHelperPlugin.Tools;
using ModStructureHelperPlugin.UI;
using UnityEngine;

namespace ModStructureHelperPlugin.UndoSystem;

// This class manages the undo history and listens to the runtime handle for when an object is moved
public class UndoHistory : MonoBehaviour
{
    private readonly List<IMemento> _mementos = new();
    
    public void ClearMemory() => _mementos.Clear();

    private bool _undoing;
    
    public IEnumerator Undo()
    {
        if (_mementos.Count == 0)
        {
            ErrorMessage.AddMessage("Cannot undo any more.");
            yield break;
        }

        _undoing = true;

        var lastMementoSaveFrame = _mementos[^1].SaveFrame;
        var latestSnapshots = GetLatestSnapshotGroup();
        foreach (var snapshot in latestSnapshots)
        {
            yield return snapshot.Restore();
        }
        
        _mementos.RemoveAll(memento => memento.SaveFrame == lastMementoSaveFrame);
        
        _undoing = false;
    }

    public void Snapshot(IMemento memento)
    {
        if (_undoing)
        {
            ErrorMessage.AddMessage("Failed to generate undo snapshot for the latest change. Please DON'T make changes while the undo process is still busy!");
            return;
        }
        _mementos.Add(memento);
    }

    private IEnumerable<IMemento> GetLatestSnapshotGroup()
    {
        var lastMemento = _mementos[^1];
        yield return lastMemento;
        var lastMementoFrame = lastMemento.SaveFrame;
        for (var i = _mementos.Count - 2; i >= 0; i--)
        {
            if (_mementos[i].SaveFrame < lastMementoFrame)
            {
                break;
            }

            yield return _mementos[i];
        }
    }

    private void Awake()
    {
        StructureInstance.OnStructureInstanceChanged += OnStructureInstanceChanged;
        StructureHelperUI.main.toolManager.handle.startedDraggingHandle.AddListener(OnEndEditing);
    }

    private void OnDestroy()
    {
        StructureInstance.OnStructureInstanceChanged -= OnStructureInstanceChanged;
        StructureHelperUI.main.toolManager.handle.startedDraggingHandle.RemoveListener(OnEndEditing);
    }

    private void OnStructureInstanceChanged(StructureInstance instance)
    {
        ClearMemory();
    }

    private void OnEndEditing()
    {
        foreach (var selected in SelectionManager.SelectedObjects)
        {
            if (selected == null) continue;
            var entityInstance = selected.GetComponent<EntityInstance>();
            if (entityInstance == null) continue;
            entityInstance.ManagedEntity.CreateAndSaveSnapshot();
        }
    }
}