using CreatureMorphs.Mono;

namespace CreatureMorphs.Patches;
[HarmonyPatch(typeof(OxygenManager))]
internal static class OxygenManagerPatches
{
    [HarmonyPatch(nameof(OxygenManager.Update))]
    [HarmonyPostfix]
    public static void UpdatePostfix(OxygenManager __instance)
    {
        if (Morphing.main.GetCurrentMorph().transform.position.y < Ocean.GetOceanLevel())
        {
            __instance.AddOxygen(15 * Time.deltaTime);
        }
    }
}