using HarmonyLib;

namespace AggressiveFauna.Patchers;

[HarmonyPatch(typeof(FleeOnDamage))]
[HarmonyPatch("OnTakeDamage")]
internal class FleePatcher
{
    [HarmonyPrefix]
    public static bool OnTakeDamage(FleeOnDamage __instance, DamageInfo damageInfo)
    {
        if (!AggressionSettings.DisableFleeing)
        {
            return true;
        }
        if (!AggressionSettings.DisableElectricityFlee && damageInfo.type == DamageType.Electrical)
        {
            return true;
        }
        if (__instance.gameObject.GetComponent<AttackLastTarget>() != null) // aggressive fish should not flee (most fish will have that component)
        {
            return false;
        }
        return true; // non-aggressive fish can still flee
    }
}