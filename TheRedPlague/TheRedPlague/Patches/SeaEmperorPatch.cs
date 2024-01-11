using System.Linq;
using HarmonyLib;
using UnityEngine;

namespace TheRedPlague.Patches;

[HarmonyPatch(typeof(SeaEmperor))]
public static class SeaEmperorPatch
{
    [HarmonyPatch(nameof(SeaEmperor.Start))]
    [HarmonyPostfix]
    public static void StartPostfix(SeaEmperor __instance)
    {
        ZombieManager.InfectSeaEmperor(__instance.gameObject);
    }
}