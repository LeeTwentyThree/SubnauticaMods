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
    
    public bool ToolEnabled { get; private set; }
    
    private void OnEnable()
    {
        bindText.text = GameInput.GetKeyCodeAsInputName(manager.GetKeyBindForTool(Type));
    }

    public void EnableTool()
    {
        iconBackground.sprite = activeBackground;
        ToolEnabled = true;
        OnToolEnabled();
    }

    public void DisableTool()
    {
        iconBackground.sprite = inactiveBackground;
        ToolEnabled = false;
        OnToolDisabled();
    }
    
    protected abstract void OnToolEnabled();
    protected abstract void OnToolDisabled();

    public virtual void UpdateTool(){}
    
    public virtual void DisableOtherTools()
    {
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
            DisableOtherTools();
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