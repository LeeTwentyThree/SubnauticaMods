using System;
using UnityEngine;

namespace ModStructureHelperPlugin.Tools;

public class SelectionTool : ToolBase
{
    public override ToolType Type => ToolType.Select;

    public override bool MultitaskTool => true;

    private void Start()
    {
        EnableTool();
    }

    protected override void OnToolEnabled()
    {
        foreach (var tool in manager.tools)
        {
            if (tool.IncompatibleWithSelectTool)
                tool.DisableTool();
        }
    }

    protected override void OnToolDisabled()
    {
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
                HandleObjectSelection(root);
        }
    }

    private void HandleObjectSelection(GameObject obj)
    {
        if (StructureInstance.Main == null)
        {
            ErrorMessage.AddMessage("Cannot select objects while not editing any structure!");
            return;
        }
        
        var isSelected = SelectionManager.IsSelected(obj);
        
        if (Input.GetKey(KeyCode.LeftControl))
        {
            if (isSelected) SelectionManager.RemoveSelectedObject(obj);
            else SelectionManager.AddSelectedObject(obj);
        }
        else if (!isSelected)
        {
            SelectionManager.SetSelectedObject(obj);
        }
    }
}