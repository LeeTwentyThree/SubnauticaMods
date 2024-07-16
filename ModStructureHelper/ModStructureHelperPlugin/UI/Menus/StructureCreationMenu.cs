using System;
using System.IO;
using System.Linq;
using ModStructureFormat;
using ModStructureHelperPlugin.StructureHandling;
using TMPro;

namespace ModStructureHelperPlugin.UI.Menus;

public class StructureCreationMenu : StructureHelperMenuBase
{
    public TMP_InputField modInputField;
    public TMP_InputField nameInputField;

    public void OnCreateButtonPressed()
    {
        if (!TryGetMod(out var modData))
        {
            ErrorMessage.AddMessage($"Failed to find mod with folder name '{modInputField.text}.' Are you sure this mod exists in your Subnautica/BepInEx/plugins folder?");
            return;
        }
        ErrorMessage.AddMessage($"Creating structure '{nameInputField.text}' for mod '{modData.Metadata.Name}.'");
        StructureInstance.CreateNewInstance(new Structure(Array.Empty<Entity>()), GetStructurePathForMod(modData.Location, nameInputField.text));
        ui.SetMenuActive(MenuType.Editing);
    }

    private static string GetStructurePathForMod(string modAssemblyLocation, string structureName)
    {
        var modFolder = Path.GetDirectoryName(modAssemblyLocation);
        var structuresFolder = Path.Combine(modFolder, "Structures");
        if (!Directory.Exists(structuresFolder))
        {
            Directory.CreateDirectory(structuresFolder);
        }

        return Path.Combine(structuresFolder, $"{structureName}.structure");
    }

    private bool TryGetMod(out BepInEx.PluginInfo modData)
    {
        var modFolder = Path.Combine(BepInEx.Paths.PluginPath, modInputField.text);
        if (!Directory.Exists(modFolder))
        {
            modData = null;
            return false;
        }

        modData = BepInEx.Bootstrap.Chainloader.PluginInfos.Values.FirstOrDefault(info => info.Location.ToLower().StartsWith(modFolder.ToLower()));
        return modData != null;
    }
}