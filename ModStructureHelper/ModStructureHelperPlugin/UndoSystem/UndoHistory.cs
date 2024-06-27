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
    
    public IEnumerator Undo()
    {
        if (_mementos.Count == 0)
        {
            ErrorMessage.AddMessage("Cannot undo any more.");
            yield break;
        }
        var memento = _mementos[^1];
        yield return memento.Restore();
        _mementos.Remove(memento);
    }

    public void Snapshot(IMemento memento)
    {
        _mementos.Add(memento);
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