using HarmonyLib;

namespace EndlessVehicleNames;

[HarmonyPatch]
internal static class Patches
{
    [HarmonyPatch(typeof(SubNameInput), nameof(SubNameInput.Start))]
    [HarmonyPostfix]
    public static void SubNameInputStartPostfix(SubNameInput __instance)
    {
        if (__instance.inputField != null)
        {
            __instance.inputField.characterLimit = 0;
        }
    }

    [HarmonyPatch(typeof(SubName), nameof(SubName.Awake))]
    [HarmonyPostfix]
    public static void SubNameAwakePostfix(SubName __instance)
    {
        if (__instance.hullName != null)
        {
            __instance.hullName.fontSizeMin = 1;
        }
    }
}