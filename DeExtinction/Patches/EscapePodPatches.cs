#if SUBNAUTICA
using DeExtinction.Mono;
using HarmonyLib;

namespace DeExtinction.Patches;

[HarmonyPatch(typeof(EscapePod))]
public static class EscapePodPatches
{
    [HarmonyPatch(nameof(EscapePod.Awake))]
    [HarmonyPostfix]
    public static void EscapePodAwakePatch(EscapePod __instance)
    {
        __instance.gameObject.AddComponent<SpawnDragonfliesAroundEscapePod>();
    }
}
#endif