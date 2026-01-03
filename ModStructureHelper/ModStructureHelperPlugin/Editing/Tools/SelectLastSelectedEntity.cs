using ModStructureHelperPlugin.Editing.Managers;

namespace ModStructureHelperPlugin.Editing.Tools;

public class SelectLastSelectedEntity : ToolBase
{
    public override ToolType Type => ToolType.SelectLastSelected;
    
    public override bool MultitaskTool => true;
    public override bool RequiresAlternateModifierHeld => true;

    protected override void OnToolEnabled()
    {
        SelectionManager.SelectLastSelected();
        DisableTool();
    }

    protected override void OnToolDisabled()
    {
        
    }
}