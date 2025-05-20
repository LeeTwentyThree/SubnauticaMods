namespace SubnauticaEntityRipper.Data;

public struct CellMetadata
{
    public bool HasBatchObjects { get; }

    public CellMetadata(bool hasBatchObjects)
    {
        HasBatchObjects = hasBatchObjects;
    }
}