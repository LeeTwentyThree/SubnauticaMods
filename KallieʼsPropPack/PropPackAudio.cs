using System.Linq;
using FMOD;
using Nautilus.Extensions;
using Nautilus.FMod;
using Nautilus.Handlers;
using Nautilus.Utility;
using UnityEngine;

namespace KallieʼsPropPack;

public static class PropPackAudio
{
    private static AssetBundle Bundle => Plugin.Bundle;

    internal static void RegisterAudio()
    {
        RegisterAmbience("SCL_Ambience", "scl ambiance", 2);
    }

    private static void RegisterAmbience(string id, string clipName, float fadeOutDuration = 2f,
        string bus = "bus:/master/SFX_for_pause/PDA_pause/all/SFX/backgrounds")
    {
        var sound = AudioUtils.CreateSound(Bundle.LoadAsset<AudioClip>(clipName),
            AudioUtils.StandardSoundModes_2D | MODE.LOOP_NORMAL);
        sound.AddFadeOut(fadeOutDuration);
        CustomSoundHandler.RegisterCustomSound(id, sound, bus);

        var tentacleHitSounds = AudioUtils
            .CreateSounds(
                new[]
                {
                    Bundle.LoadAsset<AudioClip>("TentacleHit1"), Bundle.LoadAsset<AudioClip>("TentacleHit2")
                }, AudioUtils.StandardSoundModes_3D).ToArray();
        for (int i = 0; i < tentacleHitSounds.Length; i++)
        {
            tentacleHitSounds[i].set3DMinMaxDistance(5f, 40f);
        }

        var tentacleHit = new FModMultiSounds(tentacleHitSounds, AudioUtils.BusPaths.SFX, true);
        CustomSoundHandler.RegisterCustomSound("SclTentacleHit", tentacleHit);
    }
}