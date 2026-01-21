using System;
using System.Collections.Generic;
using System.IO;
using ModStructureFormatV2;
using ModStructureHelperPlugin.UI.Buttons;
using ModStructureHelperPlugin.Utility;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ModStructureHelperPlugin.UI.Menus;

public class StructureLoadingMenu : StructureHelperMenuBase
{
    public RectTransform buttonsParent;
    public GameObject buttonTemplate;
    public TextMeshProUGUI resultsCountText;
    public InputField filter;
    
    private List<Entry> _entries = new();
    
    public void UpdateList()
    {
        foreach (Transform child in buttonsParent.transform)
        {
            Destroy(child.gameObject);
        }
        _entries.Clear();
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
            _entries.Add(new Entry(path, button));
        }
        
        UpdateFilter();
    }

    private void OnEnable()
    {
        UpdateList();
    }

    public void OnTextFieldInputChanged(string _)
    {
        UpdateFilter();
    }

    private void UpdateFilter()
    {
        int activeCount = 0;

        if (_entries.Count > 0)
        {
            var keywords = filter.text.Trim().SplitByChar(' ');

            _entries.ForEach(e =>
            {
                if (e.Button == null)
                    return;
                var active = DoesPathMatchFilter(e.FileName, keywords);
                if (active) activeCount++;
                e.Button.SetActive(active);
            });    
        }
        
        resultsCountText.text = GetResultsCountText(activeCount);
    }

    private string GetResultsCountText(int amount)
    {
        return amount == 1 ? "Displaying 1 mod structure" : $"Displaying {amount} mod structures";
    }

    private bool DoesPathMatchFilter(string path, string[] filterKeywords)
    {
        var pathLower = path.ToLower();
        if (filterKeywords == null || filterKeywords.Length == 0) return true;
        
        foreach (var keyword in filterKeywords)
        {
            if (pathLower.Contains(keyword.ToLower()))
            {
                return true;
            }
        }

        return false;
    }

    private class Entry
    {
        public Entry(string fileName, GameObject button)
        {
            FileName = fileName;
            Button = button;
        }

        public GameObject Button { get; }
        public string FileName { get; }
    }
}