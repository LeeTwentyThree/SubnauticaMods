using System.IO;

namespace ModStructureHelperPlugin.UI.Menus;

public class StructureMainMenu : StructureHelperMenuBase
{
    public void OnButtonStopEditing()
    {
        if (StructureInstance.Main)
        {
            ui.promptHandler.Ask("Stop editing the current structure? All unsaved changes will be lost on game reload.",
                new PromptChoice("Yes", StopEditingStructure), new PromptChoice("No"));
        }
        else
        {
            ui.promptHandler.Ask("There is no structure being edited currently!");
        }
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
        if (!StructureInstance.Main)
        {
            ui.promptHandler.Ask("There is no structure being edited currently!");
            return;
        }

        StructureInstance.TrySave();
    }

    public void ViewCurrentStructureFolder()
    {
        if (!StructureInstance.Main)
        {
            ui.promptHandler.Ask("There is no structure being edited currently!");
            return;
        }

        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
        {
            FileName = Path.GetDirectoryName(StructureInstance.Main.path),
            UseShellExecute = true,
            Verb = "open"
        });
    }

    public void OnButtonInstantiateCurrentStructure()
    {
        if (!StructureInstance.Main)
        {
            ui.promptHandler.Ask("There is no structure being edited currently!");
            return;
        }

        ui.promptHandler.Ask(
            "Would you like to spawn in the current structure? Only use this if you are IN THE AREA IT SPAWNS IN and it either currently does NOT exist in the world or your mods in any form or has been changed since you made this save. Otherwise, you risk corruption",
            new PromptChoice("Yes", InstantiateCurrentStructure), new PromptChoice("No"));
    }

    private void InstantiateCurrentStructure()
    {
        if (!StructureInstance.Main)
        {
            ui.promptHandler.Ask("There is no structure being edited currently!");
            return;
        }

        UWE.CoroutineHost.StartCoroutine(EntityUtility.SpawnEntitiesFromStructure(StructureInstance.Main.data));
    }

    public void OnButtonGoToStructureCenter()
    {
        if (!StructureInstance.Main)
        {
            ui.promptHandler.Ask("There is no structure being edited currently!");
            return;
        }
        Player.main.SetPosition(StructureInstance.Main.GetStructureCenterPosition());
    }
}