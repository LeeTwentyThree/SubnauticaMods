using Nautilus.Assets;
using Nautilus.Assets.PrefabTemplates;
using Nautilus.Handlers;
using Nautilus.Utility;
using Story;

namespace TheRedPlague.PrefabFiles;

public static class TurretConsolePrefab
{
    public static PrefabInfo Info { get; } = PrefabInfo.WithTechType("DriveRoomTurretConsole");

    public static void Register()
    {
        var prefab = new CustomPrefab(Info);
        var cloneTemplate = new CloneTemplate(Info, "0dbfbfbf-23f4-4506-9c49-5db80299d072");

        var goal = new StoryGoal("UnlockTurretFromConsole", Story.GoalType.PDA, 4f);
        PDAHandler.AddLogEntry(goal.key, "UnlockTurretScream", AudioUtils.GetFmodAsset("UnlockTurretScream"));
        
        StoryGoalHandler.RegisterOnGoalUnlockData(goal.key, new UnlockBlueprintData[]
        {
            new() {techType = ModCompatibility.TurretTechType, unlockType = UnlockBlueprintData.UnlockType.Available}
        });
        
        cloneTemplate.ModifyPrefab += obj =>
        {
            obj.GetComponent<StoryHandTarget>().goal = goal;
        };
        prefab.SetGameObject(cloneTemplate);
        prefab.Register();
    }
}