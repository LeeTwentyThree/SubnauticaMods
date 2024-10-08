using HarmonyLib;
using TheRedPlague.PrefabFiles.Equipment;
using UnityEngine;

namespace TheRedPlague.Patches;

[HarmonyPatch(typeof(Survival))]
public static class SurvivalPatches
{
    [HarmonyPatch(nameof(Survival.Eat))]
    public static void EatPostfix(GameObject useObj)
    {
        if (useObj == null) return;
        var techType = CraftData.GetTechType(useObj);
        if (techType == BiochemicalProtectionSuit.Info.TechType)
        {
            StoryUtils.UseBiochemicalProtectionSuitEvent.Trigger();
        }
    }
}