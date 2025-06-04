using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace PdaUpgradeCards.Data;

public static class PdaMusicDatabase
{
    public static IReadOnlyList<PdaMusicEntry> GetAllMusic()
    {
        return AllMusic;
    }

    public static int GetTrackCount()
    {
        return AllMusic.Count;
    }
    
    private static List<PdaMusicEntry> AllMusic { get; } = new();
    private static Dictionary<string, PdaMusicEntry> MusicLookUp { get; } = new();

    public static IEnumerator RefreshMusicDatabase()
    {
        var files = Directory.GetFiles(CustomTracksDirectory);
        foreach (var file in files)
        {
            if (!MusicLookUp.ContainsKey(file))
            {
                yield return RegisterNewTrack(file);
            }
        }
    }

    private static IEnumerator RegisterNewTrack(string filePath)
    {
        var music = new PdaMusicCustom(filePath);
        var success = new TaskResult<bool>();
        yield return music.LoadAudio(success);
        if (success.value == false)
            yield break;
        var entry = new PdaMusicEntry(filePath, music, new FileInfo(filePath));
        MusicLookUp.Add(filePath, entry);
        AllMusic.Add(entry);
    }

    public static string CustomTracksDirectory
    {
        get
        {
            if (!string.IsNullOrEmpty(_customTracksDirectory) && Directory.Exists(_customTracksDirectory))
                return _customTracksDirectory;
            
            var modConfigPath = Path.Combine(BepInEx.Paths.ConfigPath, "PdaUpgradeCards");
            if (!Directory.Exists(modConfigPath))
            {
                Directory.CreateDirectory(modConfigPath);
            }
            
            _customTracksDirectory = Path.Combine(modConfigPath, "MusicTracks");
            if (!Directory.Exists(_customTracksDirectory))
            {
                Directory.CreateDirectory(_customTracksDirectory);
            }
            
            return _customTracksDirectory;
        }
    }

    private static string _customTracksDirectory;

    public class PdaMusicEntry
    {
        public PdaMusicEntry(string filePath, PdaMusic music, FileInfo knownFileInfo)
        {
            FilePath = filePath;
            Music = music;
            KnownFileInfo = knownFileInfo;
        }

        public string FilePath { get; }
        public PdaMusic Music { get; }
        public FileInfo KnownFileInfo { get; }
    }
}