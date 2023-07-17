using Nautilus.Utility;
using Nautilus.Handlers;
using FMOD;
using UnityEngine;

namespace AggressiveFauna;

internal class ModAudio
{
    /// <summary>
    /// 3D sounds
    /// </summary>
    public const MODE k3DSoundModes = MODE.DEFAULT | MODE._3D | MODE.ACCURATETIME | MODE._3D_LINEARSQUAREROLLOFF;
    /// <summary>
    /// 2D sounds
    /// </summary>
    public const MODE k2DSoundModes = MODE.DEFAULT | MODE._2D | MODE.ACCURATETIME;
    /// <summary>
    /// For music, PDA and any 2D sounds that cant have more than one instance playing at a time.
    /// </summary>
    public const MODE kStreamSoundModes = k2DSoundModes | MODE.CREATESTREAM;

    public const string kMusicSFXBus = AudioUtils.BusPaths.Music;

    public static void PatchAudio()
    {
        AddMusic(Plugin.bundle.LoadAsset<AudioClip>("ColdBlack (by Shiruba)"), "ColdBlack"); // music when night falls
        ApocalypseMusic.SetupMusic(GetFmodAsset("ColdBlack"), 174f);
    }

    private static void AddMusic(AudioClip clip, string soundPath)
    {
        var sound = AudioUtils.CreateSound(clip, kStreamSoundModes);
        CustomSoundHandler.RegisterCustomSound(soundPath, sound, kMusicSFXBus);
    }

    private static FMODAsset GetFmodAsset(string audioPath)
    {
        FMODAsset asset = ScriptableObject.CreateInstance<FMODAsset>();
        asset.path = audioPath;
        return asset;
    }
}