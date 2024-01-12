using HarmonyLib;
using Story;
using TheRedPlague.Mono;

namespace TheRedPlague.Patches;

[HarmonyPatch(typeof(LaunchRocket))]
public static class LaunchRocketPatch
{
    [HarmonyPatch(nameof(LaunchRocket.OnHandClick))]
    [HarmonyPrefix]
    public static void OnHandClickPrefix(LaunchRocket __instance)
    {
        if (!__instance.IsRocketReady())
        {
            return;
        }
        if (LaunchRocket.launchStarted)
        {
            return;
        }

        if (!StoryGoalManager.main.IsGoalComplete(StoryUtils.OpenAquariumTeleporterGoalKey))
        {
            return;
        }

        // allow the rocket to be launched without that step!
        StoryGoalCustomEventHandler.main.gunDisabled = true;
    }
    
    [HarmonyPatch(nameof(LaunchRocket.OnHandClick))]
    [HarmonyPostfix]
    public static void OnHandClickPostfix(LaunchRocket __instance)
    {
        if (!LaunchRocket.launchStarted)
        {
            return;
        }

        var dome = InfectionDomeController.main;
        if (dome != null)
        {
            dome.OnBeginRocketAnimation();
        }
        
        if (StoryGoalManager.main.IsGoalComplete(StoryUtils.OpenAquariumTeleporterGoalKey) && !StoryGoalManager.main.IsGoalComplete(StoryUtils.ForceFieldLaserDisabled.key))
        {
            RocketSeaEmperor.PlayCinematic(__instance);
        }
    }
}