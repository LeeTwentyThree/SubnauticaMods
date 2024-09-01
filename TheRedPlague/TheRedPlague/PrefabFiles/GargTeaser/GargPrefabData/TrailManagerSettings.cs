using UnityEngine;

namespace TheRedPlague.PrefabFiles.GargTeaser.GargPrefabData;

[System.Serializable]
public class TrailManagerSettings
{
    public float SnapSpeed { get; }
    public float MaxOffset { get; }
    public AnimationCurve Multiplier { get; }

    public TrailManagerSettings(float snapSpeed, float maxOffset, AnimationCurve multiplier)
    {
        SnapSpeed = snapSpeed;
        MaxOffset = maxOffset;
        Multiplier = multiplier;
    }
}