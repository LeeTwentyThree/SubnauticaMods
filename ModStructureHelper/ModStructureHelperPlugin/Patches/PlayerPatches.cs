using HarmonyLib;
using ModStructureHelperPlugin.Mono;
using UnityEngine;

namespace ModStructureHelperPlugin.Patches;

[HarmonyPatch(typeof(Player))]
public static class PlayerPatches
{
    [HarmonyPatch(nameof(Player.Start))]
    [HarmonyPostfix]
    public static void StartPostfix()
    {
        var inputHandlerObj = new GameObject("StructureHelperInputHandler");
        inputHandlerObj.AddComponent<InputHandler>();
    }
}