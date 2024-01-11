using System.Linq;
using HarmonyLib;
using TheRedPlague.Mono;
using UnityEngine;

namespace TheRedPlague.Patches;

[HarmonyPatch(typeof(Player))]
public static class PlayerPatcher
{
    [HarmonyPatch(nameof(Player.Start))]
    [HarmonyPostfix]
    public static void StartPostfix(Player __instance)
    {
        MainCamera.camera.farClipPlane = 99999;
        __instance.gameObject.EnsureComponent<RandomFishSpawner>();
        __instance.gameObject.EnsureComponent<JumpScares>();
    }
}