using CreatureMorphs.Mono;

namespace CreatureMorphs.Patches;

[HarmonyPatch(typeof(HandReticle))]
internal static class HandReticlePatches
{
    [HarmonyPatch(nameof(HandReticle.Start))]
    [HarmonyPostfix]
    public static void HandReticleStartPostfix(HandReticle __instance)
    {
        __instance.gameObject.AddComponent<DisableReticleInMorph>();
    }
}