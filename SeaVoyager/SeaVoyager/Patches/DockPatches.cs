using HarmonyLib;
using SeaVoyager.Mono;
using UnityEngine;

namespace SeaVoyager.Patches;

[HarmonyPatch]
public static class DockPatches
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(SeaMoth), nameof(SeaMoth.SetPlayerInside))]
    public static void PrintSeamothControls(SeaMoth __instance, bool inside)
    {
        if (!inside) return;
        PrintVehicleControls(__instance);
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(Exosuit), nameof(SeaMoth.OnPlayerEntered))]
    public static void PrintExosuitControls(Exosuit __instance)
    {
        PrintVehicleControls(__instance);
    }


    [HarmonyPostfix]
    [HarmonyPatch(typeof(Player), nameof(Player.SpawnNearby))]
    public static void SpawnNearbyOnSeaVoyager(GameObject ignoreObject, ref bool __result)
    {
        var vehicle = ignoreObject.GetComponent<Vehicle>();
        if (vehicle == null) return;
        if (Player.main.GetVehicle() != vehicle)
        {
            return;
        }

        var hbc = vehicle.GetComponent<HeldByCable>();
        if (!hbc) return;
        if (!hbc.Docked) return;
        if (hbc.dock.ship == null) return;
        if (Vector3.Distance(hbc.dock.transform.position, hbc.transform.position) > 15f)
        {
            return;
        }

        __result = true;
        Player.main.SetPosition(hbc.dock.ship.gameObject.FindChild("ExosuitExit").transform.position);
        Player.main.transform.parent = null;
    }

    private static void PrintVehicleControls(Vehicle vehicle)
    {
        var heldByCable = vehicle.gameObject.GetComponent<HeldByCable>();
        if (heldByCable == null) return;
        if (heldByCable.dock == null) return;
        SuspendedDock.PrintExoCustomControls();
    }
}