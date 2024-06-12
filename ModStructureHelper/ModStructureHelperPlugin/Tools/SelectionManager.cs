using System.Collections.Generic;
using ModStructureFormat;
using RuntimeHandle;
using UnityEngine;

namespace ModStructureHelperPlugin.Tools;

public static class SelectionManager
{
    private static List<GameObject> _targets = new();

    public static bool IsSelected(GameObject obj) => _targets.Contains(obj);
    
    public static void SetSelectedObject(GameObject obj)
    {
        _targets = new List<GameObject> {obj};
        OnUpdateTargetInternal();
    }

    public static void ClearSelection()
    {
        _targets.Clear();
        OnUpdateTargetInternal();
    }
    
    public static void AddSelectedObject(GameObject obj)
    {
        _targets.Add(obj);
        OnUpdateTargetInternal();
    }
    
    public static void RemoveSelectedObject(GameObject obj)
    {
        _targets.Remove(obj);
        OnUpdateTargetInternal();
    }

    public static int NumberOfSelectedObjects => _targets.Count;
    
    public static IEnumerable<GameObject> SelectedObjects => _targets;

    public static bool TryGetObjectRoot(GameObject obj, out GameObject root)
    {
        if (obj.GetComponentInParent<Player>() != null || obj.GetComponentInChildren<Player>() != null)
        {
            root = null;
            return false;
        }
        
        if (obj.GetComponentInParent<HandleBase>() != null)
        {
            root = null;
            return false;
        }
        
        var componentInParent = obj.GetComponentInParent<PrefabIdentifier>();
        if (componentInParent)
        {
            root = componentInParent.gameObject;
            if (StructureInstance.Main != null)
            {
                if (!StructureInstance.Main.IsEntityPartOfStructure(componentInParent.Id))
                {
                    ErrorMessage.AddMessage("Cannot edit this object; this is not part of the currently selected structure.");
                    return false;
                }
            }
            return true;
        }

        root = null;
        return false;
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