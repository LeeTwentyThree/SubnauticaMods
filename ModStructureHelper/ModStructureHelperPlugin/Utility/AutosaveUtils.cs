using System;
using System.IO;
using System.Linq;

namespace ModStructureHelperPlugin.Utility;

public class AutosaveUtils
{
    private const string AutosaveFolderName = "Autosaves";
    
    public static void ClearOldAutoSaves()
    {
        var folder = GetAutoSaveFolderPath();
        
        var files = new DirectoryInfo(folder)
            .GetFiles("*.structure")
            .OrderByDescending(f => f.CreationTimeUtc)
            .ToList();

        if (files.Count < Plugin.ModConfig.MaxAutosaveFiles) return;
            
        foreach (var oldFile in files.Skip(Plugin.ModConfig.MaxAutosaveFiles - 1))
        {
            try
            {
                oldFile.Delete();
            }
            catch (Exception e)
            {
                Plugin.Logger.LogWarning("Exception thrown while clearing autosaves: " + e);
            }
        }
    }
    
    public static string GetAutoSaveFolderPath()
    {
        var folder = Path.Combine(Path.GetDirectoryName(Plugin.Assembly.Location), AutosaveFolderName);
        if (!Directory.Exists(folder))
            Directory.CreateDirectory(folder);
        return folder;
    }
}