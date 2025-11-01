using ModStructureHelperPlugin.UI;
using ModStructureHelperPlugin.Utility;

namespace ModStructureHelperPlugin.Editing.Tools;

public class ObjectPickerTool : ToolBase
{
    public override ToolType Type => ToolType.ObjectPicker;
    public override bool IncompatibleWithSelectTool => true;

    protected override void OnToolEnabled()
    {

    }

    protected override void OnToolDisabled()
    {

    }

    protected override string GetBindString()
    {
        var quickBindString = GameInput.FormatButton(StructureHelperInput.QuickPickEntity);
        return $"{base.GetBindString()} (quick: {quickBindString})";
    }

    public override void UpdateTool()
    {
        if (GameInput.GetButtonDown(StructureHelperInput.Interact) && !StructureHelperUI.main.IsCursorHoveringOverExternalWindows)
        {
            ObjectPickingUtils.PickObjectAtCursor();
        }   
    }

    private void Update()
    {
        if (GameInput.GetButtonDown(StructureHelperInput.QuickPickEntity) && !StructureHelperUI.main.IsCursorHoveringOverExternalWindows)
        {
            ObjectPickingUtils.PickObjectAtCursor();
        }   
    }
}