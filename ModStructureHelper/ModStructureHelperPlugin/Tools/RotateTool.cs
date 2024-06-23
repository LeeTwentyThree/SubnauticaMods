using RuntimeHandle;

namespace ModStructureHelperPlugin.Tools;

public class RotateTool : ToolBase
{
    public override ToolType Type => ToolType.Rotate;

    protected override void OnToolEnabled()
    {
        // manager.handle.enabled = true;
        manager.handle.type = HandleType.ROTATION;
    }

    protected override void OnToolDisabled()
    {
        manager.handle.gameObject.SetActive(false);
    }
}