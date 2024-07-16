using ModStructureHelperPlugin.Editing.Managers;
using ModStructureHelperPlugin.StructureHandling;
using ModStructureHelperPlugin.UI;

namespace ModStructureHelperPlugin.Editing.Tools;

public class DeleteTool : ToolBase
{
    public override ToolType Type => ToolType.Delete;

    public override bool MultitaskTool => true;

    protected override void OnToolEnabled()
    {
        if (SelectionManager.NumberOfSelectedObjects > 1)
        {
            StructureHelperUI.main.promptHandler.Ask($"Are you sure you want to delete all {SelectionManager.NumberOfSelectedObjects} selected objects?",
                new PromptChoice("Yes", DestroyAllSelectedObjects),
                new PromptChoice("No"));
        }
        else
        {
            DestroyAllSelectedObjects();
        }
        DisableTool();
    }

    private void DestroyAllSelectedObjects()
    {
        foreach (var selected in SelectionManager.SelectedObjects)
        {
            if (selected != null)
            {
                StructureInstance.Main.DeleteEntity(selected, true);
            }
        }
        SelectionManager.ClearSelection();
    }

    protected override void OnToolDisabled() { }
}