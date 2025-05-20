using System.IO;
using SubnauticaEntityRipper.Data;
using SubnauticaEntityRipper.Data.Implementation;

namespace SubnauticaEntityRipper.Utility;

public static class CellsUtils
{
    public static BatchData[] GetAllCells(bool excludeBatchCells = false, bool excludeNonBatchCells = false)
    {
        string[] batchCells = null;
        string[] nonBatchCells = null;
        
        if (!excludeBatchCells)
        {
            batchCells = Directory.GetFiles(GamePaths.GetBatchObjectsDirectory());
        }

        if (!excludeNonBatchCells)
        {
            nonBatchCells = Directory.GetFiles(GamePaths.GetCellsCacheDirectory());
        }

        var cellsCount = (batchCells?.Length ?? 0) + (nonBatchCells?.Length ?? 0);
        var cells = new BatchData[cellsCount];

        if (batchCells != null)
        {
            for (var i = 0; i < batchCells.Length; i++)
            {
                cells[i] = new BatchData(batchCells[i], new CellMetadata(true));
            }
        }
        
        if (nonBatchCells != null)
        {
            var offset = batchCells?.Length ?? 0;
            for (var i = 0; i < nonBatchCells.Length; i++)
            {
                cells[i + offset] = new BatchData(nonBatchCells[i], new CellMetadata(false));
            }
        }
        
        return cells;
    }
}