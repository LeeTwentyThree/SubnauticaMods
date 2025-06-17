using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using UnityEngine;

namespace PdaUpgradeChips.Data;

public static class PdaMusicDatabase
{
    private const string ConfigFolderName = "PdaUpgradeChips";
    private const string MusicTracksFolderName = "MusicTracks";
    private const string ModMusicConfigFileName = "ModMusic.config";

    public static IEnumerable<PdaMusic> GetAllMusic()
    {
        return AllMusic.Select(e => e.Music);
    }

    public static int GetTrackCount()
    {
        return AllMusic.Count;
    }

    private static List<PdaMusicEntry> AllMusic { get; } = new();
    private static Dictionary<string, PdaMusicEntry> MusicLookUp { get; } = new();
    private static bool _initializedFirstLoad;

    private static bool _busyRefreshingDatabase;

    public delegate void OnMusicDatabaseChangedDelegate();

    public static event OnMusicDatabaseChangedDelegate OnMusicDatabaseChanged;

    public static IEnumerator RefreshMusicDatabase()
    {
        if (_busyRefreshingDatabase)
        {
            Plugin.Logger.LogWarning("Database is already busy refreshing!");
            yield break;
        }

        _busyRefreshingDatabase = true;

        if (!_initializedFirstLoad)
        {
            try
            {
                LoadModMusic();
            }
            catch (Exception e)
            {
                Plugin.Logger.LogError("Exception thrown while loading mod music: " + e);
            }
        }

        bool changed = !_initializedFirstLoad;
        var files = Directory.GetFiles(CustomTracksDirectory);
        foreach (var file in files)
        {
            if (!MusicLookUp.TryGetValue(file, out var music))
            {
                yield return RegisterNewTrack(file);
                changed = true;
            }
            else if (music.KnownFileInfo.Length != new FileInfo(file).Length)
            {
                yield return UpdateExistingTrack(file);
                changed = true;
            }

            yield return null;
        }

        try
        {
            for (int i = 0; i < AllMusic.Count; i++)
            {
                var track = AllMusic[i];
                if (!track.IsModMusic && !File.Exists(track.FilePath))
                {
                    MusicLookUp.Remove(track.FilePath);
                    AllMusic.Remove(track);
                    changed = true;
                    i--;
                }
            }
        }
        catch (Exception e)
        {
            Plugin.Logger.LogError("Exception thrown while trying to remove unused music: " + e);
        }

        if (changed)
        {
            OnMusicDatabaseChanged?.Invoke();
        }

        _initializedFirstLoad = true;
        _busyRefreshingDatabase = false;
    }

    private static ModMusicData[] GetModMusicDataFromJson()
    {
        return JsonConvert.DeserializeObject<ModMusicData[]>(File.ReadAllText(ModMusicFilePath));
    }

    private static void LoadModMusic()
    {
        ModMusicData[] musicData;
        try
        {
            musicData = GetModMusicDataFromJson();
        }
        catch (Exception e)
        {
            Plugin.Logger.LogError("Exception thrown while trying to load mod music JSON data: " + e);
            return;
        }

        var validModsByGuid = new Dictionary<string, List<ModMusicData>>();
        foreach (var entry in musicData)
        {
            if (!BepInEx.Bootstrap.Chainloader.PluginInfos.Keys.Contains(entry.ModGuid))
            {
                continue;
            }

            if (!validModsByGuid.TryGetValue(entry.ModGuid, out var list))
            {
                list = new List<ModMusicData>();
                validModsByGuid[entry.ModGuid] = list;
            }

            list.Add(entry);
        }

        foreach (var mod in validModsByGuid)
        {
            try
            {
                var pluginInfo = BepInEx.Bootstrap.Chainloader.PluginInfos[mod.Key];

                var assetBundleName = mod.Value[0].AssetBundleName;

                AssetBundle bundle = null;

                var pluginInstance = pluginInfo.Instance;
                var assetBundleMembers = GetMembersOfType(pluginInstance.GetType(), typeof(AssetBundle));

                foreach (var member in assetBundleMembers)
                {
                    AssetBundle bundleCandidate = member switch
                    {
                        PropertyInfo propertyInfo => propertyInfo.GetValue(null) as AssetBundle,
                        FieldInfo field => field.GetValue(null) as AssetBundle,
                        _ => null
                    };
                    if (bundleCandidate != null && bundleCandidate.name == assetBundleName)
                    {
                        bundle = bundleCandidate;
                    }
                }

                if (bundle == null)
                {
                    throw new Exception("Failed to find AssetBundle from plugins class!");
                }

                foreach (var sound in mod.Value)
                {
                    try
                    {
                        var audioClip = bundle.LoadAsset<AudioClip>(sound.AudioClipName);
                        AllMusic.Add(new PdaMusicEntry(new PdaMusicAudioClip(mod.Key + audioClip.name,
                            sound.DisplayName, audioClip, sound.Volume)));
                    }
                    catch (Exception e)
                    {
                        Plugin.Logger.LogError(
                            $"Failed to load AudioClip '{sound.AudioClipName}' from bundle '{assetBundleName}': {e.Message}");
                    }
                }
            }
            catch (Exception e)
            {
                Plugin.Logger.LogError($"Failed to load music for mod '{mod.Key}': " + e);
            }
        }
    }

    private static IEnumerable<MemberInfo> GetMembersOfType(Type targetType, Type memberType)
    {
        var fields = targetType.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static)
            .Where(f => f.FieldType == memberType)
            .Cast<MemberInfo>();

        var properties = targetType.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static)
            .Where(p => p.PropertyType == memberType)
            .Cast<MemberInfo>();

        return fields.Concat(properties);
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

    private static IEnumerator UpdateExistingTrack(string filePath)
    {
        if (!MusicLookUp.TryGetValue(filePath, out var musicEntry))
        {
            Plugin.Logger.LogWarning("Somehow failed to find music track matching path " + filePath);
            yield break;
        }

        if (musicEntry.Music is not PdaMusicCustom customMusic)
        {
            Plugin.Logger.LogWarning($"Music track '{musicEntry.Music.GetTrackName()}' is of the incorrect type!");
            yield break;
        }

        var success = new TaskResult<bool>();
        yield return customMusic.LoadAudio(success);
        if (success.value == false)
            Plugin.Logger.LogWarning("Failed to update music track at path " + filePath);
    }

    private static string ConfigFolderDirectory
    {
        get
        {
            if (!string.IsNullOrEmpty(_configFolderDirectory))
            {
                return _configFolderDirectory;
            }

            _configFolderDirectory = Path.Combine(BepInEx.Paths.ConfigPath, ConfigFolderName);
            if (!Directory.Exists(_configFolderDirectory))
            {
                Directory.CreateDirectory(_configFolderDirectory);
            }

            return _configFolderDirectory;
        }
    }

    public static string CustomTracksDirectory
    {
        get
        {
            if (!string.IsNullOrEmpty(_customTracksDirectory) && Directory.Exists(_customTracksDirectory))
                return _customTracksDirectory;

            _customTracksDirectory = Path.Combine(ConfigFolderDirectory, MusicTracksFolderName);
            if (!Directory.Exists(_customTracksDirectory))
            {
                Directory.CreateDirectory(_customTracksDirectory);
            }

            return _customTracksDirectory;
        }
    }

    public static string ModMusicFilePath
    {
        get
        {
            if (!string.IsNullOrEmpty(_modMusicFilePath)) return _modMusicFilePath;

            _modMusicFilePath = Path.Combine(ConfigFolderDirectory, ModMusicConfigFileName);
            return _modMusicFilePath;
        }
    }

    private static string _configFolderDirectory;
    private static string _customTracksDirectory;
    private static string _modMusicFilePath;

    public class PdaMusicEntry
    {
        public PdaMusicEntry(string filePath, PdaMusic music, FileInfo knownFileInfo)
        {
            FilePath = filePath;
            Music = music;
            KnownFileInfo = knownFileInfo;
            IsModMusic = false;
        }

        public PdaMusicEntry(PdaMusic music)
        {
            Music = music;
            IsModMusic = true;
        }

        public bool IsModMusic { get; }
        public string FilePath { get; }
        public PdaMusic Music { get; }
        public FileInfo KnownFileInfo { get; }
    }

    [Serializable]
    private struct ModMusicData
    {
        public string ModGuid { get; set; }
        public string AssetBundleName { get; set; }
        public string AudioClipName { get; set; }
        public string DisplayName { get; set; }
        public float Volume { get; set; }

        [JsonConstructor]
        public ModMusicData()
        {
            
        }
        
        public ModMusicData(string modGuid, string assetBundleName, string audioClipName, string displayName,
            float volume)
        {
            ModGuid = modGuid;
            AssetBundleName = assetBundleName;
            AudioClipName = audioClipName;
            DisplayName = displayName;
            Volume = volume;
        }
    }
}