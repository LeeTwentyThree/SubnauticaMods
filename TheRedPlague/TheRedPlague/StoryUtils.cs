using System.Collections;
using Nautilus.Handlers;
using Nautilus.Utility;
using Story;
using TheRedPlague.Mono;
using UnityEngine;

namespace TheRedPlague;

public class StoryUtils
{
    public static ItemGoal PlagueHeartGoal { get; private set; }
    public static StoryGoal ForceFieldLaserDisabled { get; private set; }
    public static StoryGoal EnzymeRainEnabled { get; private set; }
    public static StoryGoal PickUpInfectedEnzyme { get; private set; }
    public static StoryGoal DisableDome { get; private set; }
    
    public const string OpenAquariumTeleporterGoalKey = "PrecursorPrisonAquariumFinalTeleporterActive";
    
    public static void RegisterStory()
    {
        PlagueHeartGoal = StoryGoalHandler.RegisterItemGoal("Pickup_Plague_Heart", Story.GoalType.Story, ModPrefabs.PlagueHeart.TechType);
        ForceFieldLaserDisabled = new StoryGoal("Disable_Infection_Laser", Story.GoalType.Story, 0);
        EnzymeRainEnabled = new StoryGoal("Enzyme_Rain_Started", Story.GoalType.Story, 0);
        PickUpInfectedEnzyme = StoryGoalHandler.RegisterItemGoal("Pickup_Infected_Enzyme", Story.GoalType.Story, ModPrefabs.EnzymeParticleInfo.TechType);
        DisableDome = new StoryGoal("Disable_Dome", Story.GoalType.Story, 0);
        StoryGoalHandler.RegisterOnGoalUnlockData("Pickup_Infected_Enzyme", new UnlockBlueprintData[] { new UnlockBlueprintData(){techType = ModPrefabs.EnzymeContainer.TechType, unlockType = UnlockBlueprintData.UnlockType.Available} });
        StoryGoalHandler.RegisterCustomEvent(EscapeAquariumCinematic.PlayCinematic);
    }

    public static void RegisterLanguageLines()
    {
        LanguageHandler.SetLanguageLine("InfectionLaserTerminal", "Infected terminal");
        LanguageHandler.SetLanguageLine("InfectionLaserReceptacle", "Multi-purpose receptacle");
        LanguageHandler.SetLanguageLine("InsertPlagueHeart", "Insert plague heart");
        LanguageHandler.SetLanguageLine("InsertEnzymeContainer", "Insert concentrated enzyme");
    }

    public static void DisableInfectionLaser()
    {
        if (StoryGoalManager.main.IsGoalComplete(ForceFieldLaserDisabled.key))
            return;
        
        ForceFieldLaserDisabled.Trigger();
        Utils.PlayFMODAsset(AudioUtils.GetFmodAsset("DisableDomeSound"), new Vector3(-75.89f, 323.22f, -56.99f));
        UWE.CoroutineHost.StartCoroutine(SpawnDeadEmperor());
    }

    private static IEnumerator SpawnDeadEmperor()
    {
        var task = CraftData.GetPrefabForTechTypeAsync(ModPrefabs.DeadSeaEmperorSpawnerInfo.TechType);
        yield return task;
        var spawned = Object.Instantiate(task.GetResult(), new Vector3(-54.508f, 20, -42.000f), Quaternion.identity);
        spawned.SetActive(true);
        LargeWorldStreamer.main.cellManager.RegisterGlobalEntity(spawned);
    }
    
    public static void StartInfectionRain()
    {
        if (StoryGoalManager.main.IsGoalComplete(EnzymeRainEnabled.key))
            return;
        
        EnzymeRainEnabled.Trigger();
        Utils.PlayFMODAsset(AudioUtils.GetFmodAsset("DisableDomeSound"), new Vector3(-75.89f, 323.22f, -56.99f));
    }
}