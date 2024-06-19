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
        SelectionManager.ClearSelection();
    }

    public override void UpdateTool()
    {
        if (!Input.GetMouseButtonDown(0)) return;
        
        var ray = MainCamera.camera.ScreenPointToRay(Input.mousePosition);
        // extend the ray to ignore the main character's collider
        var extendedRay = new Ray(ray.origin + MainCamera.camera.transform.forward * 0.5f, ray.direction);
        var hitSolid = Physics.Raycast(extendedRay, out var hit, 5000, -1, QueryTriggerInteraction.Ignore);
        if (hitSolid)
        {
            var selectionResultNormal = SelectionManager.TryGetObjectRoot(hit.collider.gameObject, out var solidRaycastRoot);
            if (selectionResultNormal == SelectionManager.ObjectRootResult.Success)
            {
                HandleObjectSelection(solidRaycastRoot);
                return;
            }

            if (selectionResultNormal == SelectionManager.ObjectRootResult.Failed)
            {
                return;
            }
        }

        if (!Input.GetKey(Plugin.ModConfig.PrioritizeTriggers))
        {
            return;
        }
        
        var hitTrigger = Physics.Raycast(extendedRay, out hit, 5000, -1, QueryTriggerInteraction.Collide);
        if (!hitTrigger)
        {
            SelectionManager.ClearSelection();
        }
        var selectionResultTrigger = SelectionManager.TryGetObjectRoot(hit.collider.gameObject, out var triggerRaycastRoot);
        if (selectionResultTrigger == SelectionManager.ObjectRootResult.Success)
        {
            HandleObjectSelection(triggerRaycastRoot);
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