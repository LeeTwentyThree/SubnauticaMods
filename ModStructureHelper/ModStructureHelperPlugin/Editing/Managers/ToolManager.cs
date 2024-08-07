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
            if (Input.GetKeyDown(GetKeyBindForTool(tool.Type)) && (!tool.PairedWithControl || Input.GetKey(KeyCode.LeftControl)))
            {
                tool.OnToolButtonPressed();
            }
            if (tool.ToolEnabled) tool.UpdateTool();
        }
    }
    
    public ToolBase GetTool(ToolType type) => (from tool in tools where tool.Type == type select tool).FirstOrDefault();

    public KeyCode GetKeyBindForTool(ToolType tool)
    {
        switch (tool)
        {
            case ToolType.Select:
                return Plugin.ModConfig.SelectBind;
            case ToolType.Translate:
                return Plugin.ModConfig.TranslateBind;
            case ToolType.Rotate:
                return Plugin.ModConfig.RotateBind;
            case ToolType.Scale:
                return Plugin.ModConfig.ScaleBind;
            case ToolType.DragAndDrop:
                return Plugin.ModConfig.DragBind;
            case ToolType.BrowseEntities:
                return Plugin.ModConfig.EntityEditorBind;
            case ToolType.PaintBrush:
                return Plugin.ModConfig.PaintBrushBind;
            case ToolType.GlobalSpace:
                return Plugin.ModConfig.ToggleGlobalSpaceBind;
            case ToolType.Snapping:
                return Plugin.ModConfig.ToggleSnappingBind;
            case ToolType.ObjectPicker:
                return Plugin.ModConfig.PickObjectBind;
            case ToolType.CableGenerator:
                return Plugin.ModConfig.CableEditorBind;
            case ToolType.Duplicate:
                return Plugin.ModConfig.DuplicateBind;
            case ToolType.SelectAll:
                return Plugin.ModConfig.SelectAllBind;
            case ToolType.Undo:
                return Plugin.ModConfig.UndoBind;
            case ToolType.Delete:
                return Plugin.ModConfig.DeleteBind;
            default:
                Plugin.Logger.LogWarning($"No keybind implemented for tool '{tool}'!");
                return KeyCode.None;
        }
    }
}