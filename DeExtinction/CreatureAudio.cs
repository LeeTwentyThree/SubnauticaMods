using System.Collections.Generic;
using System.Linq;
using Nautilus.FMod;
using Nautilus.Handlers;
using Nautilus.Utility;
using UnityEngine;

namespace DeExtinction;

public static class CreatureAudio
{
    private static AssetBundle Bundle => Plugin.AssetBundle;

    public static void RegisterAudio()
    {
        RegisterSound("ClownPincherEating", "ClownPincherEating",
            "bus:/master/SFX_for_pause/PDA_pause/all/SFX/creatures", 2, 16);
        RegisterSoundWithVariants("ClownPincherIdle",
            new[]
            {
                "ClownPincherIdle1", "ClownPincherIdle2", "ClownPincherIdle3", "ClownPincherIdle4", "ClownPincherIdle5"
            },
            "bus:/master/SFX_for_pause/PDA_pause/all/SFX/creatures", 2, 16);

        RegisterSoundWithVariants("GulperBite",
            new[]
            {
                "GulperBite1", "GulperBite2"
            },
            "bus:/master/SFX_for_pause/PDA_pause/all/SFX/creatures", 10, 40);

        RegisterSound("GulperBitePlayer", "GulperBitePlayer",
            "bus:/master/SFX_for_pause/PDA_pause/all/SFX/creatures", 10, 40);
        
        RegisterSound("GulperAttackSeamoth", "GulperSeamoth1",
            "bus:/master/SFX_for_pause/PDA_pause/all/SFX/creatures", 10, 40);

        RegisterSound("GulperAttackExosuit", "GulperExosuit",
            "bus:/master/SFX_for_pause/PDA_pause/all/SFX/creatures", 10, 40);

        RegisterSoundWithVariants("GulperClawAttack",
            new[]
            {
                "GulperClawAttack1", "GulperClawAttack2", "GulperClawAttack3"
            },
            "bus:/master/SFX_for_pause/PDA_pause/all/SFX/creatures", 10, 50);

        RegisterSoundWithVariants("GulperRoar",
            new[]
            {
                "GulperRoar1", "GulperRoar2", "GulperRoar3", "GulperRoar4"
            },
            "bus:/master/SFX_for_pause/PDA_pause/all/SFX/creatures", 10, 200);

        RegisterSoundWithVariants("GulperRoarFar",
            new[]
            {
                "GulperRoarFar1", "GulperRoarFar2", "GulperRoarFar3"
            },
            "bus:/master/SFX_for_pause/PDA_pause/all/SFX/creatures", 10, 400);
        
        RegisterSoundWithVariants("ThalassaceanRoar",
            new[]
            {
                "ThalassaceanRoar1", "ThalassaceanRoar2", "ThalassaceanRoar3", "ThalassaceanRoar4"
            },
            "bus:/master/SFX_for_pause/PDA_pause/all/SFX/creatures", 10, 120);
        
        RegisterSoundWithVariants("TwisteelRoar",
            new[]
            {
                "TwisteelRoar_Long", "TwisteelRoar_Short", "TwisteelRoar_Shriek1", "TwisteelRoar_Shriek2"
            },
            "bus:/master/SFX_for_pause/PDA_pause/all/SFX/creatures", 5, 50);
        
        RegisterSoundWithVariants("TwisteelAttack",
            new[]
            {
                "TwisteelRoar_Attack"
            },
            "bus:/master/SFX_for_pause/PDA_pause/all/SFX/creatures", 5, 40);
        
        RegisterSoundWithVariants("TwisteelCinematicAttack",
            new[]
            {
                "TwisteelCinematicAttack"
            },
            "bus:/master/SFX_for_pause/PDA_pause/all/SFX/creatures", 5, 40);
    }

    private static void Register2DSound(string id, string clipName, string bus)
    {
        var sound = AudioUtils.CreateSound(Bundle.LoadAsset<AudioClip>(clipName), AudioUtils.StandardSoundModes_2D);

        CustomSoundHandler.RegisterCustomSound(id, sound, bus);
    }

    private static void RegisterSound(string id, string clipName, string bus, float minDistance = 10f,
        float maxDistance = 200f)
    {
        var sound = AudioUtils.CreateSound(Bundle.LoadAsset<AudioClip>(clipName), AudioUtils.StandardSoundModes_3D);
        sound.set3DMinMaxDistance(minDistance, maxDistance);

        CustomSoundHandler.RegisterCustomSound(id, sound, bus);
    }

    private static void RegisterSoundWithVariants(string id, string[] clipNames, string bus, float minDistance = 10f,
        float maxDistance = 200f)
    {
        var clipList = new List<AudioClip>();
        clipNames.ForEach(clipName => clipList.Add(Bundle.LoadAsset<AudioClip>(clipName)));

        var sounds = AudioUtils.CreateSounds(clipList, AudioUtils.StandardSoundModes_3D);
        sounds.ForEach(sound => sound.set3DMinMaxDistance(minDistance, maxDistance));

        var multiSoundsEvent = new FModMultiSounds(sounds.ToArray(), bus, false);

        CustomSoundHandler.RegisterCustomSound(id, multiSoundsEvent);
    }
}