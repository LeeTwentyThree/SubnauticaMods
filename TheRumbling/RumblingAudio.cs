using Nautilus.Handlers;
using Nautilus.Utility;
using UnityEngine;

namespace TheRumbling;

internal static class RumblingAudio
{
    public static FMODAsset TitanAmbienceSound { get; } = AudioUtils.GetFmodAsset("TitanAmbience");

    public static void RegisterAudio()
    {
        var ambienceSound = AudioUtils.CreateSound(Plugin.AssetBundle.LoadAsset<AudioClip>("singletitanaudio"), AudioUtils.StandardSoundModes_3D);
        ambienceSound.set3DMinMaxDistance(10f, 600f);
        CustomSoundHandler.RegisterCustomSound("TitanAmbience", ambienceSound, "bus:/master/SFX_for_pause/PDA_pause/all/SFX");
    }
}