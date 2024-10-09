using HarmonyLib;
using Nautilus.Utility;
using Story;

namespace TheRedPlague.Patches;

[HarmonyPatch(typeof(VFXPrecursorGunElevator))]
public static class PlayElevatorMusicPatch
{
    private static readonly FMODAsset FirstUseMusic = AudioUtils.GetFmodAsset("IslandElevatorFirst");
    private static readonly FMODAsset GeneralUseMusic = AudioUtils.GetFmodAsset("IslandElevator");

    [HarmonyPatch(typeof(VFXPrecursorGunElevator), nameof(VFXPrecursorGunElevator.OnGunElevatorStart))]
    [HarmonyPostfix]
    public static void UseElevatorPostfix(VFXPrecursorGunElevator __instance)
    {
        if (StoryGoalManager.main.IsGoalComplete(StoryUtils.UseElevatorGoal.key))
        {
            Utils.PlayFMODAsset(GeneralUseMusic, __instance.transform.position);
        }
        else
        {
            Utils.PlayFMODAsset(FirstUseMusic, __instance.transform.position);
            StoryGoalManager.main.OnGoalComplete(StoryUtils.UseElevatorGoal.key);
        }
    }
}