using CreatureMorphs.Mono;

namespace CreatureMorphs.Patches;

[HarmonyPatch(typeof(OxygenManager))]
internal static class OxygenManagerPatches
{
    [HarmonyPatch(nameof(OxygenManager.Update))]
    [HarmonyPostfix]
    public static void UpdatePostfix(OxygenManager __instance)
    {
        if (PlayerMorpher.main == null || PlayerMorpher.main.GetCurrentMorph() == null) return;
        if (PlayerMorpher.main.GetCurrentMorph().transform.position.y < Ocean.GetOceanLevel())
        {
            __instance.AddOxygen(15 * Time.deltaTime);
        }
    }
}