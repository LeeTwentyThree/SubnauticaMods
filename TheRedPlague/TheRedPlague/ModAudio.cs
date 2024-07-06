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
        RegisterSound("InfectionLaserShot", "InfectionLaserShot",
            "bus:/master/SFX_for_pause/PDA_pause/all/SFX/reverbsend", 2, 20);
        Register2DSound("SeaEmperorJumpscare", "SeaEmperorJumpscare", "bus:/master/SFX_for_pause/PDA_pause/all");
        Register2DSound("DieFromInfection", "DieFromInfection", "bus:/master/SFX_for_pause/PDA_pause/all");
        RegisterSound("DisableDomeSound", "DisableDomeSound", "bus:/master/SFX_for_pause/PDA_pause/all", 40, 10000);
        RegisterSound("PlagueHeartAmbience", "PlagueHeartAmbience",
            "bus:/master/SFX_for_pause/PDA_pause/all/SFX/backgrounds", 4f, 20f);
        Register2DSound("InfectedDunesAmbience", "InfectedDunesAmbience",
            "bus:/master/SFX_for_pause/PDA_pause/all/SFX/backgrounds");
        // yep, i skip #1
        RegisterSoundWithVariants("WarperJumpscare",
            new string[] {"Jumpscare2", "Jumpscare3", "Jumpscare4", "Jumpscare5"},
            "bus:/master/SFX_for_pause/PDA_pause/all/SFX/reverbsend", 3f, 70);
        RegisterSoundWithVariants("InfectedWarperIdle",
            new string[]
                {"WarperSound1", "WarperSound2", "WarperSound3", "WarperSound4", "WarperSound5", "WarperSound6"},
            "bus:/master/SFX_for_pause/PDA_pause/all/SFX/reverbsend", 5f, 30f);
        RegisterSoundWithVariants("CloseJumpScare",
            // yep, I skip #5
            new string[] {"CloseScare1", "CloseScare2", "CloseScare3", "CloseScare4", "CloseScare6"},
            "bus:/master/SFX_for_pause/PDA_pause/all/SFX/reverbsend", 5f, 30f);
        RegisterSoundWithVariants("ZombieRoar", new string[] {"ZombieRoar1", "ZombieRoar2"},
            "bus:/master/SFX_for_pause/PDA_pause/all/SFX/reverbsend", 5f, 30f);
        RegisterSoundWithVariants("ZombieBite", new string[] {"ZombieBite1", "ZombieBite2", "ZombieBite3"},
            "bus:/master/SFX_for_pause/PDA_pause/all/SFX/reverbsend", 5f, 30f);
        RegisterSoundWithVariants("SmallZombieBite", new string[] {"SmallZombieBite1", "SmallZombieBite2", "SmallZombieBite3"},
            "bus:/master/SFX_for_pause/PDA_pause/all/SFX/creatures", 2f, 6f);
        Register2DSound("AirStrike", "AirStrike", "bus:/master/SFX_for_pause/PDA_pause/all");
        RegisterSoundWithVariants("AirStrikeExplosion", new string[] {"Underwater Explosion", "Underwater Explosion 2"},
            "bus:/master/SFX_for_pause/PDA_pause/all/SFX/reverbsend", 10f, 300f);

        RegisterSound("FleshBlobActivate", "FleshBlobActivate", "bus:/master/SFX_for_pause/PDA_pause/all", 10, 500);
        RegisterSound("FleshBlobTornadoLoop", "PlagueTornadoLoop", "bus:/master/SFX_for_pause/PDA_pause/all", 40, 300);
        RegisterSoundWithVariants("FleshBlobGroan",
            new string[]
            {
                "FleshBlobGroan1", "FleshBlobGroan2", "FleshBlobGroan3", "FleshBlobGroan4", "FleshBlobGroan5",
                "FleshBlobGroan6"
            },
            "bus:/master/SFX_for_pause/PDA_pause/all/SFX/reverbsend", 10f, 250f);
        RegisterSound("FleshBlobHunt", "FleshBlobApproach", "bus:/master/SFX_for_pause/PDA_pause/all", 10, 500);
        RegisterSound("FleshBlobAlarm", "FleshBlobAlarm", "bus:/master/SFX_for_pause/PDA_pause/all", 10, 500);
        RegisterSoundWithVariants("FleshBlobWalk", new string[] {"FleshBlobWalk1", "FleshBlobWalk2"},
            "bus:/master/SFX_for_pause/PDA_pause/all/SFX/reverbsend", 10f, 140);
        RegisterSoundWithVariants("FleshBlobRoar", new string[] {"FleshBlobGroanClose1", "FleshBlobGroanClose2", "FleshBlobGroanClose3", "FleshBlobScream3"},
            "bus:/master/SFX_for_pause/PDA_pause/all/SFX/reverbsend", 5f, 120f);
        RegisterSoundWithVariants("RandomFootsteps", new string[] {"FootstepsSlow", "FootstepsFast"},
            "bus:/master/SFX_for_pause/PDA_pause/all/SFX/reverbsend", 5f, 120f);
        RegisterSound("SuckerDeath", "SuckerDeath", "bus:/master/SFX_for_pause/PDA_pause/all", 3f, 50);
        RegisterSoundWithVariants("MrTeethScream", new string[] {"MrTeethScream1", "MrTeethScream2", "MrTeethScream3"},
            "bus:/master/SFX_for_pause/PDA_pause/all/SFX/reverbsend", 5f, 120f);
        RegisterSound("MrTeethGrab", "MrTeethGrab", "bus:/master/SFX_for_pause/PDA_pause/all", 5f, 50);
        RegisterSound("MrTeethBury", "MrTeethBury", "bus:/master/SFX_for_pause/PDA_pause/all", 5f, 20f);
        RegisterSound("UnlockTurretScream", "UnlockTurretScream", "bus:/master/SFX_for_pause/PDA_pause/all", 5f, 100f);
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

        var multiSoundsEvent = new FModMultiSounds(sounds.ToArray(), bus, true);

        CustomSoundHandler.RegisterCustomSound(id, multiSoundsEvent);
    }
}