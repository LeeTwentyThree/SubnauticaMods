﻿using UnityEngine;

namespace PdaUpgradeChips.Data;

public abstract class PdaMusic
{
    protected PdaMusic(string id)
    {
        Id = id;
    }

    public abstract string GetTrackName();
    public abstract float GetDuration();
    
    protected string Id { get; }
    public AudioClip SoundAsset { get; protected set; }
    public float VolumeMultiplier { get; protected set; } = 1f;
}