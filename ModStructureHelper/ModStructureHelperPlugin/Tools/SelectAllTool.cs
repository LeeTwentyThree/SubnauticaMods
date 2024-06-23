using System.Linq;

namespace ModStructureHelperPlugin.Tools;

public class SelectAllTool : ToolBase
{
    public override ToolType Type => ToolType.SelectAll;
    public override bool MultitaskTool => true;
    public override bool PairedWithControl => true;

    protected override void OnToolEnabled()
    {
        if (StructureInstance.Main == null)
        {
            ErrorMessage.AddMessage("Must be editing a structure to select all!");
            return;
        }
        
        var loadedEntityCount = StructureInstance.Main.GetLoadedEntityCount();
        var deselectAll = SelectionManager.SelectedObjects.Count() == loadedEntityCount;
        SelectionManager.ClearSelection();
        if (!deselectAll)
        {
            foreach (var entity in StructureInstance.Main.GetAllManagedEntities())
            {
                if (entity.EntityInstance == null) continue;
                SelectionManager.AddSelectedObject(entity.EntityInstance.gameObject);
            }
            ErrorMessage.AddMessage($"Selected all {loadedEntityCount} entities.");
        }
        else
        {
            ErrorMessage.AddMessage("Deselected all entities.");
        }
        
        DisableTool();
    }

    protected override void OnToolDisabled()
    {
        
    }
}