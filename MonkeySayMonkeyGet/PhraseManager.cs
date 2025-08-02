using System.Collections.Generic;
using UnityEngine;

namespace MonkeySayMonkeyGet;

public static class PhraseManager
{
    public static Dictionary<string, KnownPhrase> knownPhrases;

    // essential phrases

    public const string Negative = "_negative";
    public const string Pronoun = "_pronoun";
    public const string PronounPlural = "_pronounplural";
    public const string PronounEvery = "_pronounevery";
    public const string Repeat = "_repeat";
    public const string Self = "_self";
    public const string Demand = "_demand";
    public const string Subnautica = "_subnautica";
    public const string You = "_you";
    public const string Speed = "_speed";
    public const string TechTypeAny = "_techtype";
    public const string Everything = "_everything";
    public const string Give = "_give";

    // triggers

    public const string Hello = "hello";
    public const string Fly = "fly";
    public const string Kill = "kill";
    public const string Die = "die";
    public const string Food = "food";
    public const string Water = "water";
    public const string Oxygen = "oxygen";
    public const string Stop = "stop";
    public const string Day = "day";
    public const string Night = "night";
    public const string Home = "home";
    public const string Lifepod = "lifepod";
    public const string Submarine = "submarine";
    public const string Move = "move";
    public const string Pain = "pain";
    public const string Drown = "drown";
    public const string BeatGame = "beat the game";
    public const string Heal = "heal";
    public const string Save = "save";
    public const string Surface = "surface";
    public const string Cyclops = "cyclops";
    public const string Blueprint = "blueprint";
    public const string Delete = "delete";
    public const string Explore = "explore";
    public const string StarPlatinum = "platinum";
    public const string TheWorld = "the world";
    public const string Jump = "jump";
    public const string Sound = "sound";
    public const string Talk = "talk";
    public const string Gross = "gross";
    public const string Starve = "starve";
    public const string Stuck = "stuck";
    public const string ExplodeAurora = "explode aurora";
    public const string Cancel = "cancel";

    // other classes

    private static TechTypePhrases techTypeData;

    public static TechTypePhrases TechTypeData
    {
        get
        {
            if (techTypeData == null)
            {
                techTypeData = new TechTypePhrases();
            }
            if (!techTypeData.Initialized)
            {
                techTypeData.Initialize();
                if (!techTypeData.Initialized)
                {
                    Debug.LogError("MonkeySayMonkeyGet TechTypeData FAILED to initialize!");
                }
            }
            return techTypeData;
        }
    }

    // main initialization

    public static void Initialize()
    {
        knownPhrases = new Dictionary<string, KnownPhrase>();

        // essential phrases:

        AddKnownPhrase(Negative, "not", "do not", "don't");
        AddKnownPhrase(Pronoun, "that", "it", "the", "this", "a");
        AddKnownPhrase(PronounPlural, "those");
        AddKnownPhrase(PronounEvery, "all", "every");
        AddKnownPhrase(Repeat, "repeat", "again", "more", "see that again", "what happened");
        AddKnownPhrase(Self, "me", "we", "myself", "us", "mean");
        AddKnownPhrase(Demand, "make me", "i want", "let me", "can i", "give me", "i need", "i must", "help me", "need", "gimme", "more", "please", "create", "build");
        AddKnownPhrase(Subnautica, "subnautica", "some nautica", "some monica", "so nautica", "nautica", "sub not", "should not ago");
        AddKnownPhrase(You, "you");
        AddKnownPhrase(Speed, "speed", "fast", "faster", "slow", "slower");
        AddKnownPhrase(Everything, "everything");
        AddKnownPhrase(Give, "give", "inventory", "need", "gimme", "can i have", "want", "have", "shit"); // don't ask

        // triggers:

        AddKnownPhrase(Hello, "shrek", "testing", "test", "schreck");
        AddKnownPhrase(Fly, "hover", "levitate", "flight");
        AddKnownPhrase(Kill, "murder", "take out", "take down", "slaughter", "die", "break", "destroy", "destroyed", "killed", "call", "color", "hell", "co", "attack", "fight");
        AddKnownPhrase(Die, "death", "dye", "i'm dead", "kill me");
        AddKnownPhrase(Food, "eat", "feed", "calorie");
        AddKnownPhrase(Water, "thirsty", "drink", "what her");
        AddKnownPhrase(Oxygen, "air", "breathe");
        AddKnownPhrase(Stop, "freeze");
        AddKnownPhrase(Day, "daytime", "bright", "sunny", "can't see", "cannot see", "too dark", "can not see", "hard to see", "need flashlight");
        AddKnownPhrase(Night, "nighttime", "dark", "too bright", "blind");
        AddKnownPhrase(Home, "house", "base", "go back", "bring me back", "return");
        AddKnownPhrase(Lifepod, "life pod", "escape pod", "life partner", "life spot", "life part", "live thought", "live pot", "life pot", "like pod", "live fire", "lifetime", "life pied");
        AddKnownPhrase(Submarine, "sub", "ship", "cyclops");
        AddKnownPhrase(Move, "send", "bring", "bringing", "take", "taking", "meters", "liters", "warp", "hemingway", "outta", "get", "warp", "go", "grab", "collect");
        AddKnownPhrase(Pain, "ouch", "ow", "hurts", "hurt", "hurting", "damage", "damaged", "damaging");
        AddKnownPhrase(Drown, "suffocate", "c o two");
        AddKnownPhrase(BeatGame, "win the game", "complete the game", "finish the game", "finishing the game", "beating the game");
        AddKnownPhrase(Heal, "health", "hp", "heel", "he'll");
        AddKnownPhrase(Blueprint, "recipe", "unlock", "know how", "hemlock");
        AddKnownPhrase(Delete, "unexist", "on exist");
        AddKnownPhrase(Explore, "exploring", "random place", "random location", "somewhere random");
        AddKnownPhrase(Surface, "service", "top");
        AddKnownPhrase(Jump, "hop", "leap", "bounce", "apple");
        AddKnownPhrase(Sound, "noise", "hear", "hearing", "heard", "loud", "music", "sounded", "sounding");
        AddKnownPhrase(Talk, "talking", "say", "saying", "said", "ramble");
        AddKnownPhrase(Gross, "nasty", "disgusting", "vomit", "sick", "sickness", "unhealthy", "icky", "ag");
        AddKnownPhrase(Starve, "starvation", "hunger", "hungry", "starved", "starving");
        AddKnownPhrase(Stuck, "still", "paralyze", "paralyzed", "cannot move", "can't move");
        AddKnownPhrase(ExplodeAurora, "explode ship", "blow up aurora", "blow up ship");

        AddKnownPhrase(TheWorld, "zaragoza", "saw riddle", "saw rudolph", "it's all ronaldo", "xia rudolph", "xia riddle", "za ward", "zola router");

        // alternate creature names:

        AddKnownPhrase("ampeel", "amp eel", "shocker", "eel");
        AddKnownPhrase("boneshark", "bone shark");
        AddKnownPhrase("crabsnake", "crab snake");
        AddKnownPhrase("crabsquid", "crab squid", "crab good", "crabs squid", "crabs when", "krebs would", "crabs would");
        AddKnownPhrase("crashfish", "crash", "exploding fish", "exploder", "kamikaze");
        AddKnownPhrase("river prowler", "river prowler", "spine eel");
        AddKnownPhrase("warper", "warfare", "war per", "warp or");
        AddKnownPhrase("bladderfish", "bladder fish", "blatter fish", "blotter fish", "ladder fish", "water fish");
        AddKnownPhrase("eyeye", "eye eye");
        AddKnownPhrase("cuddlefish", "cuttlefish", "cuddle", "cute");
        AddKnownPhrase("garryfish", "gary");
        AddKnownPhrase("gasopod", "gasifier", "gas upon", "fart", "gas apart", "gossip", "gas pod", "gas up on", "guys pod", "castle pod", "gas", "guess i pod");
        AddKnownPhrase("ghostray", "ghost ray");
        AddKnownPhrase("holefish", "hole fish", "whole fish");
        AddKnownPhrase("hoopfish", "hoop fish");
        AddKnownPhrase("hoverfish", "hover fish", "hover fist");
        AddKnownPhrase("jellyray", "jelly ray");
        AddKnownPhrase("magmarang", "murmuring");
        AddKnownPhrase("peeper", "paper", "peter", "keeper", "fever", "beaver", "beeper");
        AddKnownPhrase("skyray", "sky ray", "skyro", "bird", "seagull", "sea gull", "sky race", "sky right");
        AddKnownPhrase("spadefish", "spade fish");
        AddKnownPhrase("spinefish", "spine fish");
        AddKnownPhrase("bleeder", "bleed her");
        AddKnownPhrase("rockgrub", "grub", "rock grab");
        AddKnownPhrase("shuttlebug", "shuttle bug", "shadow blog", "shutterbug");
        AddKnownPhrase("rabbit ray", "rabbit race", "rabbit right", "robbery", "rabbit raise", "rabbit raised", "rabbit razor");
        AddKnownPhrase("cave crawler", "crab");
        AddKnownPhrase("stalker", "soccer", "stall for");

        // leviathans:

        AddKnownPhrase("ghost leviathan", "ghost", "go steve", "goes to leviathans");
        AddKnownPhrase("reefback", "reef back", "whale");
        AddKnownPhrase("sea dragon leviathan", "dragon", "drag on", "seadragon");
        AddKnownPhrase("sea treader", "treader", "sea shredder", "see shredder", "see shudder", "sea shudder", "sea charter");
        AddKnownPhrase("reaper leviathan", "reaper", "river");

        // modded creatures

        AddKnownPhrase("gargantuan leviathan adult", "adult garg", "adult gargantuan");
        AddKnownPhrase("gargantuan leviathan juvenile", "garg", "gargantuan");
        AddKnownPhrase("gargantuan leviathan baby", "baby garg", "baby gargantuan");
        AddKnownPhrase("the bloop", "bloop");
        AddKnownPhrase("the blaza", "blaza", "blossom", "lhasa", "blah", "plaza", "blogger");
        AddKnownPhrase("gulper leviathan", "gopher", "gulper", "gold per", "goalkeeper");
        AddKnownPhrase("axetail", "axe tail", "axe tale", "axed how");
        AddKnownPhrase("emerald clown pincher", "emerald venture");
        AddKnownPhrase("ruby clown pincher", "ruby venture");
        AddKnownPhrase("filtorb", "filter");
        AddKnownPhrase("trianglefish", "triangle fish");

        // tech:

        AddKnownPhrase("mobile vehicle bay", "constructor", "vehicle bay", "vehicle crafter");
        AddKnownPhrase("seaglide", "siegel i", "see guide", "see glide", "she glide", "she guide", "she died", "seagull i'd", "c glide", "see lied", "sea lied", "she lied", "c by the");
        AddKnownPhrase("seaglide fragment", "siegel i fragment", "see guide fragment", "see glide fragment", "she glide fragment", "she guide fragment", "she died fragment", "seagull i'd fragment", "c glide fragment", "see lied fragment", "sea lied fragment", "she lied fragment");
        AddKnownPhrase("grav trap", "graph trap", "graph drop");
        AddKnownPhrase("grav trap fragment", "graph trap fragment");
        AddKnownPhrase("wiring kit", "wiring cat", "wiring get");
        AddKnownPhrase("standard o\u2082 tank", "oxygen tank");
        AddKnownPhrase("habitat builder", "builder tool", "builder", "habitat tool", "base builder");
        AddKnownPhrase("survival knife", "knife");
        AddKnownPhrase("flashlight", "torch");
        AddKnownPhrase("repair tool", "welder");
        AddKnownPhrase("power cell", "power so");
        AddKnownPhrase("titanium ingot", "titanium anger");
        AddKnownPhrase("plasteel ingot", "place still", "place steel", "play skill", "play style");
        AddKnownPhrase("seamoth", "seem off", "c moth", "slim off", "theme of", "theme off", "sea moth", "see math");
        AddKnownPhrase("prawn suit", "prime suit", "brown suit", "pronto", "prawn", "prancer", "one suit", "prom suit", "bronx zoo");
        AddKnownPhrase("ion cube", "i en que", "i on cube", "i on cuba");
        AddKnownPhrase("metal salvage", "metal scrap", "metal chunk", "nettle salvage");
        AddKnownPhrase("multipurpose room", "multi purpose", "multipurpose", "big room");
        AddKnownPhrase("alterra sea voyager", "voyager", "boat");
        AddKnownPhrase("vortex torpedo", "vortex", "whirlpool");
        AddKnownPhrase("gas torpedo", "ammo");
        AddKnownPhrase("cargo crate", "crate", "cargo");
        AddKnownPhrase("aurora miniature", "souvenir");

        // other stuff
        AddKnownPhrase("alien feces", "poop", "shit", "crap", "bullshit");
        AddKnownPhrase("quartz", "courts");
        AddKnownPhrase("salt deposit", "salt");
        AddKnownPhrase("lead", "led");

        // plants
        AddKnownPhrase("acid mushroom", "mushroom", "much room", "mush room", "atom mushroom", "ass and mushroom", "awesome mushroom");
        AddKnownPhrase("brain coral", "brain core");
        AddKnownPhrase("table coral", "table cholera", "cable coral", "table girl");
        AddKnownPhrase("creepvine", "creep vine", "creep fine", "creep design", "creep divine");
        AddKnownPhrase("writhing weed", "reading weed", "ribbing weed", "writing reed");

        // outcrops
        AddKnownPhrase("limestone chunk", "limestone outcrop", "limestone");
        AddKnownPhrase("basalt chunk", "basalt outcrop");
        AddKnownPhrase("sandstone chunk", "sandstone outcrop", "sandstone");
        AddKnownPhrase("shale chunk", "shale", "shale outcrop");

        // biomes
        AddKnownPhrase("shallows", "safe shallows", "safe showers");
        AddKnownPhrase("void", "crater edge", "dead zone", "ocean", "empty");
        AddKnownPhrase("koosh", "bulb", "bob zone", "bolden zone", "kush");
        AddKnownPhrase("jelly shroom", "jelly", "jellies");
        AddKnownPhrase("dunes", "reaper territory", "barren", "sand");
        AddKnownPhrase("inactive lava", "i lz", "lava chamber", "lava corridor");
        AddKnownPhrase("floating island", "floater island", "for island");
        AddKnownPhrase("lava zone", "lava lakes", "prison", "primary containment facility");
        AddKnownPhrase("aurora", "crashed ship", "herrera", "harara");
        AddKnownPhrase("lost river", "last river", "bone cave", "underwater river", "skeleton cave", "lost", "last forever");
        AddKnownPhrase("disease research facility", "the rf", "research facility", "sunken base", "warper base");
        AddKnownPhrase("crater", "cratered");
        AddKnownPhrase("cove tree", "ghost tree", "giant tree");
        AddKnownPhrase("mountains", "mountain");

        // i make mistakes sometimes!

        if (Plugin.TestingModeActive)
        {
            CheckList();
        }
    }

    private static void CheckList()
    {
        foreach (var kP in knownPhrases)
        {
            if (kP.Key != kP.Key.ToLower())
            {
                ErrorInList($"{kP.Key} must be fully lowercase!");
            }
            foreach (var similar in kP.Value.Similar)
            {
                if (similar != similar.ToLower())
                {
                    ErrorInList($"{similar} must be fully lowercase!");
                }
            }
        }
    }

    private static void ErrorInList(string message)
    {
        ErrorMessage.AddMessage($"Error in initialization of phrase list (MonkeySayMonkeyGet): '{message}'");
    }

    private static void AddKnownPhrase(string literal, params string[] similar)
    {
        knownPhrases.Add(literal, new KnownPhrase(literal, similar));
    }

    // runtime checks

    public static bool ContainsPhrase(SpeechInput input, string phrase)
    {
        if (Utils.StringContainsAllWords(input.text, phrase))
        {
            return true;
        }
        foreach (var known in knownPhrases.Values)
        {
            if (known.Literal == phrase)
            {
                foreach (var similar in known.Similar)
                {
                    if (Utils.StringContainsAllWords(input.text, similar))
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    public static int GetReferencedNumber(SpeechInput input, int defaultNumber)
    {
        var found = new List<int>();
        foreach (var kvp in numberFromName)
        {
            if (input.text.Contains(kvp.Key))
            {
                found.Add(kvp.Value);
            }
        }
        if (found.Count == 0)
        {
            return defaultNumber;
        }
        var highestValue = int.MinValue;
        for (int i = 0; i < found.Count; i++)
        {
            if (found[i] > highestValue)
            {
                highestValue = found[i];
            }
        }
        return highestValue;
    }

    public static Subject GetSubject(SpeechInput input, bool recognizeTechTypes, Subject.Type fallback, bool everythingIsAnOption)
    {   
        if (recognizeTechTypes)
        {
            var referencedTechType = GetReferencedTechType(input, out Amount amount);
            if (referencedTechType != TechType.None)
            {
                if (amount == Amount.Singular)
                {
                    return new Subject(Utils.FindObjectsByTechType(referencedTechType, 1)[0]);
                }
                else
                {
                    return new Subject(Utils.FindObjectsByTechType(referencedTechType, Utils.GetObjectLimitForAmount(amount)).ToArray());
                }
            }
        }

        if (ContainsPhrase(input, Self))
        {
            return new Subject(Subject.Type.Self);
        }

        if (everythingIsAnOption && Plugin.ModConfig.CanMentionEverything)
        {
            if (ContainsPhrase(input, Everything))
            {
                return new Subject(Subject.Type.Everything);
            }
        }

        return new Subject(fallback);
    }

    public static Direction GetReferencedDirection(SpeechInput input)
    {
        if (input.text.Contains("to me") || input.text.Contains("to us") || input.text.Contains("to here") || input.text.Contains("right to") || input.text.Contains("collect"))
        {
            return Direction.ToSelf;
        }
        foreach (var kvp in directionFromName)
        {
            if (input.text.Contains(kvp.Key))
            {
                return kvp.Value;
            }
        }
        return Direction.None;
    }

    public static SpeedChange GetReferencedSpeedChange(SpeechInput input)
    {
        foreach (var kvp in speedChangeFromName)
        {
            if (input.text.Contains(kvp.Key))
            {
                return kvp.Value;
            }
        }
        return SpeedChange.None;
    }

    public static ScaleChange GetReferencedScaleChange(SpeechInput input)
    {
        foreach (var kvp in scaleChangeFromName)
        {
            if (input.text.Contains(kvp.Key))
            {
                return kvp.Value;
            }
        }
        return ScaleChange.None;
    }

    public static POI GetReferencedPOI(SpeechInput input)
    {
        foreach (var target in goToTargets)
        {
            if (ContainsPhrase(input, target.name))
            {
                return target;
            }
        }
        return default;
    }

    public static TechType GetReferencedTechType(SpeechInput input, out Amount amount)
    {
        amount = Amount.Singular;
        if (ContainsPhrase(input, PronounPlural))
        {
            amount = Amount.Plural;
        }
        if (ContainsPhrase(input, PronounEvery))
        {
            amount = Amount.All;
        }
        if (Plugin.ModConfig.CanMentionEverything)
        {
            if (ContainsPhrase(input, Everything))
            {
                amount = Amount.LiterallyEverything;
            }
        }
        var techType = TechType.None;
        foreach (var entry in TechTypeData.entries)
        {
            if (ContainsPhrase(input, entry.literal))
            {
                techType = entry.techType;
                break;
            }
        }
        return techType;
    }

    // static data

    public static Dictionary<string, Direction> directionFromName = new Dictionary<string, Direction>
    {
        { "forward", Direction.Forward },
        { "for", Direction.Forward },
        { "foreign", Direction.Forward },
        { "ahead", Direction.Forward },
        { "in front", Direction.Forward },
        { "away", Direction.Forward },
        { "out of my face", Direction.Forward },
        { "back", Direction.Back },
        { "closer", Direction.Back },
        { "towards", Direction.Back },
        { "behind", Direction.Back },
        { "backwards", Direction.Back },
        { "left", Direction.Left },
        { "right", Direction.Right },
        { "up", Direction.Up },
        { "op", Direction.Up },
        { "sky", Direction.Up },
        { "rise", Direction.Up },
        { "down", Direction.Down },
        { "below", Direction.Down },
        { "out", Direction.Random },
        { "outta", Direction.Random },
        { "surface", Direction.Surface },
    };

    public static Dictionary<string, SpeedChange> speedChangeFromName = new Dictionary<string, SpeedChange>
    {
        { "fast", SpeedChange.Faster },
        { "faster", SpeedChange.Faster },
        { "speed", SpeedChange.Faster },
        { "slower", SpeedChange.Slower },
        { "slow", SpeedChange.Slower },
        { "snail", SpeedChange.Slower },
        { "slug", SpeedChange.Slower },
        { "sluggish", SpeedChange.Slower },
        { "default", SpeedChange.Reset },
        { "reset", SpeedChange.Reset },
        { "normal", SpeedChange.Reset },
    };

    public static Dictionary<string, ScaleChange> scaleChangeFromName = new Dictionary<string, ScaleChange>
    {
        { "big", ScaleChange.Bigger },
        { "large", ScaleChange.Bigger },
        { "huge", ScaleChange.Bigger },
        { "giant", ScaleChange.Bigger },
        { "massive", ScaleChange.Bigger },
        { "grow", ScaleChange.Bigger },
        { "growth", ScaleChange.Bigger },
        { "enlarge", ScaleChange.Bigger },

        { "small", ScaleChange.Smaller },
        { "tiny", ScaleChange.Smaller },
        { "little", ScaleChange.Smaller },
        { "miniature", ScaleChange.Smaller },
        { "mini", ScaleChange.Smaller },

        { "along", ScaleChange.Longer },
        { "long", ScaleChange.Longer },
        { "stretch", ScaleChange.Longer },
        { "short", ScaleChange.Shorter },
        { "compress", ScaleChange.Shorter },
        { "fat", ScaleChange.Shorter },
        { "thick", ScaleChange.Shorter },
    };

    public static List<POI> goToTargets = new List<POI>()
    {
        new POI("void", new Vector3(-1864, -100, 9)),
        new POI("shallows", new Vector3(-86, -4, -339)),
        new POI("kelp", new Vector3(-304, -31, -94)),
        new POI("grassy", new Vector3(-677, -73, -65)),
        new POI("mushroom forest", new Vector3(-781, -128, 576)),
        new POI("koosh", new Vector3(1260, -150, 604)),
        new POI("jelly shroom", new Vector3(-688, -209, -26)),
        new POI("sparse reef", new Vector3(-705, -224, -711)),
        new POI("grand reef", new Vector3(-755, -320, -1180)),
        new POI("dunes", new Vector3(-1511, -90, 301)),
        new POI("mountains", new Vector3(-1082, -158, 1212)),
        new POI("enforcement platform", new Vector3(361, 0, 1130)),
        new POI("blood kelp", new Vector3(-956, -347, -544)),
        new POI("underwater islands", new Vector3(-82, -77, 707)),
        new POI("crag field", new Vector3(-151, -170, -1143)),
        new POI("inactive lava", new Vector3(-224, -1200, 146)),
        new POI("floating island", new Vector3(-831, 0, -969)),
        new POI("lava zone", new Vector3(177, -1393, -85)),
        new POI("aurora", new Vector3(1353, -40, 368)),
        new POI("lava castle", new Vector3(-30, -1199, 72)),
        new POI("lost river", new Vector3(-742, -674, -148)),
        new POI("disease research facility", new Vector3(-228, -792, 322)),
        new POI("crater", new Vector3(1144, -329, 1147)),
        new POI("cove tree", new Vector3(-924, -845, 423)),
    };

    public static Dictionary<string, int> numberFromName = new Dictionary<string, int>()
    {
        { "one", 1},
        { "single", 1},
        { "two", 2},
        { "couple", 2},
        { "three", 3},
        { "four", 41},
        { "five", 5},
        { "six", 6},
        { "seven", 7},
        { "many", 7},
        { "eight", 8},
        { "nine", 9},
        { "ten", 10},
        { "a lot", 10},
        { "eleven", 11},
        { "twelve", 12},
        { "dozen", 12},
        { "thirteen", 13},
        { "fourteen", 14},
        { "fifteen", 15},
        { "sixteen", 16},
        { "seventeen", 17},
        { "eighteen", 18 },
        { "nineteen", 19},
        { "twenty", 20},
        { "tons", 20},
        { "twenty one", 21},
        { "twenty two", 22},
        { "twenty three", 23},
        { "twenty four", 24},
        { "twenty five", 25},
        { "twenty six", 26},
        { "twenty seven", 27},
        { "twenty eight", 28},
        { "twenty nine", 29},
        { "thirty", 30},
        { "thirty one", 31},
        { "thirty two", 32},
        { "thirty three", 33},
        { "thirty four", 34},
        { "thirty five", 35},
        { "thirty six", 36},
        { "thirty seven", 37},
        { "thirty eight", 38},
        { "thirty nine", 39},
        { "forty", 40},
        { "forty one", 41},
        { "forty two", 42},
        { "forty three", 43},
        { "forty four", 44},
        { "forty five", 45},
        { "forty six", 46},
        { "forty seven", 47},
        { "forty eight", 48},
        { "forty nine", 49},
        { "fifty", 50},
        { "sixty", 60},
        { "sixty nine", 69},
        { "seventy", 70},
        { "ninety", 80},
        { "hundred", 100},
        { "hundred fifty", 150},
        { "two hundred", 200},
        { "three hundred", 300},
        { "four hundred", 400},
        { "five hundred", 500},
        { "six hundred", 600},
        { "seven hundred", 700},
        { "eight hundred", 800},
        { "nine hundred", 900},
        { "thousand", 1000},
    };
}


public struct KnownPhrase
{
    public string Literal;
    public string[] Similar;

    public KnownPhrase(string literal, string[] similar)
    {
        Literal = literal;
        Similar = similar;
    }
}

public struct Subject
{
    public GameObject gameObject;
    public GameObject[] gameObjectArray;
    public Type type;

    public Subject(GameObject other)
    {
        type = Type.Other;
        gameObject = other;
        gameObjectArray = null;
    }

    public Subject(GameObject[] otherArray)
    {
        type = Type.OtherArray;
        gameObject = null;
        gameObjectArray = otherArray;
    }

    public Subject(Type type)
    {
        this.type = type;
        if (type == Type.Self)
        {
            gameObject = Player.main.gameObject;
            gameObjectArray = null;
        }
        else if (type == Type.Everything)
        {
            gameObject = null;
            gameObjectArray = Utils.FindObjectsByTechType(TechType.None, 500).ToArray();
        }
        else
        {
            gameObject = null;
            gameObjectArray = null;
        }
    }

    public List<GameObject> ToList(int max)
    {
        var list = new List<GameObject>();
        if (max < 1)
        {
            return list;
        }
        if (gameObject != null)
        {
            list.Add(gameObject);
        }
        if (gameObjectArray != null)
        {
            if (list.Count >= max)
            {
                return list;
            }
            foreach (var obj in gameObjectArray)
            {
                if (obj != null)
                {
                    list.Add(obj);
                }
            }
        }
        return list;
    }

    public enum Type
    {
        None,
        Self,
        Other,
        OtherArray,
        Everything
    }
}

public enum Direction
{
    None,
    Forward,
    Back,
    Up,
    Down,
    Left,
    Right,
    Random,
    Surface,
    ToSelf
}

public enum ScaleChange
{
    None,
    Bigger,
    Smaller,
    Longer,
    Shorter
}

public enum SpeedChange
{
    None,
    Faster,
    Slower,
    Reset
}

public struct POI
{
    public string name;
    public Vector3 coords;
    public bool IsValid { get { return !string.IsNullOrEmpty(name); } }

    public POI(string name, Vector3 coords)
    {
        this.name = name;
        this.coords = coords;
    }
}

public enum Amount
{
    Singular,
    Plural,
    All,
    LiterallyEverything
}
