using ModStructureHelperPlugin.Editing.Managers;
using ModStructureHelperPlugin.StructureHandling;
using ModStructureHelperPlugin.UI;
using ModStructureHelperPlugin.Utility;
using UnityEngine;

namespace ModStructureHelperPlugin.Editing.Tools;

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
        if (!GameInput.GetButtonDown(StructureHelperInput.Interact)) return;
        if (StructureHelperUI.main.editingScreenChecker.IsCursorHoveredOverExternalWindows()) return;
        if (manager.handle.GetIsAnyHandleHovered()) return;

        var prioritizeTriggers = ModifierFixUtils.GetModifierHeld(StructureHelperInput.PrioritizeTriggers);
        
        var ray = MainCamera.camera.ScreenPointToRay(Input.mousePosition);
        // extend the ray to ignore the main character's collider
        var extendedRay = new Ray(ray.origin + ray.direction * 0.5f, ray.direction);
        var hitSolid = Physics.Raycast(extendedRay, out var hit, 5000, -1, QueryTriggerInteraction.Ignore);
        if (hitSolid)
        {
            var selectionResultNormal = SelectionManager.TryGetObjectRoot(hit.collider.gameObject, out var solidRaycastRoot, SelectionManager.SelectionFilterMode.AllowTransformableObjects);
            if (selectionResultNormal == SelectionManager.ObjectRootResult.Success)
            {
                HandleObjectSelection(solidRaycastRoot);
                return;
            }

            if (selectionResultNormal == SelectionManager.ObjectRootResult.Failed)
            {
                if (!prioritizeTriggers && Plugin.ModConfig.ClickOffToUnselect)
                {
                    SelectionManager.ClearSelection();
                }
                return;
            }
        }

        if (!prioritizeTriggers && Plugin.ModConfig.ClickOffToUnselect)
        {
            SelectionManager.ClearSelection();
            return;
        }
        
        var hitTrigger = Physics.Raycast(extendedRay, out hit, 5000, -1, QueryTriggerInteraction.Collide);
        if (!hitTrigger && prioritizeTriggers)
        {
            SelectionManager.ClearSelection();
        }
        var selectionResultTrigger = SelectionManager.TryGetObjectRoot(hit.collider.gameObject, out var triggerRaycastRoot, SelectionManager.SelectionFilterMode.Default);
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
        
        if (GameInput.GetButtonHeld(StructureHelperInput.SelectMultipleModifier))
        {
            if (isSelected) SelectionManager.RemoveSelectedObject(obj);
            else SelectionManager.AddSelectedObject(obj);
        }
        else if (isSelected && SelectionManager.NumberOfSelectedObjects <= 1)
        {
            SelectionManager.ClearSelection();
        }
        else
        {
            SelectionManager.SetSelectedObject(obj);
        }
    }
}