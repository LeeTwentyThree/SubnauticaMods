using RuntimeHandle;

namespace ModStructureHelperPlugin.Tools;

public class TranslateTool : ToolBase
{
    public override ToolType Type => ToolType.Translate;

    protected override void OnToolEnabled()
    {
        // manager.handle.enabled = true;
        manager.handle.type = HandleType.POSITION;
    }

    protected override void OnToolDisabled()
    {
        // manager.handle.enabled = false;
    }
}