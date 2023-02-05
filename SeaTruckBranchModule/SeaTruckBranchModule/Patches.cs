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

    [HarmonyPatch(typeof(ConstructorInput), nameof(ConstructorInput.OnCraftingBegin))]
    [HarmonyPrefix]
    public static void ConstructorOnCraftingBeginPatch(ref float duration)
    {
        duration = 15f;
    }

    [HarmonyPatch(typeof(ConstructorInput), nameof(ConstructorInput.GetCraftTransform))]
    [HarmonyPostfix]
    public static void ConstructorGetCraftTransformPatch(ConstructorInput __instance, TechType techType, ref Vector3 position, ref Quaternion rotation)
    {
        if (techType == Plugin.branchModulePrefab.TechType)
        {
            __instance.GetCraftTransform(TechType.SeaTruckFabricatorModule, ref position, ref rotation);
        }
    }
}
