using System;
using System.Collections;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace PdaUpgradeChips.Data;

public class PdaMusicCustom : PdaMusic
{
    private string FileName { get; }
    private string FilePath { get; }
    private float Duration { get; set; }

    public PdaMusicCustom(string filePath) : base(filePath)
    {
        FilePath = filePath;
        FileName = Path.GetFileNameWithoutExtension(FilePath);
    }

    public IEnumerator LoadAudio(IOut<bool> success)
    {
        success.Set(false);
        var type = GetAudioType(FilePath);

        if (type == AudioType.UNKNOWN)
        {
            Plugin.Logger.LogError("Unknown audio file type for file " + FilePath);
            yield break;
        }

        var task = LoadClip(FilePath, type);
        while (!task.IsCompleted && !task.IsCanceled)
        {
            yield return null;
        }
        
        var clip = task.Result;
        
        if (clip == null)
        {
            Plugin.Logger.LogError("Failed to load audio file " + FilePath);
            yield break;
        }

        SoundAsset = clip;
        Duration = clip.length;
        success.Set(true);
    }
    
    private static async Task<AudioClip> LoadClip(string path, AudioType type)
    {
        AudioClip clip = null;
        using var uwr = UnityWebRequestMultimedia.GetAudioClip(path, type);
        uwr.SendWebRequest();

        // wrap tasks in try/catch, otherwise it'll fail silently
        try
        {
            while (!uwr.isDone) await Task.Delay(5);

            if (uwr.isNetworkError || uwr.isHttpError) Debug.Log($"{uwr.error}");
            else
            {
                clip = DownloadHandlerAudioClip.GetContent(uwr);
            }
        }
        catch (Exception err)
        {
            Debug.Log($"{err.Message}, {err.StackTrace}");
        }

        return clip;
    }

    private static AudioType GetAudioType(string filePath)
    {
        var extension = Path.GetExtension(filePath);
        return extension switch
        {
            ".mp3" => AudioType.MPEG,
            ".wav" => AudioType.WAV,
            ".ogg" => AudioType.OGGVORBIS,
            _ => AudioType.UNKNOWN
        };
    }

    public override string GetTrackName()
    {
        return FileName;
    }

    public override float GetDuration()
    {
        return Duration;
    }
}