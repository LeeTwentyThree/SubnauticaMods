using System;
using System.IO;

namespace SubnauticaEntityRipper.Utility;

public static class DataExportFolder
{
    private const string FolderName = "EntityRipper";

    private static bool _createdFolder;
    private static string _cachedPath;

    public static string GetFolderPath()
    {
        if (_createdFolder) return _cachedPath;

        _cachedPath = Path.Combine(BepInEx.Paths.GameRootPath, FolderName);
        Directory.CreateDirectory(_cachedPath);
        _createdFolder = true;
        return _cachedPath;
    }

    public static string GetQuickPathForNewExport(string fileNamePrefix)
    {
        return Path.Combine(GetFolderPath(), fileNamePrefix + "-" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".txt");
    }
}