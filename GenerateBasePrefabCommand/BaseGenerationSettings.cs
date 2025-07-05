namespace GenerateBasePrefabCommand;

public struct BaseGenerationSettings
{
    public bool IncludeSupports { get; }

    public BaseGenerationSettings(bool includeSupports)
    {
        IncludeSupports = includeSupports;
    }
}