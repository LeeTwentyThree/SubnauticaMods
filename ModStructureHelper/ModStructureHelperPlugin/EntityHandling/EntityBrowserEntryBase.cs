using UnityEngine;

namespace ModStructureHelperPlugin.EntityHandling;

public abstract class EntityBrowserEntryBase
{
    public string Path { get; }
    public abstract string Name { get; }
    public abstract Sprite Sprite { get; }

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