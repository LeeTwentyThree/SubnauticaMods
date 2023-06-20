using BepInEx.Bootstrap;

namespace CreatureMorphs;

internal static class MorphDatabase
{
    internal static void Setup()
    {
        AddBuiltInEntries();
    }

    private static readonly List<Entry> entries = new List<Entry>();

    private static void AddBuiltInEntries()
    {
        var builder = new MorphBuilder();

        builder.Create(TechType.Biter, MorphAnimationType.Prey);
        builder.AddAbility<Bite>((b) => { });
        builder.Finish();

        builder.Create(TechType.Stalker, MorphAnimationType.Shark, 4);
        builder.AddAbility<Bite>((b) => { });
        builder.SetSwimSpeed(6f);
        builder.SetCameraFollowDistance(8f);
        builder.Finish();

        builder.Create(TechType.BoneShark, MorphAnimationType.Shark, 4);
        builder.SetCameraFollowDistance(8f);
        builder.Finish();

        builder.Create(TechType.Crabsnake, MorphAnimationType.Shark, 5);
        builder.SetCameraFollowDistance(16f);
        builder.Finish();

        builder.Create(TechType.Shocker, MorphAnimationType.Shark, 6);
        builder.SetCameraFollowDistance(12f);
        builder.Finish();

        builder.Create(TechType.CrabSquid, MorphAnimationType.Shark, 8);
        builder.SetCameraFollowDistance(10f);
        builder.Finish();

        builder.Create(TechType.Warper, MorphAnimationType.Shark, 5);
        builder.SetCameraFollowDistance(10f);
        builder.SetSwimSpeed(6f);
        builder.Finish();

        builder.Create(TechType.ReaperLeviathan, MorphAnimationType.Leviathan, 10);
        builder.SetCameraFollowDistance(16f);
        builder.SetCameraPositionOffset(new Vector3(8, 0, 15));
        builder.Finish();

        builder.Create(TechType.GhostLeviathan, MorphAnimationType.Leviathan, 15);
        builder.SetCameraFollowDistance(20);
        builder.Finish();

        builder.Create(TechType.SeaDragon, MorphAnimationType.Leviathan, 20);
        builder.SetCameraFollowDistance(30);
        builder.Finish();

        if (ModExists("ProjectAncients"))
        {
            // gargantuan leviathan...
        }
    }

    private static bool ModExists(string id) => Chainloader.PluginInfos.ContainsKey(id);

    // automatically called within MorphBuilder!
    public static void RegisterMorphType(MorphType morph, params TechType[] creatureTechTypes)
    {
        entries.Add(new Entry(morph, creatureTechTypes));
    }

    public static MorphType GetMorphType(TechType creatureTechType)
    {
        foreach (var entry in entries)
            foreach (var tt in entry.CreatureTechTypes)
                if (tt == creatureTechType) return entry.Morph;
        return null;
    }

    public static List<Entry> GetAllMorphTypes()
    {
        return entries;
    }

    public readonly struct Entry
    {
        public TechType[] CreatureTechTypes { get; }
        public MorphType Morph { get; }

        public Entry(MorphType morph, params TechType[] creatureTechTypes)
        {
            CreatureTechTypes = creatureTechTypes;
            Morph = morph;
        }

        public TechType MainTechType => CreatureTechTypes[0];

        public MorphType GetMorphType() => MorphDatabase.GetMorphType(MainTechType);
    }
}