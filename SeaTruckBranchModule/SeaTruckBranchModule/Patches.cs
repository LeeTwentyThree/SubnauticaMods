namespace SeaTruckBranchModule;

[HarmonyPatch]
internal static class Patches
{
    [HarmonyPatch(typeof(SeaTruckConnection), nameof(SeaTruckConnection.RotationMatches))]
    [HarmonyPostfix]
    public static void SeaTruckConnectionRotationMatchesPatch(ref bool __result)
    {
        __result = true;
    }
}
