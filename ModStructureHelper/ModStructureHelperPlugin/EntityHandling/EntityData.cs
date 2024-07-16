namespace ModStructureHelperPlugin.EntityHandling;

public class EntityData
{
    public string Name { get; }
    public string ClassId { get; }
    public string Path { get; }

    public EntityData(string classId, string path)
    {
        ClassId = classId;
        Path = path;
        Name = PathUtils.GetFileNameWithoutExtension(path);
    }
}
