﻿using System.Linq;
using ModStructureHelperPlugin.Editing.Tools;
using ModStructureHelperPlugin.Handle;
using ModStructureHelperPlugin.UI;
using ModStructureHelperPlugin.UndoSystem;
using UnityEngine;

namespace ModStructureHelperPlugin.Editing.Managers;

public class ToolManager : MonoBehaviour
{
    public ToolBase[] tools;

    public RuntimeTransformHandle handle;
    public UndoHistory undoHistory;
    
    public Sprite inactiveBackground;
    public Sprite activeBackground;

    public delegate void OnToolStateChanged(ToolBase tool, bool toolEnabled);

    public OnToolStateChanged OnToolStateChangedHandler;
    
    private void OnValidate()
    {
        tools = GetComponentsInChildren<ToolBase>();
    }

    private void Update()
    {
        // Return early if not focused or if the entity browser window is open
        var entityBrowserOpen = UIEntityWindow.Main != null && UIEntityWindow.Main.isActiveAndEnabled;
        if (!StructureHelperUI.main.IsFocused) return;
        
        foreach (var tool in tools)
        {
            if (tool.Type != ToolType.BrowseEntities && entityBrowserOpen) continue;
            if (GameInput.GetButtonDown(GetButtonForTool(tool.Type)) && (!tool.RequiresModifierHeld || GameInput.GetButtonHeld(StructureHelperInput.ToolHotkeyModifier)))
            {
                tool.OnToolButtonPressed();
            }
            if (tool.ToolEnabled) tool.UpdateTool();
        }
    }
    
    public ToolBase GetTool(ToolType type) => (from tool in tools where tool.Type == type select tool).FirstOrDefault();

    public GameInput.Button GetButtonForTool(ToolType tool)
    {
        switch (tool)
        {
            case ToolType.Select:
                return StructureHelperInput.SelectBind;
            case ToolType.Translate:
                return StructureHelperInput.TranslateBind;
            case ToolType.Rotate:
                return StructureHelperInput.RotateBind;
            case ToolType.Scale:
                return StructureHelperInput.ScaleBind;
            case ToolType.DragAndDrop:
                return StructureHelperInput.DragBind;
            case ToolType.BrowseEntities:
                return StructureHelperInput.EntityEditorBind;
            case ToolType.PaintBrush:
                return StructureHelperInput.PaintBrushBind;
            case ToolType.GlobalSpace:
                return StructureHelperInput.ToggleGlobalSpaceBind;
            case ToolType.Snapping:
                return StructureHelperInput.ToggleSnappingBind;
            case ToolType.ObjectPicker:
                return StructureHelperInput.PickObjectBind;
            case ToolType.CableGenerator:
                return StructureHelperInput.CableEditorBind;
            case ToolType.Duplicate:
                return StructureHelperInput.DuplicateBind;
            case ToolType.SelectAll:
                return StructureHelperInput.SelectAllBind;
            case ToolType.Undo:
                return StructureHelperInput.UndoBind;
            case ToolType.Delete:
                return StructureHelperInput.DeleteBind;
            default:
                Plugin.Logger.LogWarning($"No keybind implemented for tool '{tool}'!");
                return GameInput.Button.None;
        }
    }
}