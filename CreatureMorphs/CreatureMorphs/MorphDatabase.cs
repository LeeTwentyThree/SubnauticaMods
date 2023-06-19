using BepInEx.Bootstrap;

namespace CreatureMorphs;

public static class MorphDatabase
{
    internal static void Setup()
    {
        AddBuiltInEntries();
    }

    private static readonly List<Entry> entries = new List<Entry>();

    private static void AddBuiltInEntries()
    {
        var builder = new MorphBuilder();

        builder.Create(TechType.Biter, MorphModeType.Prey);
        builder.AddAbility<Bite>((b) => { });
        builder.Finish();

        builder.Create(TechType.Stalker, MorphModeType.Shark);
        builder.AddAbility<Bite>((b) => { });
        builder.Finish();

        builder.Create(TechType.GhostLeviathan, MorphModeType.Leviathan);
        builder.Finish();

        builder.Create(TechType.CrabSquid, MorphModeType.Shark);
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
            foreach (var tt in entry.creatureTechTypes)
                if (tt == creatureTechType) return entry.morph;
        return null;
    }

    public static List<Entry> GetAllMorphTypes()
    {
        return entries;
    }

    public struct Entry
    {
        public TechType[] creatureTechTypes;
        public MorphType morph;

        public Entry(MorphType morph, params TechType[] creatureTechTypes)
        {
            this.creatureTechTypes = creatureTechTypes;
            this.morph = morph;
        }

        public TechType MainTechType => creatureTechTypes[0];
    }
}