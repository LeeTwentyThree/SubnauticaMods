using System.Collections.Generic;
using System.IO;
using SubnauticaEntityRipper.Data.Implementation;
using SubnauticaEntityRipper.Data.Interfaces;

namespace SubnauticaEntityRipper.Data.Exporters;

public class EntityCoreDataExporter : IEntityExporter
{
    public void ExportData(IBatchParser parser, IEnumerable<BatchData> inputCells, string outputFile)
    {
        using var writer = new StreamWriter(outputFile);

        foreach (var input in inputCells)
        {
            parser.SetCurrentBatch(input);
            
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