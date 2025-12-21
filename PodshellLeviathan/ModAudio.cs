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

    public static void RegisterAudio()
    {
        RegisterPodshellSound(CloseRoar, "close underwater roar by simon", 400);
        RegisterPodshellSound(Death, "Death sound underwater", 400);
        RegisterPodshellSound(Idle, "Idle_ close_ underwater", 200);
        RegisterPodshellSound(LongRoarClose, "Long roar_ close_ underwater", 400);
        RegisterPodshellSound(LongRoarFar, "Long roar_ far_ underwater", 400);
        RegisterPodshellSound(ShortRoarClose, "Short roar_ close_ underwater", 400);
        RegisterPodshellSound(ShortRoarFar, "Short roar_ far_ underwater", 400);
        RegisterPodshellSound(TeethGrinding, "Teeth grinding underwater", 320f);
    }

    private static void RegisterPodshellSound(FMODAsset asset, string clipName, float maxDistance)
    {
        var sound = AudioUtils.CreateSound(Plugin.Assets.LoadAsset<AudioClip>(clipName), AudioUtils.StandardSoundModes_3D);
        sound.set3DMinMaxDistance(10, maxDistance);
        CustomSoundHandler.RegisterCustomSound(asset.path, sound, AudioUtils.BusPaths.UnderwaterCreatures);
    }
}