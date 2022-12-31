using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;

namespace SubnauticaModManager;

internal static partial class FileManagement
{
    public static void UnzipContents(string path, string intoDirectory, bool deleteZipFile)
    {
        if (string.IsNullOrEmpty(path) || string.IsNullOrEmpty(intoDirectory))
        {
            Plugin.Logger.LogError($"Empty path(s) detected while trying to unzip a file.");
            return;
        }
        if (!File.Exists(path) || !File.Exists(intoDirectory))
        {
            Plugin.Logger.LogError($"Invalid path(s) detected while trying to unzip a file.");
            return;
        }
        ZipFile.ExtractToDirectory(path, intoDirectory);
        if (deleteZipFile)
        {
            File.Delete(path);
        }
    }

    public static bool RestartRequired => _restartRequired;
    public static void RequireRestart() => _restartRequired = true;

    private static bool _restartRequired;
}
