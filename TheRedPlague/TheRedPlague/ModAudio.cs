using System.Collections.Generic;
using System.Linq;
using Nautilus.FMod;
using Nautilus.Handlers;
using Nautilus.Utility;
using UnityEngine;

namespace TheRedPlague;

public static class ModAudio
{
    private static AssetBundle Bundle => Plugin.AssetBundle;
    
    public static void RegisterAudio()
    {
        RegisterSound("InfectionLaserShot", "InfectionLaserShot", "bus:/master/SFX_for_pause/PDA_pause/all/SFX/reverbsend", 2, 20);
        Register2DSound("SeaEmperorJumpscare", "SeaEmperorJumpscare", "bus:/master/SFX_for_pause/PDA_pause/all");
        Register2DSound("DieFromInfection", "DieFromInfection", "bus:/master/SFX_for_pause/PDA_pause/all");
        RegisterSound("DisableDomeSound", "DisableDomeSound", "bus:/master/SFX_for_pause/PDA_pause/all", 40, 10000);
        RegisterSound("PlagueHeartAmbience", "PlagueHeartAmbience", "bus:/master/SFX_for_pause/PDA_pause/all/SFX/backgrounds", 4f, 20f);
        Register2DSound("InfectedDunesAmbience", "InfectedDunesAmbience", "bus:/master/SFX_for_pause/PDA_pause/all/SFX/backgrounds");
        RegisterSoundWithVariants("WarperJumpscare", new string[]{"Jumpscare1", "Jumpscare2", "Jumpscare3", "Jumpscare4", "Jumpscare5"}, "bus:/master/SFX_for_pause/PDA_pause/all/SFX/reverbsend", 3f, 70);
        RegisterSoundWithVariants("InfectedWarperIdle", new string[]{"WarperSound1", "WarperSound2", "WarperSound3", "WarperSound4", "WarperSound5", "WarperSound6"}, "bus:/master/SFX_for_pause/PDA_pause/all/SFX/reverbsend", 5f, 30f);
        RegisterSoundWithVariants("CloseJumpScare", new string[]{"CloseScare1", "CloseScare2", "CloseScare3", "CloseScare4", "CloseScare5", "CloseScare6"}, "bus:/master/SFX_for_pause/PDA_pause/all/SFX/reverbsend", 5f, 30f);
        RegisterSoundWithVariants("ZombieRoar", new string[]{"ZombieRoar1", "ZombieRoar2"}, "bus:/master/SFX_for_pause/PDA_pause/all/SFX/reverbsend", 5f, 30f);
    }
    
    private static void Register2DSound(string id, string clipName, string bus)
    {
        var sound = AudioUtils.CreateSound(Bundle.LoadAsset<AudioClip>(clipName), AudioUtils.StandardSoundModes_2D);

        CustomSoundHandler.RegisterCustomSound(id, sound, bus);
    }
    
    private static void RegisterSound(string id, string clipName, string bus, float minDistance = 10f, float maxDistance = 200f)
    {
        var sound = AudioUtils.CreateSound(Bundle.LoadAsset<AudioClip>(clipName), AudioUtils.StandardSoundModes_3D);
        sound.set3DMinMaxDistance(minDistance, maxDistance);

        CustomSoundHandler.RegisterCustomSound(id, sound, bus);
    }

    private static void RegisterSoundWithVariants(string id, string[] clipNames, string bus, float minDistance = 10f, float maxDistance = 200f)
    {
        var clipList = new List<AudioClip>();
        clipNames.ForEach(clipName => clipList.Add(Bundle.LoadAsset<AudioClip>(clipName)));

        var sounds = AudioUtils.CreateSounds(clipList, AudioUtils.StandardSoundModes_3D);
        sounds.ForEach(sound => sound.set3DMinMaxDistance(minDistance, maxDistance));

        var multiSoundsEvent = new FModMultiSounds(sounds.ToArray(), bus, false);

        CustomSoundHandler.RegisterCustomSound(id, multiSoundsEvent);
    }
}