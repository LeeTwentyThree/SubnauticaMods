namespace ModStructureHelperPlugin.UI.Menus;

public class StructureMainMenu : StructureHelperMenuBase
{
    public void OnButtonStopEditing()
    {
        ui.promptHandler.Ask("Unload? All unsaved changes will be lost.", new PromptChoice("Yes", StopEditingStructure), new PromptChoice("No"),  new PromptChoice("Idk"));
    }

    private void StopEditingStructure()
    {
        if (StructureInstance.main)
        {
            Destroy(StructureInstance.main.gameObject);
        }
    }
    
    public void OnButtonSaveChanges()
    {
        StructureInstance.TrySave();
    }
}