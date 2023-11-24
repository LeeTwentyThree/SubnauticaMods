using Nautilus.Handlers;
using Nautilus.Utility;
using UnityEngine;

namespace WeatherMod;

public class ModSound
{
    public AudioClip Clip { get; }
    public string Id { get; }
    public float Duration { get; }
    public FMODAsset Asset { get; }

    public ModSound(AudioClip clip, string id, float duration = -1f)
    {
        Clip = clip;
        Id = id;
        Duration = duration > 0 ? duration : clip.length; 
        Asset = AudioUtils.GetFmodAsset(id);
    }
    
    public ModSound(string clipName, string id, float duration = -1f) : this(Plugin.AssetBundle.LoadAsset<AudioClip>(clipName), id, duration) { }
    
    public ModSound(string clipNameAndId, float duration = -1f) : this(Plugin.AssetBundle.LoadAsset<AudioClip>(clipNameAndId), clipNameAndId, duration) { }

    public void Register(string bus, bool is3D, float minDistance = 1f, float maxDistance = 100f)
    {
        var sound = AudioUtils.CreateSound(Clip, is3D ? AudioUtils.StandardSoundModes_3D : AudioUtils.StandardSoundModes_2D);
        if (maxDistance > 0f)
        {
            sound.set3DMinMaxDistance(minDistance, maxDistance);
        }
        CustomSoundHandler.RegisterCustomSound(Id, sound, bus);
    }
}