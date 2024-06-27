using System;
using System.Collections.Generic;
using System.Collections;
using ModStructureHelperPlugin.UI;
using ModStructureHelperPlugin.UndoSystem;
using UnityEngine;

namespace ModStructureHelperPlugin.Tools;

public class DuplicateTool : ToolBase
{
    public override ToolType Type => ToolType.Duplicate;

    public override bool MultitaskTool => true;
    public override bool PairedWithControl => true;

    protected override void OnToolEnabled()
    {
        StartCoroutine(DuplicateAllSelectedObjects());
        DisableTool();
    }

    private IEnumerator DuplicateAllSelectedObjects()
    {
        var duplicatedObjects = new List<GameObject>();
        var loadedPrefabs = new Dictionary<string, GameObject>();
        var addedObjectIds = new List<string>();
        foreach (var selected in SelectionManager.SelectedObjects)
        {
            var prefabIdentifier = selected.GetComponent<PrefabIdentifier>();
            
            if (prefabIdentifier == null)
            {
                ErrorMessage.AddMessage($"Cannot duplicate '{selected}'; this object has no prefab identifier!");
                continue;
            }
            
            if (!loadedPrefabs.TryGetValue(prefabIdentifier.ClassId, out var prefab))
            {
                var task = UWE.PrefabDatabase.GetPrefabAsync(prefabIdentifier.ClassId);
                yield return task;
                if (!task.TryGetPrefab(out prefab)) ErrorMessage.AddMessage($"Failed to load prefab for prefab of Class ID {prefabIdentifier.ClassId}");
                loadedPrefabs.Add(prefabIdentifier.ClassId, prefab);
            }

            var newCopy = Instantiate(prefab);
            newCopy.transform.position = selected.transform.position;
            newCopy.transform.rotation = selected.transform.rotation;
            newCopy.transform.localScale = selected.transform.localScale;

            var newEntityIdentifier = newCopy.GetComponent<PrefabIdentifier>();
            StructureInstance.Main.RegisterNewEntity(newEntityIdentifier, false);
            addedObjectIds.Add(newEntityIdentifier.Id);
            duplicatedObjects.Add(newCopy);
        }
        SelectionManager.ClearSelection();
        duplicatedObjects.ForEach(SelectionManager.AddSelectedObject);
        // MANUALLY snapshot the objects being added, all at once, so that it all occurs in the same frame!
        addedObjectIds.ForEach(id => StructureHelperUI.main.toolManager.undoHistory.Snapshot(new AddEntityMemento(id, Time.frameCount)));
        ErrorMessage.AddMessage($"Duplicated {duplicatedObjects.Count} object(s).");
    }

    protected override void OnToolDisabled() { }
}