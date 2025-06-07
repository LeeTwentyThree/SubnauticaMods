using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace PdaUpgradeCards.Data;

public static class PdaMusicDatabase
{
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

    private static ModMusicData[] _modMusic =
    {
        new("com.aci.hydra", "hydra_assets", "CreatureAttack_Quiet", "CotV - Creature Attack", 1.3f),
        new("com.aci.hydra", "hydra_assets", "abandon hope extended_Quiet", "CotV - Abandon Hope", 1.3f),
        new("com.aci.hydra", "hydra_assets", "Hydra_Sting", "CotV - The Hydra", 0.7f),
        new("com.lee23.theredplague", "theredplagueaudio", "infectedbiomedemo1", "Red Plague - Infected Zone", 1.3f),
        new("com.lee23.theredplague", "theredplagueaudio", "skyisland", "Red Plague - Among the Clouds", 1.5f),
        new("com.lee23.theredplague", "theredplagueaudio", "voidislandcave", "Red Plague - Void Island Cave", 1.5f),
        new("com.lee23.theredplague", "theredplagueaudio", "infectedaurorademo1_2", "Red Plague - Mr. Teeth", 1.5f),
        new("com.lee23.theredplague", "theredplagueaudio", "RedPlagueDemo1.1", "Red Plague - The Plaggue Spreads", 1.5f),
        new("com.lee23.theredplague", "theredplagueaudio", "TRPConceptOST4(1)", "Red Plague - In the Halls of the Insane", 1.5f),
        new("com.lee23.theredplague", "theredplagueaudio", "TRPOstConcept2", "Red Plague - The Regular", 1.5f),
        new("com.lee23.theredplague", "theredplagueaudio", "TRPOstConecpt1(1)", "Red Plague - Bioweapon", 1.5f),
        new("com.aotu.returnoftheancients", "projectacientsaudio", "Main_Theme", "Return of the Ancients - Menu", 1.3f),
    };

    private static Dictionary<string, Func<bool>> ModMusicConfigLookUp { get; } = new()
    {
        { "com.aci.hydra", () => Plugin.ModConfig.LoadHydraModMusic },
        { "com.lee23.theredplague", () => Plugin.ModConfig.LoadRedPlagueMusic },
        { "com.aotu.returnoftheancients", () => Plugin.ModConfig.LoadReturnOfTheAncientsMusic }
    };

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
            LoadModMusic();
        }
        
        bool changed = !_initializedFirstLoad;
        var files = Directory.GetFiles(CustomTracksDirectory);
        foreach (var file in files)
        {
            if (!MusicLookUp.ContainsKey(file))
            {
                yield return RegisterNewTrack(file);
                changed = true;
            }
        }

        if (changed)
        {
            OnMusicDatabaseChanged?.Invoke();
        }

        _initializedFirstLoad = true;
        _busyRefreshingDatabase = false;
    }

    private static void LoadModMusic()
    {
        var validModsByGuid = new Dictionary<string, List<ModMusicData>>();
        foreach (var entry in _modMusic)
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
            if (ModMusicConfigLookUp.TryGetValue(mod.Key, out var shouldCheckModFunc) && !shouldCheckModFunc.Invoke())
            {
                Plugin.Logger.LogInfo($"Skipping loading music from {mod.Key} because it is disabled.");
                continue;
            }
            
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

    private struct ModMusicData
    {
        public string ModGuid { get; }
        public string AssetBundleName { get; }
        public string AudioClipName { get; }
        public string DisplayName { get; }
        public float Volume { get; }

        public ModMusicData(string modGuid, string assetBundleName, string audioClipName, string displayName, float volume)
        {
            ModGuid = modGuid;
            AssetBundleName = assetBundleName;
            AudioClipName = audioClipName;
            DisplayName = displayName;
            Volume = volume;
        }
    }
}