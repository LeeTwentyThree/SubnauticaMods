using Nautilus.Handlers;
using Story;
using TheRedPlague.Mono;

namespace TheRedPlague;

public class StoryUtils
{
    public static ItemGoal PlagueHeartGoal { get; private set; }
    public static StoryGoal ForceFieldLaserDisabled { get; private set; }
    public static StoryGoal EnzymeRainEnabled { get; private set; }
    public const string OpenAquariumTeleporterGoalKey = "PrecursorPrisonAquariumFinalTeleporterActive";
    
    public static void RegisterStory()
    {
        PlagueHeartGoal = StoryGoalHandler.RegisterItemGoal("Pickup_Plague_Heart", Story.GoalType.Story, ModPrefabs.PlagueHeart.TechType);
        ForceFieldLaserDisabled = new StoryGoal("Disable_Infection_Laser", Story.GoalType.Story, 0);
        EnzymeRainEnabled = new StoryGoal("Enzyme_Rain_Started", Story.GoalType.Story, 0);
        StoryGoalHandler.RegisterCustomEvent(EscapeAquariumCinematic.PlayCinematic);
    }

    public static void RegisterLanguageLines()
    {
        LanguageHandler.SetLanguageLine("InfectionLaserTerminal", "Infected terminal");
        LanguageHandler.SetLanguageLine("InfectionLaserReceptacle", "Multi-purpose receptacle");
        LanguageHandler.SetLanguageLine("InsertPlagueHeart", "Insert infection core");
    }
}