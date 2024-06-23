using System.Collections.Generic;
using System.Linq;
using ModStructureHelperPlugin.UI;
using UnityEngine;

namespace ModStructureHelperPlugin.EntityHandling;

public class EntityBrowserFolder : EntityBrowserEntryBase
{
    private string _name;

    public EntityBrowserFolder(string path) : base(path)
    {
        _name = System.IO.Path.GetFileName(path);
        if (string.IsNullOrEmpty(_name))
        {
            _name = "Root";
        }
    }

    public override string Name => _name;

    public override Sprite Sprite => EntityDatabase.main.folderSprite;

    public List<EntityBrowserEntryBase> Subentries { get; private set; } = new List<EntityBrowserEntryBase>();

    public override void OnInteract()
    {
        UIEntityWindow.main.RenderFolder(this);
    }

    public void SortSubentries()
    {
        var sortedList = new List<EntityBrowserEntryBase>();
        sortedList.AddRange(Subentries.Where(s => s is EntityBrowserFolder).OrderBy(s => s.Name).ToList());
        sortedList.AddRange(Subentries.Where(s => !(s is EntityBrowserFolder)).OrderBy(s => s.Name).ToList());
        Subentries = sortedList;
    }

    public List<EntityBrowserFolder> GetParentFolders()
    {
        var list = new List<EntityBrowserFolder>();
        string workingPath = Path;
        while (!string.IsNullOrEmpty(workingPath))
        {
            var indexOfLastSlash = workingPath.LastIndexOf('/');
            if (indexOfLastSlash < 0) break;
            workingPath = workingPath.Substring(0, indexOfLastSlash);
            if (EntityDatabase.main.TryGetFolder(workingPath, out var folder))
            {
                list.Add(folder);
            }
            else
            {
                ErrorMessage.AddMessage($"Failed to find folder with path '{workingPath}'.");
            }
        }
        list.Reverse();
        return list;
    }

    public override void OnConstructButton(GameObject button)
    {
        // disable the paint button. the buttons use object pooling, so it's important that you disable this buttons in case it was enabled at another point.
        button.transform.GetChild(1).gameObject.SetActive(false);
    }
}
