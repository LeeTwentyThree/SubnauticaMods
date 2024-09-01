using TheRedPlague.Mono.PlagueGarg.GargBehavior.GargEssentials;

namespace TheRedPlague.PrefabFiles.GargTeaser.GargPrefabData;

[System.Serializable]
public class GargRoarSettings
{
    public bool TriggerScreenShake { get; }
    public float MinRoarDelay { get; }
    public float MaxRoarDelay { get; }
    public bool DealsDamage { get; }
    public GargantuanRoar.RoarMode RoarMode { get; }
    public string RoarSoundEventPrefix { get; }
    public float CloseRoarDistanceThreshold { get; }
    public bool ProducesChromaticAberration { get; }

    public GargRoarSettings(bool triggerScreenShake, float minRoarDelay, float maxRoarDelay, bool dealsDamage, GargantuanRoar.RoarMode roarMode,
        string roarSoundEventPrefix, float closeRoarDistanceThreshold, bool producesChromaticAberration)
    {
        TriggerScreenShake = triggerScreenShake;
        MinRoarDelay = minRoarDelay;
        MaxRoarDelay = maxRoarDelay;
        DealsDamage = dealsDamage;
        RoarMode = roarMode;
        RoarSoundEventPrefix = roarSoundEventPrefix;
        CloseRoarDistanceThreshold = closeRoarDistanceThreshold;
        ProducesChromaticAberration = producesChromaticAberration;
    }
}
