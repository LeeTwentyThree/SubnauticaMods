namespace SubnauticaEntityRipper.Data.Implementation;

public struct BatchData
{
    public string FilePath { get; }
    public CellMetadata Metadata { get; }

    public BatchData(string filePath, CellMetadata metadata)
    {
        FilePath = filePath;
        Metadata = metadata;
    }
}