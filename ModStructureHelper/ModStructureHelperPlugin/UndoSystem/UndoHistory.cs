using System.Collections;
using System.Collections.Generic;
using ModStructureHelperPlugin.Editing.Managers;
using ModStructureHelperPlugin.Editing.Tools;
using ModStructureHelperPlugin.StructureHandling;
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

        if (!TryGetLastValidMemento(out var lastValidMementoIndex))
        {
            _mementos.Clear();
            _undoing = false;
            ErrorMessage.AddMessage("Failed to undo any more.");
            yield break;
        }

        var changes = 0;
        
        var lastMementoSaveFrame = _mementos[lastValidMementoIndex].SaveFrame;
        var latestSnapshots = GetLatestSnapshotGroup(lastValidMementoIndex);
        foreach (var snapshot in latestSnapshots)
        {
            yield return snapshot.Restore();
            changes++;
        }
        
        ErrorMessage.AddMessage(changes == 1 ? "Reverted one change." : $"Reverted {changes} changes.");
        
        _mementos.RemoveAll(memento => memento.SaveFrame == lastMementoSaveFrame);
        
        _undoing = false;
    }

    private bool TryGetLastValidMemento(out int lastMementoIndex)
    {
        for (var i = _mementos.Count - 1; i >= 0; i--)
        {
            if (!_mementos[i].Invalid)
            {
                lastMementoIndex = i;
                return true;
            }
        }

        lastMementoIndex = 0;
        return false;
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

    private IEnumerable<IMemento> GetLatestSnapshotGroup(int lastIndex)
    {
        var lastMemento = _mementos[lastIndex];
        yield return lastMemento;
        var lastMementoFrame = lastMemento.SaveFrame;
        for (var i = _mementos.Count - 2; i >= 0; i--)
        {
            if (_mementos[i].Invalid || _mementos[i].SaveFrame < lastMementoFrame)
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
            if (entityInstance != null)
                entityInstance.ManagedEntity.CreateAndSaveSnapshot();
            var transformableObject = selected.GetComponent<TransformableObject>();
            if (transformableObject != null)
                transformableObject.CreateAndSaveSnapshot();
        }
    }
}