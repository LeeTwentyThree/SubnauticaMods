using System.Collections.Generic;
using System.IO;
using System.Linq;
using SubnauticaEntityRipper.Data.Implementation;
using SubnauticaEntityRipper.Data.Interfaces;
using UWE;

namespace SubnauticaEntityRipper.Data.Exporters;

public class CountingExporter : IEntityExporter
{
    public void ExportData(IBatchParser parser, IEnumerable<BatchData> inputCells, string outputFile)
    {
        Dictionary<string, CountedEntity> dictionary = new();

        foreach (var input in inputCells)
        {
            parser.SetCurrentBatch(input);

            foreach (var cell in parser.ReadCells())
            {
                foreach (var entity in cell.ReadEntities())
                {
                    if (!dictionary.TryGetValue(entity.CoreData.ClassId, out var count))
                    {
                        count = new CountedEntity(entity.CoreData.ClassId);
                        dictionary.Add(entity.CoreData.ClassId, count);
                    }

                    count.AddOne();
                }
            }
        }

        using var writer = new StreamWriter(outputFile);
        var countedList = dictionary.Values.OrderBy(c => c.Count);
        foreach (var entry in countedList)
        {
            var friendlyName = "UNKNOWN";
            if (PrefabDatabase.TryGetPrefabFilename(entry.ClassId, out var fileName))
            {
                friendlyName = fileName.Split('/').Last().Split('.').First();
            }

            writer.WriteLine(entry.Count.ToString().PadRight(8) + " - " + friendlyName.PadRight(20) + " - " + entry.ClassId);
        }
    }

    private class CountedEntity
    {
        public string ClassId { get; }
        public int Count { get; private set; }

        public int AddOne() => Count++;

        public CountedEntity(string classId)
        {
            ClassId = classId;
            Count = 0;
        }
    }
}