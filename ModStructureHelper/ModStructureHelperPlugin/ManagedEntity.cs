using ModStructureFormat;

namespace ModStructureHelperPlugin;

public class ManagedEntity
{
    public Entity EntityData { get; }
    public EntityInstance EntityInstance { get; }

    public ManagedEntity(Entity entityData)
    {
        EntityData = entityData;
    }

    public ManagedEntity(EntityInstance entityInstance)
    {
        EntityInstance = entityInstance;
    }
}