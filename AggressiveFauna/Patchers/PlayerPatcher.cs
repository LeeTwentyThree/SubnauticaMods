using HarmonyLib;

namespace AggressiveFauna.Patchers;

[HarmonyPatch(typeof(Player))]
[HarmonyPatch(nameof(Player.Start))]
internal class PlayerPatcher
{
    [HarmonyPostfix]
    public static void Start(Player __instance)
    {
        __instance.gameObject.EnsureComponent<ApocalypseWarning>();
    }
}
