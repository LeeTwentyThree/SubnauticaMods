using RuntimeHandle;

namespace ModStructureHelperPlugin.Tools;

public class ToggleGlobalSpaceTool : ToolBase
{
    public override ToolType Type => ToolType.GlobalSpace;

    public override bool MultitaskTool => true;

    protected override void OnToolEnabled()
    {
        manager.handle.space = HandleSpace.WORLD;
    }

    protected override void OnToolDisabled()
    {
        manager.handle.space = HandleSpace.LOCAL;
    }
}