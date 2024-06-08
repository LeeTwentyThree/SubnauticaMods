using ModStructureHelperPlugin.Mono;
using UnityEngine;

namespace ModStructureHelperPlugin.EntityHandling;

public class EntityBrowserEntity : EntityBrowserEntryBase
{
    public EntityData EntityData { get; }
    public override string Name => EntityData.Name;

    public override Sprite Sprite => IconGenerator.TryGetIcon(EntityData.ClassId, out var sprite) ? sprite : EntityDatabase.main.defaultEntitySprite;

    public EntityBrowserEntity(string path, EntityData entity) : base(path)
    {
        EntityData = entity;
    }

    public override void OnInteract()
    {
        ErrorMessage.AddMessage($"Failed to spawn entity by Class ID '{EntityData.ClassId}' (behavior not implemented yet!");
    }
}