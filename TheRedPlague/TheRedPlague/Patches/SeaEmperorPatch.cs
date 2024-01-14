using System.Linq;
using HarmonyLib;
using Story;
using UnityEngine;

namespace TheRedPlague.Patches;

[HarmonyPatch(typeof(SeaEmperor))]
public static class SeaEmperorPatch
{
    [HarmonyPatch(nameof(SeaEmperor.Start))]
    [HarmonyPostfix]
    public static void StartPostfix(SeaEmperor __instance)
    {
        if (StoryGoalManager.main.IsGoalComplete(StoryUtils.OpenAquariumTeleporterGoalKey))
        {
            __instance.gameObject.SetActive(false);
            return;
        }
        ZombieManager.InfectSeaEmperor(__instance.gameObject);
    }
}