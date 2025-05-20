using System.Collections.Generic;
using SubnauticaEntityRipper.Data.Implementation;

namespace SubnauticaEntityRipper.Data.Interfaces;

public interface IBatchParser
{
    BatchData CurrentBatch { get; }
    
    void SetCurrentBatch(BatchData batch);
    
    IEnumerable<ICellParser> ReadCells();
}