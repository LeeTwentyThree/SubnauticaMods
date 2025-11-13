using ModStructureHelperPlugin.EntityHandling.Icons;
using UnityEngine;

namespace ModStructureHelperPlugin.EntityHandling;

public abstract class EntityBrowserEntryBase
{
    public string Path { get; }
    public abstract string Name { get; }
    public abstract EntityIcon Icon { get; }
    public abstract string GetTooltip();

    public EntityBrowserEntryBase(string path)
    {
        Path = path;
    }

    public abstract void OnInteract();

    public string GetParentFolder()
    {
        return PathUtils.GetParentDirectory(Path);
    }
    
    public virtual void OnConstructButton(GameObject button) {}
}