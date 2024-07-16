﻿using System;
using System.Linq;
using ModStructureHelperPlugin.Editing.Tools;
using ModStructureHelperPlugin.Interfaces;
using ModStructureHelperPlugin.UI;
using UnityEngine;

namespace ModStructureHelperPlugin.Mono;

public class EnableColliderForSelection : MonoBehaviour, ITransformationListener
{
    public SphereCollider managedCollider;

    private void OnEnable()
    {
        UpdateManagedCollider(true);
        var ui = StructureHelperUI.main;
        if (ui != null) ui.toolManager.OnToolStateChangedHandler += OnToolStateChanged;
    }

    private void Start()
    {
        UpdateManagedCollider(true);
    }

    private void OnDisable()
    {
        UpdateManagedCollider(false);
        var ui = StructureHelperUI.main;
        if (ui != null) ui.toolManager.OnToolStateChangedHandler -= OnToolStateChanged;
    }

    private void OnToolStateChanged(ToolBase tool, bool toolEnabled)
    {
        UpdateManagedCollider(GetSelectionColliderShouldEnable());
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

    private void UpdateManagedCollider(bool enableCollider)
    {
        if (!managedCollider) return;
        managedCollider.enabled = enableCollider;
        if (enableCollider)
        {
            UpdateColliderScale();
        }
    }

    private void UpdateColliderScale()
    {
        var thisObjectScale = (transform.lossyScale.x + transform.lossyScale.y + transform.lossyScale.z) / 3f;
        // what even are these random numbers I chose?
        managedCollider.radius = Mathf.Clamp(1f / thisObjectScale, 0.00001f, 100000f);
    }

    public void OnStartTransforming()
    {
        
    }

    public void OnFinishTransforming()
    {
        UpdateColliderScale();
    }
}