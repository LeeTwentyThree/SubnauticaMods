namespace ModStructureHelperPlugin.UI.Menus;

public class StructureMainMenu : StructureHelperMenuBase
{
    public void OnButtonStopEditing()
    {
        ui.promptHandler.Ask("Stop editing the current structure? All unsaved changes will be lost on game reload.", new PromptChoice("Yes", StopEditingStructure), new PromptChoice("No"),  new PromptChoice("Idk"));
    }

    private void StopEditingStructure()
    {
        if (StructureInstance.Main)
        {
            Destroy(StructureInstance.Main.gameObject);
        }
    }
    
    public void OnButtonSaveChanges()
    {
        StructureInstance.TrySave();
    }
}