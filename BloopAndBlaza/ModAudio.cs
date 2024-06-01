using System.Collections.Generic;
using System.Linq;
using Nautilus.FMod;
using Nautilus.Handlers;
using Nautilus.Utility;
using UnityEngine;

namespace BloopAndBlaza;

public static class ModAudio
{
    private static AssetBundle Bundle => Plugin.AssetBundle;
    
    private const string CreaturesBus = "bus:/master/SFX_for_pause/PDA_pause/all/SFX/creatures";

    public static void RegisterAllAudio()
    {
        // Blaza:
        RegisterSoundWithVariants("BlazaIdle", new string[] {"BlazaIdle1", "BlazaIdle2", "BlazaIdle3", "BlazaIdle4", "BlazaIdle5"}, CreaturesBus, 5f, 150f);
        RegisterSoundWithVariants("BlazaBite", new string[] {"BlazaBite1", "BlazaBite2"}, CreaturesBus, 5f, 30f);
        RegisterSound("BlazaExosuitAttack", "BlazaExosuit", CreaturesBus, 5f, 30f);
        RegisterSound("BlazaSeamothAttack", "BlazaSeamoth", CreaturesBus, 5f, 30f);

        // Bloop:
        RegisterSoundWithVariants("BloopIdle", new string[] {"BloopRoar1", "BloopRoar2", "BloopRoar3", "BloopRoar4", "BloopRoar5"}, CreaturesBus, 20f, 100f);
        RegisterSound("BloopVortexAttack", "BloopVortexAttack", CreaturesBus, 5f, 40f);
        RegisterSoundWithVariants("BloopSwim", new string[] {"BloopSwim1", "BloopSwim2", "BloopSwim3", "BloopSwim4", "BloopSwim5"}, CreaturesBus, 5f, 40f);
    }
    
    private static void RegisterSound(string id, string clipName, string bus, float minDistance = 10f,
        float maxDistance = 200f)
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

        var multiSoundsEvent = new FModMultiSounds(sounds.ToArray(), bus, true);

        CustomSoundHandler.RegisterCustomSound(id, multiSoundsEvent);
    }
}