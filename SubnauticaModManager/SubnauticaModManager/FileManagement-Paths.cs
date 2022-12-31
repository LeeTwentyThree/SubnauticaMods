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
    private static string _pluginFolderPath;
    private static string _myAssetsFolderPath;
    private static string _subnauticaFolderPath;
    private static string _modDownloadFolderPath;
    private static string _disabledModsFolderPath;
    private static string _tempFolderPath;

    private const string kModDownloadFolderName = "ModDownloads";
    private const string kDisabledModsFolderName = "DisabledMods";
    private const string kTempFolderName = "Temp";
    private const string kAssetsFolderName = "Assets";

    public static string PluginFolder
    {
        get
        {
            if (string.IsNullOrEmpty(_pluginFolderPath))
            {
                _pluginFolderPath = Path.GetDirectoryName(Plugin.assembly.Location);
            }
            return _pluginFolderPath;
        }
    }

    public static string PluginAssetsFolder
    {
        get
        {
            if (string.IsNullOrEmpty(_myAssetsFolderPath))
            {
                _myAssetsFolderPath = Path.Combine(PluginFolder, kAssetsFolderName);
            }
            return _myAssetsFolderPath;
        }
    }

    public static string SubnauticaFolder
    {
        get
        {
            if (string.IsNullOrEmpty(_subnauticaFolderPath))
            {
                _subnauticaFolderPath = Path.Combine(PluginFolder, @"..\..\..\");
            }
            return _subnauticaFolderPath;
        }
    }

    public static string ModDownloadFolder
    {
        get
        {
            if (string.IsNullOrEmpty(_modDownloadFolderPath))
            {
                _modDownloadFolderPath = Path.Combine(SubnauticaFolder, kModDownloadFolderName);
                if (!Directory.Exists(_modDownloadFolderPath))
                {
                    Directory.CreateDirectory(_modDownloadFolderPath);
                }
            }
            return _modDownloadFolderPath;
        }
    }

    public static string DisableModsFolder
    {
        get
        {
            if (string.IsNullOrEmpty(_disabledModsFolderPath))
            {
                _disabledModsFolderPath = Path.Combine(SubnauticaFolder, kDisabledModsFolderName);
                if (!Directory.Exists(_disabledModsFolderPath))
                {
                    Directory.CreateDirectory(_disabledModsFolderPath);
                }
            }
            return _disabledModsFolderPath;
        }
    }

    public static string TempFolder
    {
        get
        {
            if (string.IsNullOrEmpty(_tempFolderPath))
            {
                _tempFolderPath = Path.Combine(PluginFolder, kTempFolderName);
                if (!Directory.Exists(_tempFolderPath))
                {
                    Directory.CreateDirectory(_tempFolderPath).Attributes = FileAttributes.Directory | FileAttributes.Hidden;
                }
            }
            return _tempFolderPath;
        }
    }

    public static string FromAssetsFolder(string localPath)
    {
        return Path.Combine(PluginAssetsFolder, localPath);
    }
}
