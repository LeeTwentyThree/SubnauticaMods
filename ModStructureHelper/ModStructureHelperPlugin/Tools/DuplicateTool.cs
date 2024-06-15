using System.Collections.Generic;
using UnityEngine;

namespace ModStructureHelperPlugin.Tools;

public class DuplicateTool : ToolBase
{
    public override ToolType Type => ToolType.Duplicate;

    public override bool MultitaskTool => true;
    public override bool PairedWithControl => true;

    protected override void OnToolEnabled()
    {
        DuplicateAllSelectedObjects();
        DisableTool();
    }

    private void DuplicateAllSelectedObjects()
    {
        var duplicatedObjects = new List<GameObject>();
        foreach (var selected in SelectionManager.SelectedObjects)
        {
            if (selected != null)
            {
                duplicatedObjects.Add(StructureInstance.Main.SpawnPrefabIntoStructure(selected.gameObject));
            }
        }
        SelectionManager.ClearSelection();
        duplicatedObjects.ForEach(SelectionManager.AddSelectedObject);
        ErrorMessage.AddMessage($"Duplicated {duplicatedObjects.Count} objects.");
    }

    protected override void OnToolDisabled() { }
}