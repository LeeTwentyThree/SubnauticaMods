using ModStructureHelperPlugin.Handle.Handles;

namespace ModStructureHelperPlugin.Editing.Tools;

public class RotateTool : ToolBase
{
    public override ToolType Type => ToolType.Rotate;

    protected override void OnToolEnabled()
    {
        manager.handle.gameObject.SetActive(true);
        manager.handle.type = HandleType.ROTATION;
    }

    protected override void OnToolDisabled()
    {
        manager.handle.gameObject.SetActive(false);
    }
}