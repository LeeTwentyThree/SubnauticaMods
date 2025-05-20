using SubnauticaEntityRipper.Data.Interfaces;

namespace SubnauticaEntityRipper.Data.Implementation;

public class EntityDefinition : IEntityDefinition
{
    public CoreEntityData CoreData { get; }

    public EntityDefinition(CoreEntityData coreData)
    {
        CoreData = coreData;
    }

    public override string ToString()
    {
        return "{" + CoreData + "}";
    }
}