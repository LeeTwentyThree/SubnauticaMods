using System.Collections.Generic;
using DeathContainer.Mono;
using HarmonyLib;

namespace DeathContainer;

[HarmonyPatch]
internal static class Patches
{
    [HarmonyPrefix]
    [HarmonyPatch(typeof(Inventory), nameof(Inventory.LoseItems))]
    public static void LoseItemsPrefix()
    {
        DeathContainerBehaviour.droppedItems = new List<Pickupable>();
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(Inventory), nameof(Inventory.LoseItems))]
    public static void LoseItemsPostfix(ref bool __result)
    {
        if (__result)
        {
            DeathContainerBehaviour.SpawnDeathContainer(Player.main.transform.position);
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(Inventory), nameof(Inventory.InternalDropItem))]
    public static void InternalDropItemPostfix(ref bool __result, Pickupable pickupable)
    {
        if (__result && IsPlayerDead() && DeathContainerBehaviour.droppedItems != null)
        {
            DeathContainerBehaviour.droppedItems.Add(pickupable);
        }
    }

    private static bool IsPlayerDead()
    {
        var ugui = uGUI.main;
        if (ugui == null) return false;
        return ugui.respawning.loadingBackground.sequence.target; // target: the screen is fading to black/already is black
    }
}
