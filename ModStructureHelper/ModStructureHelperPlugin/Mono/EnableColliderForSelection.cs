using System;
using ModStructureHelperPlugin.Tools;
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
        if ((tool.Type == ToolType.Select || tool.Type == ToolType.DragAndDrop) && managedCollider)
        {
            managedCollider.enabled = toolEnabled;
        }
    }

    private void OnDestroy()
    {
        Destroy(managedCollider);
    }
}