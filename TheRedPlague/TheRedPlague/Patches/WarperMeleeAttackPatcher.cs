using HarmonyLib;
using UnityEngine;

namespace TheRedPlague.Patches;

[HarmonyPatch(typeof(WarperMeleeAttack))]
public static class WarperMeleeAttackPatcher
{
    [HarmonyPatch(nameof(WarperMeleeAttack.CanBite))]
    [HarmonyPostfix]
    public static void CanBitePostfix(WarperMeleeAttack __instance, GameObject target, ref bool __result)
    {
        if (__result == false && target.GetComponent<Creature>() != null)
        {
            __result = true;
        }
    }
}