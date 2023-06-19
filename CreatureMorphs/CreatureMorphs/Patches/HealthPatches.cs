using CreatureMorphs.Mono;

namespace CreatureMorphs.Patches;

[HarmonyPatch(typeof(uGUI_HealthBar))]
internal static class HealthPatches
{
    [HarmonyPatch(nameof(uGUI_HealthBar.LateUpdate))]
    [HarmonyPostfix]
    public static void LateUpdatePostfix(uGUI_HealthBar __instance)
    {
        if (PlayerMorpher.PlayerCurrentlyMorphed)
        {
            var morph = PlayerMorpher.main.GetCurrentMorph();
            if (morph != null && morph.liveMixin != null)
            {
                var lm = morph.liveMixin;
                float actualHealth = lm.health - Mathf.Clamp(lm.tempDamage, 0, float.MaxValue);
                __instance.curr = Mathf.Clamp01(actualHealth / lm.maxHealth);
                __instance.SetValue(actualHealth, lm.maxHealth);
                __instance.curr = Mathf.Clamp01(actualHealth / lm.maxHealth);
            }
        }
    }
}