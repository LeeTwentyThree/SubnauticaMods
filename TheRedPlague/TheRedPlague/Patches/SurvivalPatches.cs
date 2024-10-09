using HarmonyLib;
using TheRedPlague.PrefabFiles.Equipment;
using UnityEngine;

namespace TheRedPlague.Patches;

[HarmonyPatch(typeof(Survival))]
public static class SurvivalPatches
{
    [HarmonyPatch(nameof(Survival.Eat))]
    [HarmonyPostfix]
    public static void EatPostfix(GameObject useObj)
    {
        if (useObj == null) return;
        var techType = CraftData.GetTechType(useObj);
        if (techType != BiochemicalProtectionSuit.Info.TechType)
        {
            return;
        }
        StoryUtils.UseBiochemicalProtectionSuitEvent.Trigger();
        var pickupable = useObj.GetComponent<Pickupable>();
        if (pickupable) Inventory.main.TryRemoveItem(pickupable);
    }
}