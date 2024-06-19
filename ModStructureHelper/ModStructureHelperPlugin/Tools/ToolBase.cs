using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ModStructureHelperPlugin.Tools;

public abstract class ToolBase : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI bindText;
    [SerializeField] private Image iconBackground;
    [SerializeField] private Sprite inactiveBackground;
    [SerializeField] private Sprite activeBackground;
    
    public ToolManager manager;
    
    public abstract ToolType Type { get; }
    public virtual bool MultitaskTool => false;
    public virtual bool PairedWithControl => false;
    public virtual bool IncompatibleWithSelectTool => false;
    
    public bool ToolEnabled { get; private set; }
    
    private bool _selectToolWasDisabledByThis;
    
    private void OnEnable()
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
        DisableOtherTools();
        iconBackground.sprite = activeBackground;
        ToolEnabled = true;
        manager.OnToolStateChangedHandler?.Invoke(this, true);
        
        OnToolEnabled();
    }

    public void DisableTool()
    {
        if (!ToolEnabled) return;
        iconBackground.sprite = inactiveBackground;
        ToolEnabled = false;
        manager.OnToolStateChangedHandler?.Invoke(this, false);
        
        OnToolDisabled();
        
        if (IncompatibleWithSelectTool && _selectToolWasDisabledByThis)
        {
            foreach (var tool in manager.tools)
            {
                if (tool.Type == ToolType.Select)
                {
                    tool.EnableTool();
                }
            }

            _selectToolWasDisabledByThis = false;
            SelectionManager.ClearSelection();   
        }
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
            if (!tool.MultitaskTool)
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
}