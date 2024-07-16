using ModStructureHelperPlugin.Handle.Handles;

namespace ModStructureHelperPlugin.Editing.Tools;

public class ScaleTool : ToolBase
{
    public override ToolType Type => ToolType.Scale;

    protected override void OnToolEnabled()
    {
        manager.handle.gameObject.SetActive(true);
        manager.handle.type = HandleType.SCALE;
    }

    protected override void OnToolDisabled()
    {
        manager.handle.gameObject.SetActive(false);
    }
}