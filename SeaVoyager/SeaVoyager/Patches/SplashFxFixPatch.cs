using HarmonyLib;
using UnityEngine;

namespace SeaVoyager.Patches;

[HarmonyPatch(typeof(VFXConstructing))]
public static class SplashFxFixPatch
{
    [HarmonyPrefix]
    [HarmonyPatch(nameof(VFXConstructing.PlaySplashFX))]
    public static bool PlaySplashFXPrefix(VFXConstructing __instance)
    {
        if (!IsSeaVoyager(__instance.gameObject))
        {
            return true;
        }

        var obj = Utils.SpawnPrefabAt(__instance.surfaceSplashFX, __instance.transform, __instance.transform.position);
        obj.transform.localEulerAngles = __instance.localRotation;
        obj.transform.Rotate(Vector3.up, 90, Space.World);
        obj.transform.position += new Vector3(0, -10, 0);
        obj.SendMessage("Play", SendMessageOptions.DontRequireReceiver);
        return false;
    }

    private static bool IsSeaVoyager(GameObject obj)
    {
        return obj.GetComponent<Mono.SeaVoyager>() != null;
    }
}