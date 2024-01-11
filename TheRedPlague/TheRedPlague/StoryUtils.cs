using Nautilus.Handlers;
using Story;

namespace TheRedPlague;

public class StoryUtils
{
    public static ItemGoal PlagueHeartGoal { get; private set; }
    public static StoryGoal ForceFieldLaserDisabled { get; private set; }
    public static StoryGoal EnzymeRainEnabled { get; private set; }
    
    public static void RegisterStory()
    {
        PlagueHeartGoal = StoryGoalHandler.RegisterItemGoal("Pickup_Plague_Heart", Story.GoalType.Story, ModPrefabs.PlagueHeart.TechType);
        ForceFieldLaserDisabled = new StoryGoal("Disable_Infection_Laser", Story.GoalType.Story, 0);
        EnzymeRainEnabled = new StoryGoal("Enzyme_Rain_Started", Story.GoalType.Story, 0);
    }

    public static void RegisterLanguageLines()
    {
        LanguageHandler.SetLanguageLine("InfectionLaserTerminal", "Infected terminal");
    }
}