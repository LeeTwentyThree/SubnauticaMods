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
    public static void UnzipFile(string path, string intoDirectory)
    {
        ZipFile.ExtractToDirectory(path, intoDirectory);
    }

    public static bool RestartRequired => _restartRequired;
    public static void RequireRestart() => _restartRequired = true;

    private static bool _restartRequired;
}
