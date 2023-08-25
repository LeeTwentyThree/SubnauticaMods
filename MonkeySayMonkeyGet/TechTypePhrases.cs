using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonkeySayMonkeyGet;

public class TechTypePhrases
{
    public List<Entry> entries;

    private bool initialized;

    public bool Initialized { get { return initialized; } }

    public struct Entry
    {
        public TechType techType;
        public string literal;

        public Entry(TechType techType, string literal)
        {
            this.techType = techType;
            this.literal = literal;
        }
    }

    public bool Initialize()
    {
        if (!CanInitialize())
        {
            return false;
        }
        CheckAllTechTypes();
        initialized = true;
        return true;
    }

    private void CheckAllTechTypes()
    {
        entries = new List<Entry>();
        var language = Language.main;

        // vanilla

        var vanillaTechTypes = Enum.GetValues(typeof(TechType)).Cast<TechType>();
        ProcessTechTypes(language, vanillaTechTypes);

        // modded

        TechTypeCacheReader cacheReader = new TechTypeCacheReader();
        var read = cacheReader.Read();
        if (read)
        {
            foreach (var entry in cacheReader.entries)
            {
                if (language.Contains(entry.TechType))
                {
                    var localizedName = language.Get(entry.TechType).ToLower();
                    var withoutUnneccessaryWords = RemoveUnnecessaryWordsFromLocalizedName(localizedName);
                    entries.Add(new Entry(entry.TechType, withoutUnneccessaryWords));
                }
            }
        }
        else
        {
            ErrorMessage.AddMessage("MonkeySayMonkeyGet: Failed to load modded TechTypes.");
        }

        if (Plugin.config.EnableDebugMode)
        {
            ErrorMessage.AddMessage(string.Format("MonkeySayMonkeyGet: Patched {0} TechType names. Open mod settings to disable this notification.", entries.Count));
        }
    }

    private void ProcessTechTypes(Language language, IEnumerable<TechType> techTypes)
    {
        foreach (var techType in techTypes)
        {
            if (language.Contains(techType))
            {
                var localizedName = language.Get(techType).ToLower();
                var withoutUnneccessaryWords = RemoveUnnecessaryWordsFromLocalizedName(localizedName);
                entries.Add(new Entry(techType, withoutUnneccessaryWords));
            }
        }
    }

    private string RemoveUnnecessaryWordsFromLocalizedName(string localizedName)
    {
        var wordsList = localizedName.Split(' ');
        var newWordsList = new List<string>();
        foreach (var word in wordsList)
        {
            if (!unnecessaryWords.Contains(word))
            {
                newWordsList.Add(word);
            }
        }
        var sb = new StringBuilder();
        for (var i = 0; i < newWordsList.Count; i++)
        {
            if (i > 0)
            {
                sb.Append(" ");
            }
            sb.Append(newWordsList[i]);
        }
        return sb.ToString();
    }

    private List<string> unnecessaryWords = new List<string>()
    {
        "ore", "module"
    };

    private bool CanInitialize()
    {
        return Language.main != null && Language.main.strings != null && Language.main.strings.Count > 0;
    }

    public LinkedTechTypes GetLinkedTechTypes(TechType single)
    {
        foreach (var ltt in linkedTechTypes)
        {
            if (ltt.ContainsTechType(single))
            {
                return ltt;
            }
        }
        return new LinkedTechTypes(single);
    }

    public List<LinkedTechTypes> linkedTechTypes = new List<LinkedTechTypes>()
    {
        new LinkedTechTypes(TechType.GhostLeviathan, TechType.GhostLeviathanJuvenile),
        new LinkedTechTypes(TechType.SeaEmperorJuvenile, TechType.SeaEmperor),
        new LinkedTechTypes(TechType.Reefback, TechType.ReefbackBaby),
        new LinkedTechTypes(TechType.AcidMushroom, TechType.WhiteMushroom),
        new LinkedTechTypes(TechType.CaveCrawler, TechType.Shuttlebug),
        new LinkedTechTypes(TechType.AluminumOxide, TechType.DrillableAluminiumOxide),
        new LinkedTechTypes(TechType.Copper, TechType.DrillableCopper),
        new LinkedTechTypes(TechType.Diamond, TechType.DrillableDiamond),
        new LinkedTechTypes(TechType.Gold, TechType.DrillableGold),
        new LinkedTechTypes(TechType.Kyanite, TechType.DrillableKyanite),
        new LinkedTechTypes(TechType.Lead, TechType.DrillableLead),
        new LinkedTechTypes(TechType.Lithium, TechType.DrillableLithium),
        new LinkedTechTypes(TechType.Magnetite, TechType.DrillableMagnetite),
        new LinkedTechTypes(TechType.MercuryOre, TechType.DrillableMercury),
        new LinkedTechTypes(TechType.Nickel, TechType.DrillableNickel),
        new LinkedTechTypes(TechType.Salt, TechType.DrillableSalt),
        new LinkedTechTypes(TechType.Silver, TechType.DrillableSilver),
        new LinkedTechTypes(TechType.Sulphur, TechType.DrillableSulphur),
        new LinkedTechTypes(TechType.Titanium, TechType.DrillableTitanium),
        new LinkedTechTypes(TechType.UraniniteCrystal, TechType.DrillableUranium)
    };
}

public struct LinkedTechTypes
{
    public TechType[] types;

    public LinkedTechTypes(params TechType[] types)
    {
        this.types = types;
    }

    public bool ContainsTechType(TechType techType)
    {
        foreach (var t in types)
        {
            if (t == techType)
            {
                return true;
            }
        }
        return false;
    }
}
