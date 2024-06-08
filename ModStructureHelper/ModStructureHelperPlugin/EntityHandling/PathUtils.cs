namespace ModStructureHelperPlugin.EntityHandling;

// Similar to System.IO features but designed for an enclosed browser system
public static class PathUtils
{
    public static string GetParentDirectory(string path)
    {
        var indexOf = path.LastIndexOf("/");
        if (indexOf == -1)
        {
            return string.Empty;
        }
        return path.Substring(0, indexOf);
    }

    public static string GetFileName(string path)
    {
        var indexOfSlash = path.LastIndexOf("/");
        if (indexOfSlash == -1)
        {
            return string.Empty;
        }
        return path.Substring(indexOfSlash + 1);
    }

    public static string GetFileNameWithoutExtension(string path)
    {
        var fileName = GetFileName(path);
        return fileName.Split('.')[0];
    }
}