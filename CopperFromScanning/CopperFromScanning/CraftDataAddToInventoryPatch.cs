namespace CopperFromScanning.Patches;

using HarmonyLib;

[HarmonyPatch(typeof(CraftData), nameof(CraftData.AddToInventory))]
internal class CraftDataAddToInventoryPatch
{
    [HarmonyPrefix]
    private static bool Prefix(TechType techType, int num, bool noMessage, bool spawnIfCantAdd)
    {
        // weird compatibility thing I guess
        if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("IngredientsFromScanning") ||
            techType != TechType.Titanium ||
            num != 2 ||
            noMessage ||
            !spawnIfCantAdd)
            return true;

        CraftData.AddToInventory(TechType.Titanium);
        CraftData.AddToInventory(TechType.Copper);
        return false;
    }
}