using System.Collections.Generic;
using SubnauticaEntityRipper.Data.Implementation;

namespace SubnauticaEntityRipper.Data.Interfaces;

public interface IEntityExporter
{
    void ExportData(IBatchParser parser, IEnumerable<BatchData> inputCells, string outputFile);
}