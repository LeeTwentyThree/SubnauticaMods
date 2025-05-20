using System.Collections.Generic;
using System.IO;
using SubnauticaEntityRipper.Data.Implementation;
using SubnauticaEntityRipper.Data.Interfaces;

namespace SubnauticaEntityRipper.Data.Exporters;

public class EntityCoreDataExporter : IEntityExporter
{
    public void ExportData(IBatchParser parser, IEnumerable<BatchData> inputBatches, string outputFile)
    {
        using var writer = new StreamWriter(outputFile);

        foreach (var batch in inputBatches)
        {
            parser.SetCurrentBatch(batch);
            
            foreach (var cell in parser.ReadCells())
            {
                foreach (var entity in cell.ReadEntities())
                {
                    writer.WriteLine(entity.ToString());
                }
            }
        }
    }
}