using HarmonyLib;
using UnityEngine;

namespace TheRedPlague.Patches;

[HarmonyPatch(typeof(Eatable))]
public static class EatablePatches
{
    // pls don't copy this lol, this basically signifies to the code that this is a biochemical protection module
    public const int MagicBiochemicalProtectionModuleDespawnDelayAmount = 3194139;
    
    [HarmonyPatch(nameof(Eatable.GetSecondaryTooltip))]
    public static void GetSecondaryTooltipPostfix(Eatable __instance, ref string __result)
    {
        if (Mathf.Approximately(__instance.despawnDelay, MagicBiochemicalProtectionModuleDespawnDelayAmount))
        {
            __result = Language.main.Get("BiochemicalProtectionModule");
        }
    }
}