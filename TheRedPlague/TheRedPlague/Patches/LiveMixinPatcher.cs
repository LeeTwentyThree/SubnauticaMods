using HarmonyLib;
using TheRedPlague.Mono;
using TheRedPlague.Mono.FleshBlobs;
using UnityEngine;

namespace TheRedPlague.Patches;

[HarmonyPatch(typeof(LiveMixin))]
public class LiveMixinPatcher
{
    [HarmonyPatch(nameof(LiveMixin.Awake))]
    [HarmonyPostfix]
    public static void AwakePostfix(LiveMixin __instance)
    {
        var rb = __instance.GetComponent<Rigidbody>();
        if (rb != null && !rb.isKinematic)
            __instance.gameObject.AddComponent<FleshBlobInfluenced>();
    }
    
    [HarmonyPatch(nameof(LiveMixin.TakeDamage))]
    [HarmonyPrefix]
    public static bool TakeDamagePrefix(LiveMixin __instance, float originalDamage, GameObject dealer)
    {
        if (originalDamage <= 0 || dealer == null)
            return true;
        if (__instance.GetComponent<Creature>() == null)
            return true;
        var damagedByZombie = ZombieManager.IsZombie(dealer);
        if (!damagedByZombie || dealer.GetComponent<FriendlyWarper>() != null)
            return true;
        if (ZombieManager.IsZombie(__instance.gameObject)) // am I already a zombie?
            return false;
        ZombieManager.Zombify(__instance.gameObject);
        return false;
    }
}