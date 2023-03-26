using CreatureMorphs.Mono;

namespace CreatureMorphs.Patches;
[HarmonyPatch(typeof(HandReticle))]
internal static class HandReticlePatches
{
    [HarmonyPatch(nameof(HandReticle.ShouldHideAll))]
    [HarmonyPostfix]
    public static void ShouldHideAllPostfix(HandReticle __instance, ref bool __result)
    {
        if (Morphing.PlayerCurrentlyMorphed)
        {
            __result = true;
        }
    }
}