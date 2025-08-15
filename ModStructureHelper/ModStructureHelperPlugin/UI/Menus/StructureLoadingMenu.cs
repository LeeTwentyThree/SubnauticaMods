using System;
using System.IO;
using ModStructureFormat;
using ModStructureHelperPlugin.UI.Buttons;
using ModStructureHelperPlugin.Utility;
using UnityEngine;

namespace ModStructureHelperPlugin.UI.Menus;

public class StructureLoadingMenu : StructureHelperMenuBase
{
    public RectTransform buttonsParent;
    public GameObject buttonTemplate;
    
    public void UpdateList()
    {
        foreach (Transform child in buttonsParent.transform)
        {
            Destroy(child.gameObject);
        }
        var files = Directory.GetFiles(BepInEx.Paths.PluginPath, "*.structure", SearchOption.AllDirectories);
        var autosaveDirectory = Path.GetFullPath(AutosaveUtils.GetAutoSaveFolderPath());
        foreach (var path in files)
        {
            if (string.Equals(Path.GetFullPath(Path.GetDirectoryName(path)), autosaveDirectory, StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }
            var button = Instantiate(buttonTemplate, buttonsParent);
            button.GetComponent<EditStructureButton>().SetStructure(Structure.LoadFromFile(path), Path.GetFileNameWithoutExtension(path), path);
            button.SetActive(true);
        }
    }

    private void OnEnable()
    {
        UpdateList();
    }
}