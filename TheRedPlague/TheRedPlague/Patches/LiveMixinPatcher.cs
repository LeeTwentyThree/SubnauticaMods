using HarmonyLib;
using UnityEngine;

namespace TheRedPlague.Patches;

[HarmonyPatch(typeof(LiveMixin))]
public class LiveMixinPatcher
{
    [HarmonyPatch(nameof(LiveMixin.TakeDamage))]
    [HarmonyPostfix]
    public static void TakeDamagePostfix(LiveMixin __instance, float originalDamage, GameObject dealer)
    {
        if (originalDamage <= 0 || dealer == null)
            return;
        if (__instance.GetComponent<Creature>() == null)
            return;
        if (!ZombieManager.IsZombie(dealer))
            return;
        if (ZombieManager.IsZombie(__instance.gameObject))
            return;
        ZombieManager.Zombify(__instance.gameObject);
    }
}