using System.Collections;
using Nautilus.Handlers;
using Nautilus.Utility;
using Story;
using TheRedPlague.Mono;
using UnityEngine;

namespace TheRedPlague;

public class StoryUtils
{
    public static CompoundGoal AngelinaIntroductionEvent { get; private set; }
    public static CompoundGoal BiochemicalProtectionSuitUnlockEvent { get; private set; }
    public static StoryGoal DomeConstructionEvent { get; private set; }
    public static StoryGoal AuroraThrusterEvent { get; private set; }
    public static ItemGoal PlagueHeartGoal { get; private set; }
    public static StoryGoal ForceFieldLaserDisabled { get; private set; }
    public static StoryGoal EnzymeRainEnabled { get; private set; }
    public static StoryGoal PickUpInfectedEnzyme { get; private set; }
    public static StoryGoal DisableDome { get; private set; }
    public static StoryGoal InfectCables { get; private set; }
    
    public const string OpenAquariumTeleporterGoalKey = "PrecursorPrisonAquariumFinalTeleporterActive";
    
    public static void RegisterStory()
    {
        RegisterAct1();
        
        AuroraThrusterEvent = new StoryGoal("AuroraThrusterEvent", Story.GoalType.Story, 0);
        PlagueHeartGoal = StoryGoalHandler.RegisterItemGoal("Pickup_Plague_Heart", Story.GoalType.Story, ModPrefabs.PlagueHeart.TechType);
        ForceFieldLaserDisabled = new StoryGoal("Disable_Infection_Laser", Story.GoalType.Story, 0);
        EnzymeRainEnabled = new StoryGoal("Enzyme_Rain_Started", Story.GoalType.Story, 0);
        PickUpInfectedEnzyme = StoryGoalHandler.RegisterItemGoal("Pickup_Infected_Enzyme", Story.GoalType.Story, ModPrefabs.EnzymeParticleInfo.TechType);
        DisableDome = new StoryGoal("Disable_Dome", Story.GoalType.Story, 0);
        InfectCables = new StoryGoal("InfectCables", Story.GoalType.Story, 0);
        StoryGoalHandler.RegisterOnGoalUnlockData("Pickup_Infected_Enzyme", new UnlockBlueprintData[] { new UnlockBlueprintData(){techType = ModPrefabs.EnzymeContainer.TechType, unlockType = UnlockBlueprintData.UnlockType.Available} });
        StoryGoalHandler.RegisterCustomEvent(EscapeAquariumCinematic.PlayCinematic);
        StoryGoalHandler.RegisterCustomEvent(PlayerInfectionDeath.PlayCinematic);
    }

    private static void RegisterAct1()
    {
        AngelinaIntroductionEvent = StoryGoalHandler.RegisterCompoundGoal("AngelinaIntroduction", Story.GoalType.Radio,
            300, "OnPlayRadioBounceBack");
        RegisterVoiceLog("AngelinaIntroduction", "AngelinaIntroduction");
        
        BiochemicalProtectionSuitUnlockEvent = StoryGoalHandler.RegisterCompoundGoal("BiochemicalProtectionSuitUnlock", Story.GoalType.PDA,
            45, "AngelinaIntroduction");
        RegisterVoiceLog("BiochemicalProtectionSuitUnlock", "BiochemicalProtectionSuitUnlock");
        StoryGoalHandler.RegisterOnGoalUnlockData("BiochemicalProtectionSuitUnlock", new []{new UnlockBlueprintData()
        {
            techType = TechType.Bioreactor,
            unlockType = UnlockBlueprintData.UnlockType.Available
        }});
        
        DomeConstructionEvent = new StoryGoal("DomeConstructionEvent", Story.GoalType.Story, 0);
    }

    public static void RegisterLanguageLines()
    {
        LanguageHandler.SetLanguageLine("InfectionLaserTerminal", "Infected terminal");
        LanguageHandler.SetLanguageLine("InfectionLaserReceptacle", "Multi-purpose receptacle");
        LanguageHandler.SetLanguageLine("InsertPlagueHeart", "Insert plague heart");
        LanguageHandler.SetLanguageLine("InsertEnzymeContainer", "Insert concentrated enzyme");
        LanguageHandler.SetLanguageLine("DisableDomePrompt", "Disable dome");
        LanguageHandler.SetLanguageLine("Ency_Infection", Language.main.Get("Ency_Infection_REPLACE"));
        LanguageHandler.SetLanguageLine("EncyDesc_Infection", Language.main.Get("EncyDesc_Infection_REPLACE"));
    }

    private static void RegisterVoiceLog(string id, string clipName)
    {
        var sound = AudioUtils.CreateSound(Plugin.AssetBundle.LoadAsset<AudioClip>(clipName), AudioUtils.StandardSoundModes_2D);

        CustomSoundHandler.RegisterCustomSound(id, sound, AudioUtils.BusPaths.VoiceOvers);
        
        PDAHandler.AddLogEntry(id, id, AudioUtils.GetFmodAsset(id));
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
    
    public static void DisableDomeEvent()
    {
        if (StoryGoalManager.main.IsGoalComplete(DisableDome.key))
            return;
        
        DisableDome.Trigger();
        Utils.PlayFMODAsset(AudioUtils.GetFmodAsset("DisableDomeSound"), new Vector3(0, 600, 0));
    }
}