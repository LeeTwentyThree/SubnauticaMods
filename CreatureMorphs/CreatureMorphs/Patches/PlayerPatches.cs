using CreatureMorphs.Mono;

namespace CreatureMorphs.Patches;

[HarmonyPatch(typeof(Player))]
internal static class PlayerPatches
{
    [HarmonyPatch(nameof(Player.Start))]
    [HarmonyPostfix]
    public static void StartPostfix(Player __instance)
    {
        __instance.gameObject.AddComponent<PlayerMorpher>();
    }
}