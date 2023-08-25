using UnityEngine;
using HarmonyLib;
using MonkeySayMonkeyGet.Mono;

namespace MonkeySayMonkeyGet.Patches;

[HarmonyPatch]
public static class PlayerPatches
{
    [HarmonyPatch(typeof(Player), nameof(Player.Start))]
    [HarmonyPostfix]
    public static void StartPatch()
    {
        VoiceListener.CreateInstance();
    }
}
