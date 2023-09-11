using HarmonyLib;
using System.Collections.Generic;

namespace LoseEverything;

[HarmonyPatch]
internal static class Patches
{
    [HarmonyPrefix()]
    [HarmonyPatch(typeof(Inventory), nameof(Inventory.LoseItems))]
    public static bool InventoryLoseItemsPrefix(Inventory __instance, ref bool __result)
    {
        if (!Plugin.Config.LoseItemsOnDeath && !Plugin.Config.LoseEquipmentOnDeath)
        {
            return true;
        }
        var list = new List<InventoryItem>();
        foreach (InventoryItem inventoryItem in __instance.container)
        {
            if (Plugin.Config.LoseItemsOnDeath || inventoryItem.item.destroyOnDeath)
            {
                if (Plugin.Config.KeepToolsOnDeath && inventoryItem.item.gameObject.GetComponent<PlayerTool>() != null)
                {
                    continue;
                }
                list.Add(inventoryItem);
            }
        }
        foreach (InventoryItem inventoryItem2 in (IItemsContainer)__instance.equipment)
        {
            if (Plugin.Config.LoseEquipmentOnDeath || inventoryItem2.item.destroyOnDeath)
            {
                list.Add(inventoryItem2);
            }
        }
        __result = false;
        for (int i = 0; i < list.Count; i++)
        {
            if (__instance.InternalDropItem(list[i].item, false))
            {
                __result = true;
            }
        }
        return false;
    }
}
