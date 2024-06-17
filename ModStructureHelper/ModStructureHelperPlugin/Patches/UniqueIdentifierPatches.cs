using HarmonyLib;

namespace ModStructureHelperPlugin.Patches;

[HarmonyPatch(typeof(UniqueIdentifier))]
public static class UniqueIdentifierPatches
{
    [HarmonyPostfix]
    [HarmonyPatch(nameof(UniqueIdentifier.Register))]
    public static void RegisterPostfix(UniqueIdentifier __instance)
    {
        if (__instance is not PrefabIdentifier prefabIdentifier)
        {
            return;
        }

        /*
        if (!__instance.isActiveAndEnabled)
        {
            return;
        }
        */
        
        // add the spawned object to the current structure if it was in the data
        if (StructureInstance.Main != null)
        {
            StructureInstance.Main.RegisterExistingEntity(prefabIdentifier);
        }
    }
}