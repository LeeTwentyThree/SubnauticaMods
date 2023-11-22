using System.Collections;
using HarmonyLib;
using UnityEngine;
using UWE;
using WeatherMod.Mono;

namespace WeatherMod.Patches;

[HarmonyPatch(typeof(VFXClouds), nameof(VFXClouds.LateUpdate))]
public static class RemoveIslandClouds
{
    [HarmonyPostfix]
    public static void Postfix(VFXClouds __instance)
    {
        var t = __instance.transform.parent;
        if (t != null && t.GetChild(1).gameObject.activeSelf)
        {
            t.GetChild(1).gameObject.SetActive(false);
            t.GetChild(2).gameObject.SetActive(false);
            t.GetChild(3).gameObject.SetActive(false);
            t.GetChild(4).gameObject.SetActive(false);
        }
    }
}