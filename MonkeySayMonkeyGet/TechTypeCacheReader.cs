using System.Collections.Generic;
using System.IO;

namespace MonkeySayMonkeyGet;

public class TechTypeCacheReader
{
    public bool Read()
    {
        var modFolder = Path.GetDirectoryName(Plugin.Assembly.Location);
        if (InvalidPath(modFolder)) return false;
        var pluginsFolder = Path.GetDirectoryName(modFolder);
        if (InvalidPath(pluginsFolder)) return false;
        var bepInExFolder = Path.GetDirectoryName(pluginsFolder);
        if (InvalidPath(bepInExFolder)) return false;
        var smlCacheFolder = Path.Combine(pluginsFolder, "Modding Helper", "TechTypeCache");
        var nautilusCacheFolder = Path.Combine(bepInExFolder, "config", "Nautilus", "TechTypeCache");
        LoadCacheFolder(smlCacheFolder);
        LoadCacheFolder(nautilusCacheFolder);
        return entries.Count > 0;
    }

    private void LoadCacheFolder(string libraryModFolder)
    {
        if (InvalidPath(libraryModFolder)) return;
        var cacheFile = Path.Combine(libraryModFolder, "TechTypeCache.txt");
        if (InvalidPath(cacheFile)) return;
        var allText = File.ReadAllText(cacheFile);
        if (string.IsNullOrEmpty(allText)) return;
        entries.Clear();
        var splitted = allText.Split(new char[] { '\n' });
        foreach (var dual in splitted)
        {
            var array = dual.Split(new char[] { ':' });
            if (array.Length != 2)
            {
                continue;
            }
            var techTypeInt = int.Parse(array[1]);
            var techType = (TechType)techTypeInt;
            entries.Add(new Entry(array[0].ToLower(), techType));
        }
    }

    public readonly List<Entry> entries = new List<Entry>();

    public struct Entry
    {
        public string Name;
        public TechType TechType;

        public Entry(string name, TechType techType)
        {
            Name = name;
            TechType = techType;
        }
    }

    private bool InvalidPath(string path)
    {
        if (!Directory.Exists(path) && !File.Exists(path))
        {
            return true;
        }
        return false;
    }
}
