﻿using UnityEngine;

namespace ModStructureHelperPlugin.Tools;

public class ObjectPickerTool : ToolBase
{
    public override ToolType Type => ToolType.ObjectPicker;

    private bool _iDisabledTheSelectToolAndMustReenableItForTheGreaterGood;
    
    protected override void OnToolEnabled()
    {
        foreach (var tool in manager.tools)
        {
            if (tool.Type == ToolType.Select & tool.ToolEnabled)
            {
                tool.DisableTool();
                _iDisabledTheSelectToolAndMustReenableItForTheGreaterGood = true;
            }
        }
    }

    protected override void OnToolDisabled()
    {
        if (!_iDisabledTheSelectToolAndMustReenableItForTheGreaterGood) return;
        foreach (var tool in manager.tools)
        {
            if (tool.Type == ToolType.Select)
            {
                tool.EnableTool();
            }
        }
    }

    public override void UpdateTool()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var ray = MainCamera.camera.ScreenPointToRay(Input.mousePosition);
            // extend the ray to ignore the main character's collider
            var extendedRay = new Ray(ray.origin + MainCamera.camera.transform.forward * 0.5f, ray.direction);
            if (!Physics.Raycast(extendedRay, out var hit, 5000, -1, QueryTriggerInteraction.Ignore)) return;
            if (SelectionManager.TryGetObjectRoot(hit.collider.gameObject, out var root))
                HandleObjectPicking(root);
        }   
    }
    
    private void HandleObjectPicking(GameObject obj)
    {
        var prefabIdentifier = obj.GetComponent<PrefabIdentifier>();
        if (prefabIdentifier == null)
        {
            ErrorMessage.AddMessage($"Warning: {obj} has no PrefabIdentifier! Cannot pick this object for brushing.");
            return;
        }
        ErrorMessage.AddMessage($"The prefab with Class ID '{prefabIdentifier.ClassId}' has been selected for brushing.");
        
        ErrorMessage.AddMessage("implement brush picking logic here");
    }
}