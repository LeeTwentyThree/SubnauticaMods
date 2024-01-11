using System.Linq;
using HarmonyLib;
using Story;
using TheRedPlague.Mono;
using UnityEngine;

namespace TheRedPlague.Patches;

[HarmonyPatch(typeof(LaunchRocket))]
public static class LaunchRocketPatch
{
    [HarmonyPatch(nameof(LaunchRocket.OnHandClick))]
    [HarmonyPostfix]
    public static void OnHandClickPostfix(LaunchRocket __instance)
    {
        if (LaunchRocket.launchStarted && !StoryGoalManager.main.IsGoalComplete(StoryUtils.ForceFieldLaserDisabled.key))
        {
            RocketSeaEmperor.PlayCinematic(__instance);
        }
    }
}