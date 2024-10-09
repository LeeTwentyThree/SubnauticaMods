using System.Collections.Generic;
using System.Linq;
using FMOD;
using Nautilus.FMod;
using Nautilus.Handlers;
using Nautilus.Utility;
using UnityEngine;
using UnityEngine.Assertions.Must;

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
                "FleshBlobGroan1", "FleshBlobGroan2", "FleshBlobGroan3", "FleshBlobGroan4", "FleshBlobGroan5"
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
        
        // Mr teeth
        RegisterSoundWithVariants("MrTeethScream", new string[] {"MrTeethScream1", "MrTeethScream2", "MrTeethScream3"},
            "bus:/master/SFX_for_pause/PDA_pause/all/SFX/reverbsend", 5f, 120f);
        RegisterSound("MrTeethGrab", "MrTeethGrab", "bus:/master/SFX_for_pause/PDA_pause/all", 5f, 50);
        RegisterSound("MrTeethBury", "MrTeethBury", "bus:/master/SFX_for_pause/PDA_pause/all", 5f, 20f);
        
        // Aurora misc
        RegisterSound("UnlockTurretScream", "UnlockTurretScream", "bus:/master/SFX_for_pause/PDA_pause/all", 5f, 100f);
        RegisterSound("AuroraThrusterEvent", "AuroraThrusterEventFinal", "bus:/master/SFX_for_pause/PDA_pause/all", 5f, 10000f);
        
        // Cinematics
        
        Register2DSound("DomeConstruction", "DomeConstruction", "bus:/master/SFX_for_pause/PDA_pause/all");
        RegisterSound("NuclearExplosion", "Nuclear Explosion", "bus:/master/SFX_for_pause/PDA_pause/all", 5f, 5000f);
        RegisterSound("NuclearShockwave", "Nuclear Shockwave", "bus:/master/SFX_for_pause/PDA_pause/all", 5f, 5000f);
        
        // Plague cyclops
        RegisterSound("PlagueCyclopsAheadFlank", "aheadflank", "bus:/master/SFX_for_pause/PDA_pause/all/all voice/cyclops voice", 5f, -1);
        RegisterSound("PlagueCyclopsAheadSlow", "aheadslow", "bus:/master/SFX_for_pause/PDA_pause/all/all voice/cyclops voice", 5f, -1);
        RegisterSound("PlagueCyclopsAheadStandard", "aheadstandard", "bus:/master/SFX_for_pause/PDA_pause/all/all voice/cyclops voice", 5f, -1);
        RegisterSound("PlagueCyclopsAssimilationSuccessful", "assimilationsuccessful", "bus:/master/SFX_for_pause/PDA_pause/all/all voice/cyclops voice", 5f, 200f);
        RegisterSoundWithVariants("PlagueCyclopsScream", new[] {"cyclopsscream1","cyclopsscream2","cyclopsscream3"}, "bus:/master/SFX_for_pause/PDA_pause/all/all voice/cyclops voice", 5f, 200f);
        RegisterSound("PlagueCyclopsEngineMaintenance", "enginemaintenance", "bus:/master/SFX_for_pause/PDA_pause/all/all voice/cyclops voice", 5f, -1);
        RegisterSound("PlagueCyclopsEnginePoweringDown", "EnginePoweringDown", "bus:/master/SFX_for_pause/PDA_pause/all/all voice/cyclops voice", 5f, -1);
        RegisterSound("PlagueCyclopsEnginePoweringUp", "EnginePoweringUp", "bus:/master/SFX_for_pause/PDA_pause/all/all voice/cyclops voice", 5f, -1);
        RegisterSound("PlagueCyclopsPropellerObstruction", "propellorobstruction", "bus:/master/SFX_for_pause/PDA_pause/all/all voice/cyclops voice", 5f, -1);
        RegisterSound("PlagueCyclopsWelcomeAboard", "welcomeaboard", "bus:/master/SFX_for_pause/PDA_pause/all/all voice/cyclops voice", 5f, -1);
        RegisterSound("PlagueCyclopsWelcomeAboardGlitched", "WelcomeAboardGlitched", "bus:/master/SFX_for_pause/PDA_pause/all/all voice/cyclops voice", 5f, -1);
        RegisterSound("PlagueCyclopsWreckSpeech", "wreckspeech", "bus:/master/SFX_for_pause/all_no_pda_pause/all_voice_no_pda_pause/VOs", 5f, -1);
        RegisterSound("PlagueCyclopsDeath", "PlagueCyclopsDeath", "bus:/master/SFX_for_pause/PDA_pause/all/all voice/cyclops voice", 5f, -1);
        RegisterSound("PlagueCyclopsFall", "cyclopscrash", "bus:/master/SFX_for_pause/PDA_pause/all", 5f, 100);
        RegisterSound("PlagueCyclopsTentaclesSpawn", "cyclopsupgradeanimation", "bus:/master/SFX_for_pause/PDA_pause/all", 5f, 20);
        RegisterSound("PlagueCyclopsEngineBreak", "CyclopsEngineBreak", "bus:/master/SFX_for_pause/PDA_pause/all", 5f, 40);
        
        // Drifter
        var drifterIdleSound = AudioUtils.CreateSound(Bundle.LoadAsset<AudioClip>("DrifterIdle"), AudioUtils.StandardSoundModes_3D | MODE.LOOP_NORMAL);
        drifterIdleSound.set3DMinMaxDistance(5, 80);

        CustomSoundHandler.RegisterCustomSound("DrifterIdle", drifterIdleSound, "bus:/master/SFX_for_pause/PDA_pause/all");
        
        // Drifter cannon
        RegisterSound("DrifterCannonFire", "DrifterCannonFire", "bus:/master/SFX_for_pause/PDA_pause/all", 5f, 15f);
        RegisterSound("DrifterCannonFireNoAmmo", "DrifterCannonFireNoAmmo", "bus:/master/SFX_for_pause/PDA_pause/all", 5f, 15f);

        // Music
        RegisterSound("VoidIslandMusic", "voidislandcave", "bus:/master/SFX_for_pause/nofilter/music", -1f, -1);
        
        // Garg
        var gargSfxBus = "bus:/master/SFX_for_pause/PDA_pause/all/SFX/reverbsend";
            
        var gargCloseSounds = AudioUtils.CreateSounds(Plugin.AssetBundle.LoadAllAssetsWithPrefix<AudioClip>("garg_roar"), AudioUtils.StandardSoundModes_3D).ToArray();
        for (int i = 0; i < gargCloseSounds.Length; i++)
        {
            gargCloseSounds[i].set3DMinMaxDistance(25f, 10000f);
        }
        var gargCloseEvent = new FModMultiSounds(gargCloseSounds, gargSfxBus, true);
        CustomSoundHandler.RegisterCustomSound("RPGargAdultRoarClose", gargCloseEvent);

        var gargFarSounds = AudioUtils.CreateSounds(Plugin.AssetBundle.LoadAllAssetsWithPrefix<AudioClip>("garg_for_anth_distant"), AudioUtils.StandardSoundModes_3D).ToArray();
        for (int i = 0; i < gargFarSounds.Length; i++)
        {
            gargFarSounds[i].set3DMinMaxDistance(1f, 10000f);
        }
        var gargFarEvent = new FModMultiSounds(gargFarSounds, gargSfxBus, true);
        CustomSoundHandler.RegisterCustomSound("RPGargAdultRoarFar", gargFarEvent);
        
        var biteSounds = AudioUtils.CreateSounds(Plugin.AssetBundle.LoadAllAssetsWithPrefix<AudioClip>("GargBiteAttack"), AudioUtils.StandardSoundModes_3D).ToArray();
        for (int i = 0; i < biteSounds.Length; i++)
        {
            biteSounds[i].set3DMinMaxDistance(10f, 100f);
        }
        var biteEvent = new FModMultiSounds(biteSounds, gargSfxBus, true);
        CustomSoundHandler.RegisterCustomSound("RPGargBite", biteEvent);

        var gargEatPlayerCinematic = AudioUtils.CreateSound(Plugin.AssetBundle.LoadAsset<AudioClip>("GargBiteAttack5"), AudioUtils.StandardSoundModes_3D);
        gargEatPlayerCinematic.set3DMinMaxDistance(1f, 200f);
        CustomSoundHandler.RegisterCustomSound("RPGargEatPlayerCinematic", gargEatPlayerCinematic, gargSfxBus);

        var gargGrabSeamoth = AudioUtils.CreateSound(Plugin.AssetBundle.LoadAsset<AudioClip>("GargVehicleAttack"), AudioUtils.StandardSoundModes_3D);
        gargGrabSeamoth.set3DMinMaxDistance(1f, 100f);
        CustomSoundHandler.RegisterCustomSound("RPGargGrabSeamoth", gargGrabSeamoth, gargSfxBus);

        var gargGrabExosuit = AudioUtils.CreateSound(Plugin.AssetBundle.LoadAsset<AudioClip>("GargVehicleAttack"), AudioUtils.StandardSoundModes_3D);
        gargGrabExosuit.set3DMinMaxDistance(1f, 100f);
        CustomSoundHandler.RegisterCustomSound("RPGargGrabExosuit", gargGrabExosuit, gargSfxBus);

        var gargGrabCyclops = AudioUtils.CreateSound(Plugin.AssetBundle.LoadAsset<AudioClip>("GargCyclopsAttack"), AudioUtils.StandardSoundModes_3D);
        gargGrabCyclops.set3DMinMaxDistance(1f, 100f);
        CustomSoundHandler.RegisterCustomSound("RPGargGrabCyclops", gargGrabCyclops, gargSfxBus);

        var gargGrabReaper = AudioUtils.CreateSound(Plugin.AssetBundle.LoadAsset<AudioClip>("GargReaperAttack"), AudioUtils.StandardSoundModes_3D);
        gargGrabReaper.set3DMinMaxDistance(1f, 400f);
        CustomSoundHandler.RegisterCustomSound("RPGargGrabReaper", gargGrabReaper, gargSfxBus);

        var gargGrabGhost = AudioUtils.CreateSound(Plugin.AssetBundle.LoadAsset<AudioClip>("GargGhostLeviathanAttack"), AudioUtils.StandardSoundModes_3D);
        gargGrabGhost.set3DMinMaxDistance(1f, 400f);
        CustomSoundHandler.RegisterCustomSound("RPGargGrabGhost", gargGrabGhost, gargSfxBus);
        
        // plague garg
        var plagueGargCloseSounds = AudioUtils.CreateSounds(Plugin.AssetBundle.LoadAllAssetsWithPrefix<AudioClip>("infected_garg_roar-"), AudioUtils.StandardSoundModes_3D).ToArray();
        for (int i = 0; i < plagueGargCloseSounds.Length; i++)
        {
            plagueGargCloseSounds[i].set3DMinMaxDistance(25f, 10000f);
        }
        var plagueGargCloseEvent = new FModMultiSounds(plagueGargCloseSounds, gargSfxBus, true);
        CustomSoundHandler.RegisterCustomSound("PlagueGargRoarClose", plagueGargCloseEvent);

        var plagueGargFarSounds = AudioUtils.CreateSounds(Plugin.AssetBundle.LoadAllAssetsWithPrefix<AudioClip>("infected_garg_roar_far-"), AudioUtils.StandardSoundModes_3D).ToArray();
        for (int i = 0; i < plagueGargFarSounds.Length; i++)
        {
            plagueGargFarSounds[i].set3DMinMaxDistance(1f, 10000f);
        }
        var plagueGargFarEvent = new FModMultiSounds(plagueGargFarSounds, gargSfxBus, true);
        CustomSoundHandler.RegisterCustomSound("PlagueGargRoarFar", plagueGargFarEvent);

        
        var plagueGargSnarlSounds = AudioUtils.CreateSounds(Plugin.AssetBundle.LoadAllAssetsWithPrefix<AudioClip>("infected_garg_snarl"), AudioUtils.StandardSoundModes_3D).ToArray();
        for (int i = 0; i < plagueGargSnarlSounds.Length; i++)
        {
            plagueGargSnarlSounds[i].set3DMinMaxDistance(25f, 400f);
        }
        var plagueGargSnarl = new FModMultiSounds(plagueGargSnarlSounds, gargSfxBus, true);
        CustomSoundHandler.RegisterCustomSound("PlagueGargSnarl", plagueGargSnarl);
        
        // MUSIC!
        
        Register2DSound("IslandElevatorFirst", "elevator2b", AudioUtils.BusPaths.Music);
        Register2DSound("IslandElevator", "elevator2lousyversion", AudioUtils.BusPaths.Music);
    }
    
    private static IEnumerable<T> LoadAllAssetsWithPrefix<T>(this AssetBundle assetBundle, string prefix) where T : Object
    {
        return assetBundle.LoadAllAssets<T>().Where(c => c.name.StartsWith(prefix));
    }

    private static void Register2DSound(string id, string clipName, string bus)
    {
        var sound = AudioUtils.CreateSound(Bundle.LoadAsset<AudioClip>(clipName), AudioUtils.StandardSoundModes_2D);

        CustomSoundHandler.RegisterCustomSound(id, sound, bus);
    }

    private static void RegisterSound(string id, string clipName, string bus, float minDistance = 10f,
        float maxDistance = 200f)
    {
        var sound = AudioUtils.CreateSound(Bundle.LoadAsset<AudioClip>(clipName), maxDistance >= 0 ? AudioUtils.StandardSoundModes_3D : AudioUtils.StandardSoundModes_2D);
        if (maxDistance >= 0)
            sound.set3DMinMaxDistance(minDistance, maxDistance);

        CustomSoundHandler.RegisterCustomSound(id, sound, bus);
    }

    private static void RegisterSoundWithVariants(string id, string[] clipNames, string bus, float minDistance = 10f,
        float maxDistance = 200f)
    {
        var clipList = new List<AudioClip>();
        clipNames.ForEach(clipName => clipList.Add(Bundle.LoadAsset<AudioClip>(clipName)));

        var sounds = AudioUtils.CreateSounds(clipList, maxDistance >= 0 ? AudioUtils.StandardSoundModes_3D : AudioUtils.StandardSoundModes_2D);
        if (maxDistance >= 0)
            sounds.ForEach(sound => sound.set3DMinMaxDistance(minDistance, maxDistance));

        var multiSoundsEvent = new FModMultiSounds(sounds.ToArray(), bus, true);

        CustomSoundHandler.RegisterCustomSound(id, multiSoundsEvent);
    }
}