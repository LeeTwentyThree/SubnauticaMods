namespace TheRedPlague.PrefabFiles.GargTeaser.GargPrefabData;

[System.Serializable]
public class GargSmallVehicleAggressionSettings
{
    public float MaxRange { get; }
    public float AggressionPerSecond { get; }

    public GargSmallVehicleAggressionSettings(float maxRange, float aggressionPerSecond)
    {
        MaxRange = maxRange;
        AggressionPerSecond = aggressionPerSecond;
    }
}