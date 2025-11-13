using UnityEngine;

namespace ModStructureHelperPlugin.EntityHandling.Icons;

public class EntityIconBasic : EntityIcon
{
    public override Sprite Sprite { get; }
    public override Color ColorMultiplier { get; }

    public EntityIconBasic(Sprite sprite, Color color)
    {
        Sprite = sprite;
        ColorMultiplier = color;
    }

    public EntityIconBasic(Sprite sprite) : this(sprite, Color.white)
    {
    }
}