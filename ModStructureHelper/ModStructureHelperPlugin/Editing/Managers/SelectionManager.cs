﻿using System.Collections.Generic;
using ModStructureHelperPlugin.Handle;
using ModStructureHelperPlugin.Handle.Handles;
using ModStructureHelperPlugin.Interfaces;
using ModStructureHelperPlugin.OutlineShit;
using ModStructureHelperPlugin.OutlineShit.Rendering;
using ModStructureHelperPlugin.StructureHandling;
using UnityEngine;

namespace ModStructureHelperPlugin.Editing.Managers;

public static class SelectionManager
{
    private static List<GameObject> _targets = new();

    public static bool IsSelected(GameObject obj) => _targets.Contains(obj);
    
    public static void SetSelectedObject(GameObject obj)
    {
        _targets.ForEach(old => OnTargetRemovedInternal(old));
        _targets = new List<GameObject> {obj};
        OnTargetAddedInternal(obj);
        OnUpdateTargetInternal();
    }

    public static void ClearSelection()
    {
        _targets.ForEach(obj => OnTargetRemovedInternal(obj));
        _targets.Clear();
        OnUpdateTargetInternal();
    }
    
    public static void AddSelectedObject(GameObject obj)
    {
        _targets.Add(obj);
        OnTargetAddedInternal(obj);
        OnUpdateTargetInternal();
    }
    
    public static void RemoveSelectedObject(GameObject obj)
    {
        OnTargetRemovedInternal(obj);
        _targets.Remove(obj);
        OnUpdateTargetInternal();
    }

    public static int NumberOfSelectedObjects => _targets.Count;
    
    public static IEnumerable<GameObject> SelectedObjects => _targets;

    private static void OnTargetAddedInternal(GameObject newTarget)
    {
        if (newTarget == null) return;
        var outline = AddOutline(newTarget);
        if (outline.OutlineRenderers.Count == 0)
        {
            ErrorMessage.AddMessage($"Selecting {newTarget.gameObject}, which has no active renderers!");
        }
        foreach (var selectionListener in newTarget.GetComponents<ISelectionListener>())
            selectionListener.OnObjectSelected();
    }
    
    private static void OnTargetRemovedInternal(GameObject target)
    {
        if (target == null) return;
        Object.DestroyImmediate(target.GetComponent<OutlineBehaviour>());
        foreach (var selectionListener in target.GetComponents<ISelectionListener>())
            selectionListener.OnObjectDeselected();
    }

    private static OutlineBehaviour AddOutline(GameObject obj)
    {
        var existing = obj.GetComponent<OutlineBehaviour>();
        if (existing) return existing;
        
        var outlineBehaviour = obj.AddComponent<OutlineBehaviour>();
        outlineBehaviour.OutlineResources = Plugin.OutlineResources;
        
        outlineBehaviour.OutlineColor = Color.yellow;
        outlineBehaviour.OutlineWidth = 8;
        outlineBehaviour.OutlineRenderMode = OutlineRenderFlags.Blurred;
        return outlineBehaviour;
    }

    public static ObjectRootResult TryGetObjectRoot(GameObject obj, out GameObject root, SelectionFilterMode filter)
    {
        if (obj.GetComponentInParent<Player>() != null || obj.GetComponentInChildren<Player>() != null)
        {
            root = null;
            return ObjectRootResult.Failed;
        }
        
        if (obj.GetComponentInParent<HandleBase>() != null)
        {
            root = null;
            return ObjectRootResult.Failed;
        }

        if (filter.HasFlag(SelectionFilterMode.AllowTransformableObjects) && obj.GetComponentInParent<TransformableObject>() != null)
        {
            root = obj;
            return ObjectRootResult.Success;
        }
        
        var componentInParent = obj.GetComponentInParent<PrefabIdentifier>();
        if (componentInParent)
        {
            root = componentInParent.gameObject;
            if (!filter.HasFlag(SelectionFilterMode.NoStructureRequired) && StructureInstance.Main != null)
            {
                if (!StructureInstance.Main.IsEntityPartOfStructure(componentInParent.Id))
                {
                    ErrorMessage.AddMessage($"Cannot edit '{root.gameObject.name}'; this is not part of the currently selected structure.");
                    return ObjectRootResult.Failed;
                }
            }

            return ObjectRootResult.Success;
        }

        root = null;
        return ObjectRootResult.NoSelection;
    }

    public enum ObjectRootResult
    {
        NoSelection,
        Success,
        Failed
    }

    [System.Flags]
    public enum SelectionFilterMode
    {
        Default,
        NoStructureRequired = 1,
        AllowTransformableObjects = 2,
    }

    private static void OnUpdateTargetInternal()
    {
        var runtimeTransformHandle = RuntimeTransformHandle.main;
        if (!runtimeTransformHandle)
        {
            Plugin.Logger.LogError("Cannot update selection - the runtime transform handle does not exist!");
        }

        // remove null elements
        _targets.RemoveAll(target => target == null);
        
        // update the transformation handle
        switch (_targets.Count)
        {
            case 0:
                runtimeTransformHandle.SetTarget(null);
                break;
            case 1:
                runtimeTransformHandle.SetTarget(_targets[0].transform);
                break;
            case > 1:
                ErrorMessage.AddMessage("Unsupported number of objects selected. Multi-object selection is not supported!");
                break;
        }
    }
}