using System;
using System.Collections.Generic;
using System.IO;
using SubnauticaEntityRipper.Data.Interfaces;
using UWE;

namespace SubnauticaEntityRipper.Data.Implementation;

public class StandardBatchParser : IBatchParser, IDisposable
{
    public BatchData CurrentBatch { get; private set; }

    private readonly bool _verboseLogging;

    private Stream _inputStream;
    private PooledObject<ProtobufSerializer> _serializer = ProtobufSerializerPool.GetProxy();

    private StandardCellParser _cellParser;

    private readonly byte[] _buffer = new byte[32768];
    
    public StandardBatchParser(bool verboseLogging)
    {
        _verboseLogging = verboseLogging;
    }

    public void SetCurrentBatch(BatchData batch)
    {
        CurrentBatch = batch;

        CleanUp();

        _inputStream = new FileStream(batch.FilePath, FileMode.Open, FileAccess.Read);
        
        _cellParser = new StandardCellParser(_serializer, _inputStream, _verboseLogging);
    }

    public IEnumerable<ICellParser> ReadCells()
    {
        if (CurrentBatch.Metadata.HasBatchObjects)
        {
            var streamHeader = ProtobufSerializer.streamHeaderPool.Get();
            _serializer.Value.Deserialize(_inputStream, streamHeader, _verboseLogging);
            ProtobufSerializer.streamHeaderPool.Return(streamHeader);
            throw new NotImplementedException();
        }
        else
        {
            var cellsFileHeader = new CellManager.CellsFileHeader();
            _serializer.Value.Deserialize(_inputStream, cellsFileHeader, _verboseLogging);
            var cellHeaderEx = new CellManager.CellHeaderEx();
            for (var i = 0; i < cellsFileHeader.numCells; i++)
            {
                _serializer.Value.Deserialize(_inputStream, cellHeaderEx, verbose: false);

                // int size = _inputStream.Read(_buffer, 0, cellHeaderEx.dataLength);
                
                if (cellHeaderEx.dataLength > 0)
                {
                    yield return _cellParser;
                }
                
                _inputStream.Seek(cellHeaderEx.waiterDataLength, SeekOrigin.Current);
                _inputStream.Seek(cellHeaderEx.legacyDataLength, SeekOrigin.Current);
                
                /*
                // Load legacy data
                _disposableSerialDataReader.ReadFromStream(_inputStream, cellHeaderEx.legacyDataLength);
                // Load waiter data
                _disposableSerialDataReader.ReadFromStream(_inputStream, cellHeaderEx.waiterDataLength);
                */
            }
        }

        CleanUp();
    }

    private void CleanUp()
    {
        _inputStream?.Dispose();
        _inputStream = null;
    }
    
    public void Dispose()
    {
        CleanUp();
    }
}