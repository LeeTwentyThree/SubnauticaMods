using System.Collections.Generic;

namespace SubnauticaEntityRipper.Data.Interfaces;

public interface ICellParser
{
    IEnumerable<IEntityDefinition> ReadEntities();
}