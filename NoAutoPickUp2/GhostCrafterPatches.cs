using HarmonyLib;

namespace NoAutoPickUp2;

[HarmonyPatch(typeof(GhostCrafter))]
public static class GhostCrafterPatches
{
    [HarmonyPatch(nameof(GhostCrafter.OnCraftingEnd))]
    [HarmonyPrefix]
    public static bool OnCraftingEnd_Prefix(GhostCrafter __instance)
    {
        var tree = __instance.craftTree;
        var shouldDisable = Logic.ShouldDisableAutoPickupForCraftTree(tree);
        if (shouldDisable)
        {
            return false; // skip original method
        }
        return true; // don't skip original method
    }
}