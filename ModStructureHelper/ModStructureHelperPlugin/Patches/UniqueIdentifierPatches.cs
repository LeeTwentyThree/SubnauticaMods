using HarmonyLib;

namespace ModStructureHelperPlugin.Patches;

[HarmonyPatch(typeof(UniqueIdentifier))]
public static class UniqueIdentifierPatches
{
    [HarmonyPostfix]
    [HarmonyPatch(nameof(UniqueIdentifier.Awake))]
    public static void AwakePostfix(UniqueIdentifier __instance)
    {
        if (__instance is not PrefabIdentifier prefabIdentifier)
        {
            return;
        }
        // add the spawned object to the current structure if it was in the data
    }
}