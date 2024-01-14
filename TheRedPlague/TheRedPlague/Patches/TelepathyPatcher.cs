using HarmonyLib;
using UnityEngine;

namespace TheRedPlague.Patches;

[HarmonyPatch(typeof(TelepathyScreenFXController))]
public static class TelepathyPatcher
{
    [HarmonyPatch(nameof(TelepathyScreenFXController.Start))]
    [HarmonyPostfix]
    public static void StartPostfix(TelepathyScreenFXController __instance)
    {
        __instance.fx.mat.SetColor("_Color", Color.red);
        __instance.fx.mat.SetColor("_ColorStrength", Color.red);
        var renderer = __instance.fx.leviathanGhostPrefab.GetComponent<Renderer>();
        var materials = renderer.materials;
        foreach (var material in materials)
        {
            material.color = Color.red;
        }

        renderer.materials = materials;
    }
}