using System.Linq;
using HarmonyLib;
using UnityEngine;

namespace TheRedPlague.Patches;

[HarmonyPatch(typeof(Player))]
public static class PlayerPatcher
{
    [HarmonyPatch(nameof(Player.Start))]
    [HarmonyPostfix]
    public static void OnEnablePostfix()
    {
        MainCamera.camera.farClipPlane = 99999;
    }
}