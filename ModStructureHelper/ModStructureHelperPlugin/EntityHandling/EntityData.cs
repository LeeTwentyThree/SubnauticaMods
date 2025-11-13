namespace ModStructureHelperPlugin.EntityHandling;

public class EntityData
{
    public string Name { get; }
    public string TranslatedName { get; }
    public string ClassId { get; }
    public string Path { get; }
    public string SourceName { get; }

    public EntityData(string classId, string path, string sourceName, string translatedName = default)
    {
        ClassId = classId;
        Path = path;
        Name = PathUtils.GetFileNameWithoutExtension(path);
        SourceName = sourceName;
        TranslatedName = translatedName ?? Name;
    }
}
