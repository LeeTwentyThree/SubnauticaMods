using UnityEngine;

namespace ModStructureHelperPlugin.EntityHandling;

public abstract class EntityPreviewBase
{
    public EntityData Entity { get; }

    public EntityPreviewBase(EntityData entity)
    {
        Entity = entity;
    }

    public abstract GameObject InstantiatePreview(EntityInstance entityInstance);

    protected void SetTransformFromEntityInstance(Transform transform, EntityInstance instance)
    {
        transform.position = instance.Position;
        transform.rotation = instance.Rotation;
        transform.localScale = instance.Scale;
    }
}
