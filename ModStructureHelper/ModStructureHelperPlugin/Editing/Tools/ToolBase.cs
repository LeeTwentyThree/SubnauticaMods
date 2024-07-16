using ModStructureHelperPlugin.Editing.Managers;
using ModStructureHelperPlugin.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ModStructureHelperPlugin.Editing.Tools;

public abstract class ToolBase : TooltipTarget
{
    [SerializeField] private TextMeshProUGUI bindText;
    [SerializeField] private Image iconBackground;
    
    public ToolManager manager;
    
    public abstract ToolType Type { get; }
    public virtual bool MultitaskTool => false;
    public virtual bool PairedWithControl => false;
    public virtual bool IncompatibleWithSelectTool => false;
    
    public bool ToolEnabled { get; private set; }
    
    private bool _selectToolWasDisabledByThis;
    
    protected virtual void OnEnable()
    {
        bindText.text = GetBindString();
    }
    
    private string GetBindString()
    {
        var inputName = GameInput.GetKeyCodeAsInputName(manager.GetKeyBindForTool(Type));
        return PairedWithControl ? $"Ctrl + {inputName}" : inputName;
    }

    public void EnableTool()
    {
        if (ToolEnabled) return;
        iconBackground.sprite = manager.activeBackground;
        ToolEnabled = true;
        manager.OnToolStateChangedHandler?.Invoke(this, true);
        
        DisableOtherTools();
        OnToolEnabled();
    }

    public void DisableTool()
    {
        if (!ToolEnabled) return;
        iconBackground.sprite = manager.inactiveBackground;
        ToolEnabled = false;
        manager.OnToolStateChangedHandler?.Invoke(this, false);
        
        OnToolDisabled();

        if (!IncompatibleWithSelectTool || !_selectToolWasDisabledByThis) return;
        
        var stillIncompatible = false;
        foreach (var tool in manager.tools)
        {
            if (tool != this && tool.IncompatibleWithSelectTool && tool.ToolEnabled)
            {
                stillIncompatible = true;
            }
        }

        if (!stillIncompatible)
        {
            foreach (var tool in manager.tools)
            {
                if (tool.Type == ToolType.Select)
                {
                    tool.EnableTool();
                }
            }   
        }

        _selectToolWasDisabledByThis = false;
        SelectionManager.ClearSelection();
    }
    
    protected abstract void OnToolEnabled();
    protected abstract void OnToolDisabled();

    public virtual void UpdateTool(){}
    
    public virtual void DisableOtherTools()
    {
        if (IncompatibleWithSelectTool)
        {
            _selectToolWasDisabledByThis = false;
            foreach (var tool in manager.tools)
            {
                if (tool.Type == ToolType.Select && tool.ToolEnabled)
                {
                    tool.DisableTool();
                    _selectToolWasDisabledByThis = true;
                }
            }
        }
        
        if (MultitaskTool) return;
        
        foreach (var tool in manager.tools)
        {
            if (!tool.MultitaskTool && tool != this)
                tool.DisableTool();
        }
    }

    public void OnToolButtonPressed()
    {
        if (ToolEnabled)
        {
            DisableTool();
        }
        else
        {
            EnableTool();
        }
    }

    private void OnValidate()
    {
        if (manager == null) manager = GetComponentInParent<ToolManager>();
        if (bindText == null) bindText = GetComponentInChildren<TextMeshProUGUI>();
        if (iconBackground == null) iconBackground = GetComponent<Image>();
    }

    public override string GetTooltipText()
    {
        var typeName = Type.ToString();
        var prettyName = string.Empty;
        for (var i = 0; i < typeName.Length; i++)
        {
            if (i > 0 && char.IsUpper(typeName[i]))
            {
                prettyName += " ";
                prettyName += char.ToLower(typeName[i]);
            }
            else
            {
                prettyName += typeName[i];
            }
        }
        return prettyName;
    }
}