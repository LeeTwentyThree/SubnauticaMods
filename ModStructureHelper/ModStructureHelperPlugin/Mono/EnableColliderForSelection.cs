using System.Linq;
using ModStructureHelperPlugin.Editing.Tools;
using ModStructureHelperPlugin.Handle;
using ModStructureHelperPlugin.UI;
using UnityEngine;

namespace ModStructureHelperPlugin.Mono;

public class EnableColliderForSelection : MonoBehaviour
{
    public SphereCollider managedCollider;

    private void OnEnable()
    {
        if (managedCollider) managedCollider.enabled = true;
        var ui = StructureHelperUI.main;
        if (ui != null) ui.toolManager.OnToolStateChangedHandler += OnToolStateChanged;
    }

    private void OnDisable()
    {
        if (managedCollider) managedCollider.enabled = false;
        var ui = StructureHelperUI.main;
        if (ui != null) ui.toolManager.OnToolStateChangedHandler -= OnToolStateChanged;
    }

    private void OnToolStateChanged(ToolBase tool, bool toolEnabled)
    {
        if (managedCollider) managedCollider.enabled = GetSelectionColliderShouldEnable();
    }

    private bool GetSelectionColliderShouldEnable()
    {
        return StructureHelperUI.main.toolManager.tools.Any(tool =>
            tool.ToolEnabled && (tool.Type is ToolType.Select or ToolType.ObjectPicker ||
            tool.Type is ToolType.DragAndDrop && !((DragAndDropTool) tool).Dragging));
    }

    private void OnDestroy()
    {
        Destroy(managedCollider);
    }
}