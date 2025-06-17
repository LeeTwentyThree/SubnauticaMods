using UnityEngine;

namespace PdaUpgradeChips.Data;

public class PdaMusicAudioClip : PdaMusic
{
    private string DisplayName { get; }
    private float Duration { get; }

    public PdaMusicAudioClip(string id, string displayName, AudioClip audioClip, float volumeMultiplier = 1f) : base(id)
    {
        SoundAsset = audioClip;
        DisplayName = displayName;
        Duration = audioClip.length;
        VolumeMultiplier = volumeMultiplier;
    }

    public override string GetTrackName()
    {
        return DisplayName;
    }

    public override float GetDuration()
    {
        return Duration;
    }
}