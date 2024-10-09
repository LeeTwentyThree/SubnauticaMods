using System.Collections;
using System.Collections.Generic;
using Nautilus.Handlers;
using Nautilus.Utility;
using Story;
using TheRedPlague.Mono;
using TheRedPlague.PrefabFiles.Equipment;
using UnityEngine;

namespace TheRedPlague;

public class StoryUtils
{
    #region ACT 1

    public static CompoundGoal AngelinaIntroductionEvent { get; private set; }
    public static CompoundGoal BiochemicalProtectionSuitUnlockEvent { get; private set; }
    public static StoryGoal UseBiochemicalProtectionSuitEvent { get; private set; }
    public static CompoundGoal TransfuserMissionEvent { get; private set; }
    public static StoryGoal UnlockTransfuserEvent { get; private set; }
    public static StoryGoal TransfuserSampleTakenEvent { get; private set; }
    public static CompoundGoal AngelinaFirstSampleShuttlePadInstructions { get; private set; }
    public static StoryGoal DomeConstructionEvent { get; private set; }
    public static CompoundGoal DomeSpeakEvent { get; private set; }
    public static CompoundGoal DomeConstructionFinishedEvent { get; private set; }

    #endregion

    // FUTURE ACTS
    public static StoryGoal AuroraThrusterEvent { get; private set; }
    public static ItemGoal PlagueHeartGoal { get; private set; }
    public static StoryGoal HiveMindReleasedGoal { get; private set; }
    public static StoryGoal ForceFieldLaserDisabled { get; private set; }
    public static StoryGoal EnzymeRainEnabled { get; private set; }
    public static StoryGoal PickUpInfectedEnzyme { get; private set; }
    public static StoryGoal DisableDome { get; private set; }
    public static StoryGoal InfectCables { get; private set; }

    #region GENERAL

    public static StoryGoal UseElevatorGoal { get; private set; }
    public static BiomeGoal EnterPlagueDunesGoal { get; private set; }

    #endregion

    public const string OpenAquariumTeleporterGoalKey = "PrecursorPrisonAquariumFinalTeleporterActive";

    #region ASSETS

    private static Sprite VoiceLogIconAlterra { get; } = Plugin.AssetBundle.LoadAsset<Sprite>("LogIcon-Alterra");

    // private static Sprite VoiceLogIconPerson { get; } = Plugin.AssetBundle.LoadAsset<Sprite>("LogIcon-Person");
    private static Sprite VoiceLogIconPda { get; } = Plugin.AssetBundle.LoadAsset<Sprite>("LogIcon-PDA");
    // reserve this for non-alterra comms
    // private static Sprite VoiceLogIconRadio { get; } = Plugin.AssetBundle.LoadAsset<Sprite>("LogIcon-Radio");

    #endregion

    public static void RegisterStory()
    {
        RegisterAct1();

        AuroraThrusterEvent = new StoryGoal("AuroraThrusterEvent", Story.GoalType.Story, 0);
        PlagueHeartGoal = StoryGoalHandler.RegisterItemGoal("Pickup_Plague_Heart", Story.GoalType.Story,
            ModPrefabs.PlagueHeart.TechType);
        HiveMindReleasedGoal = new StoryGoal("HiveMindReleased", Story.GoalType.Story, 0);
        ForceFieldLaserDisabled = new StoryGoal("Disable_Infection_Laser", Story.GoalType.Story, 0);
        EnzymeRainEnabled = new StoryGoal("Enzyme_Rain_Started", Story.GoalType.Story, 0);
        PickUpInfectedEnzyme = StoryGoalHandler.RegisterItemGoal("Pickup_Infected_Enzyme", Story.GoalType.Story,
            ModPrefabs.EnzymeParticleInfo.TechType);
        DisableDome = new StoryGoal("Disable_Dome", Story.GoalType.Story, 0);
        InfectCables = new StoryGoal("InfectCables", Story.GoalType.Story, 0);
        StoryGoalHandler.RegisterOnGoalUnlockData("Pickup_Infected_Enzyme",
            new UnlockBlueprintData[]
            {
                new UnlockBlueprintData()
                {
                    techType = ModPrefabs.EnzymeContainer.TechType,
                    unlockType = UnlockBlueprintData.UnlockType.Available
                }
            });
        StoryGoalHandler.RegisterCustomEvent(EscapeAquariumCinematic.PlayCinematic);
        StoryGoalHandler.RegisterCustomEvent(PlayerInfectionDeath.PlayCinematic);

        UseElevatorGoal = new StoryGoal("UseIslandElevator", Story.GoalType.Story, 0);
        EnterPlagueDunesGoal =
            StoryGoalHandler.RegisterBiomeGoal("EnterPlagueDunes", Story.GoalType.PDA, "dunes", 1, 3);
        RegisterVoiceLog("EnterPlagueDunes", "EnterPlagueDunes", VoiceLogIconPda);
    }

    private static void RegisterAct1()
    {
        AngelinaIntroductionEvent = StoryGoalHandler.RegisterCompoundGoal("AngelinaIntroduction", Story.GoalType.Radio,
            300, "OnPlayRadioBounceBack");
        RegisterVoiceLog("AngelinaIntroduction", "AngelinaIntroduction", VoiceLogIconAlterra);

        BiochemicalProtectionSuitUnlockEvent = StoryGoalHandler.RegisterCompoundGoal(
            "PDABiochemicalProtectionSuitUnlock", Story.GoalType.PDA,
            50, "OnPlayAngelinaIntroduction");
        RegisterVoiceLog("PDABiochemicalProtectionSuitUnlock", "PDABiochemicalProtectionSuitUnlock", VoiceLogIconPda);
        StoryGoalHandler.RegisterOnGoalUnlockData("PDABiochemicalProtectionSuitUnlock", new[]
        {
            new UnlockBlueprintData
            {
                techType = BiochemicalProtectionSuit.Info.TechType,
                unlockType = UnlockBlueprintData.UnlockType.Available
            }
        });

        UseBiochemicalProtectionSuitEvent = new StoryGoal("PDABiochemSuitEquipped", Story.GoalType.PDA, 0f);
        RegisterVoiceLog("PDABiochemSuitEquipped", "PDABiochemSuitEquipped", VoiceLogIconPda);

        TransfuserMissionEvent =
            StoryGoalHandler.RegisterCompoundGoal("TransfuserMission", Story.GoalType.PDA, 14,
                "PDABiochemSuitEquipped");
        RegisterVoiceLog("TransfuserMission", "TransfuserMission", VoiceLogIconAlterra);

        UnlockTransfuserEvent = new StoryGoal("PDATransfuserUnlocked", Story.GoalType.PDA, 2);
        RegisterVoiceLog("PDATransfuserUnlocked", "PDATransfuserUnlocked", VoiceLogIconPda);
        InfectionSamplerTool.RegisterLateStoryData();

        TransfuserSampleTakenEvent = new StoryGoal("PDAInfectionSampleTaken", Story.GoalType.PDA, 0.5f);
        RegisterVoiceLog("PDAInfectionSampleTaken", "PDAInfectionSampleTaken", VoiceLogIconPda);

        AngelinaFirstSampleShuttlePadInstructions =
            StoryGoalHandler.RegisterCompoundGoal("AngelinaFirstSampleShuttlePadInstructions", Story.GoalType.PDA, 30,
                "PDAInfectionSampleTaken");
        RegisterVoiceLog("AngelinaFirstSampleShuttlePadInstructions", "AngelinaFirstSampleShuttlePadInstructions",
            VoiceLogIconAlterra);

        DomeConstructionEvent = new StoryGoal("DomeConstructionEvent", Story.GoalType.Story, 0);

        DomeSpeakEvent =
            StoryGoalHandler.RegisterCompoundGoal("DomeSpeakEvent", Story.GoalType.PDA, 37, "DomeConstructionEvent");
        RegisterVoiceLog("DomeSpeakEvent", "DomeVoiceV1", VoiceLogIconAlterra);

        DomeConstructionFinishedEvent = StoryGoalHandler.RegisterCompoundGoal("DomeConstructionFinishedEvent",
            Story.GoalType.Story, 45, "DomeConstructionEvent");
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

    private static void RegisterVoiceLog(string id, string clipName, Sprite icon)
    {
        var sound = AudioUtils.CreateSound(Plugin.AssetBundle.LoadAsset<AudioClip>(clipName),
            AudioUtils.StandardSoundModes_2D);

        CustomSoundHandler.RegisterCustomSound(id, sound, AudioUtils.BusPaths.VoiceOvers);

        PDAHandler.AddLogEntry(id, id, AudioUtils.GetFmodAsset(id), icon);
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