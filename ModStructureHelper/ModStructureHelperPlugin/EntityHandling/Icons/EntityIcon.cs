using UnityEngine;

namespace ModStructureHelperPlugin.EntityHandling.Icons;

public abstract class EntityIcon
{
    public abstract Sprite Sprite { get; }
    public abstract Color ColorMultiplier { get; }
}