namespace ModStructureHelperPlugin.EntityHandling;

public class EntityData
{
    public string Name { get; }
    public string ClassId { get; }
    public string Path { get; }
    public EntityPreviewBase Preview { get; private set; }

    public EntityData(string classId, string path)
    {
        ClassId = classId;
        Path = path;
        Name = PathUtils.GetFileNameWithoutExtension(path);
    }

    public void SetEntityPreview<T>(T preview) where T : EntityPreviewBase
    {
        Preview = preview;
    }
}
