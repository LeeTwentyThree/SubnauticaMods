namespace TheRedPlague.PrefabFiles.GargTeaser.GargPrefabData;

[System.Serializable]
public class GargBiteSettings
{
    public bool CanAttackPlayer { get; }
    public float Damage { get; }
    public bool InstantlyKillsPlayer { get; }
    public bool CanGrabCyclops { get; }
    public GargGrabFishMode GrabFishMode { get; }

    public GargBiteSettings(bool attackPlayer, float damage, bool instantlyKillsPlayer, bool canGrabCyclops, GargGrabFishMode grabFishMode)
    {
        CanAttackPlayer = attackPlayer;
        Damage = damage;
        InstantlyKillsPlayer = instantlyKillsPlayer;
        CanGrabCyclops = canGrabCyclops;
        GrabFishMode = grabFishMode;
    }
}