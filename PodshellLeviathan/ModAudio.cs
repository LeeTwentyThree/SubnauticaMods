using System.Collections.Generic;
using System.Linq;
using FMOD;
using Nautilus.Extensions;
using Nautilus.FMod;
using Nautilus.Handlers;
using Nautilus.Utility;
using UnityEngine;

namespace PodshellLeviathan;

public static class ModAudio
{
    public static FMODAsset CloseRoar { get; } = AudioUtils.GetFmodAsset("PodshellCloseRoar");
    public static FMODAsset Death { get; } = AudioUtils.GetFmodAsset("PodshellDeath");
    public static FMODAsset Idle { get; } = AudioUtils.GetFmodAsset("PodshellIdle");
    public static FMODAsset LongRoarClose { get; } = AudioUtils.GetFmodAsset("PodshellLongRoarClose");
    public static FMODAsset LongRoarFar { get; } = AudioUtils.GetFmodAsset("PodshellLongRoarFar");
    public static FMODAsset ShortRoarClose { get; } = AudioUtils.GetFmodAsset("PodshellShortRoarClose");
    public static FMODAsset ShortRoarFar { get; } = AudioUtils.GetFmodAsset("PodshellShortRoarFar");
    public static FMODAsset TeethGrinding { get; } = AudioUtils.GetFmodAsset("PodshellTeethGrinding");
    public static FMODAsset PodshellBabyRoar { get; } = AudioUtils.GetFmodAsset("PodshellBabyRoar");
    
    // music
    public static FMODAsset PodshellMusic { get; } = AudioUtils.GetFmodAsset("PodshellMusic");

    public static void RegisterAudio()
    {
        RegisterPodshellSound(CloseRoar, "close underwater roar by simon", 10, 400);
        RegisterPodshellSound(Death, "Death sound underwater", 10, 400);
        RegisterPodshellSound(Idle, "Idle_ close_ underwater", 10, 200);
        RegisterPodshellSound(LongRoarClose, "Long roar_ close_ underwater", 10, 400);
        RegisterPodshellSound(LongRoarFar, "Long roar_ far_ underwater", 10, 400);
        RegisterPodshellSound(ShortRoarClose, "Short roar_ close_ underwater", 10, 400);
        RegisterPodshellSound(ShortRoarFar, "Short roar_ far_ underwater", 10, 400);
        RegisterPodshellSound(TeethGrinding, "Teeth grinding underwater", 10, 320f);
        RegisterPodshellSound(PodshellBabyRoar, 1.4f, 27f, new[]
        {
            "PodshellBabyRoar1",
            "PodshellBabyRoar2",
            "PodshellBabyRoar3",
            "PodshellBabyRoar4"
        });

        RegisterMusic(PodshellMusic, "PodshellLeviathanOST", 2);
    }

    private static void RegisterPodshellSound(FMODAsset asset, string clipName, float minDistance, float maxDistance)
    {
        var sound = AudioUtils.CreateSound(Plugin.Assets.LoadAsset<AudioClip>(clipName), AudioUtils.StandardSoundModes_3D);
        sound.set3DMinMaxDistance(minDistance, maxDistance);
        CustomSoundHandler.RegisterCustomSound(asset.path, sound, AudioUtils.BusPaths.UnderwaterCreatures);
    }
    
    private static void RegisterPodshellSound(FMODAsset asset, float minDistance, float maxDistance, string[] clipNames)
    {
        var clipList = new List<AudioClip>();
        clipNames.ForEach(clipName => clipList.Add(Plugin.Assets.LoadAsset<AudioClip>(clipName)));

        var sounds = AudioUtils.CreateSounds(clipList,
            maxDistance >= 0 ? AudioUtils.StandardSoundModes_3D : AudioUtils.StandardSoundModes_2D);
        sounds.ForEach(sound =>
        {
            if (maxDistance >= 0)
                sound.set3DMinMaxDistance(minDistance, maxDistance);
        });
        
        var multiSoundsEvent = new FModMultiSounds(sounds.ToArray(), AudioUtils.BusPaths.UnderwaterCreatures, true);
        
        CustomSoundHandler.RegisterCustomSound(asset.path, multiSoundsEvent);
    }
    
    private static void RegisterMusic(FMODAsset asset, string clipName, float fadeOutDuration = 10f,
        bool useSoundEffectsBus = false, bool looping = false)
    {
        var mode = AudioUtils.StandardSoundModes_2D;
        if (looping)
            mode |= MODE.LOOP_NORMAL;
        var sound = AudioUtils.CreateSound(Plugin.Assets.LoadAsset<AudioClip>(clipName), mode);
        if (fadeOutDuration > Mathf.Epsilon)
            sound.AddFadeOut(fadeOutDuration);
        CustomSoundHandler.RegisterCustomSound(asset.id, sound,
            useSoundEffectsBus ? "bus:/master/SFX_for_pause/PDA_pause/all" : AudioUtils.BusPaths.Music);
    }
}