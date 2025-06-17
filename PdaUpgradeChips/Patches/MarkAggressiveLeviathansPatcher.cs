using HarmonyLib;
using PdaUpgradeChips.MonoBehaviours;

namespace PdaUpgradeChips.Patches;

[HarmonyPatch(typeof(Creature))]
internal static class MarkAggressiveLeviathansPatcher
{
    [HarmonyPatch(nameof(Creature.Start))]
    [HarmonyPostfix]
    public static void CreatureStartPostfix(Creature __instance)
    {
        var ecoTarget = __instance.GetComponent<EcoTarget>();
        if (ecoTarget == null)
            return;
        if (ecoTarget.type != EcoTargetType.Leviathan &&
            CreatureData.GetBehaviourType(__instance.gameObject) != BehaviourType.Leviathan)
            return;
        var aggressive = __instance.GetComponent<AggressiveWhenSeeTarget>();
        var attack = __instance.GetComponent<AttackLastTarget>();
        if (aggressive == null && attack == null)
            return;
        __instance.gameObject.EnsureComponent<AggressiveLeviathanTracker>();
    }
}