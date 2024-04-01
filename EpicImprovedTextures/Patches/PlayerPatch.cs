using HarmonyLib;
using UnityEngine;

namespace EpicImprovedTextures.Patches;

[HarmonyPatch(typeof(Player))]
public static class PlayerPatch
{
    [HarmonyPatch(nameof(Player.Start))]
    [HarmonyPostfix]
    public static void PlayerStartPostfix(Player __instance)
    {
        var textureUpdaterObject = new GameObject("TextureUpdater");
        textureUpdaterObject.AddComponent<PeriodicallyUpdateRenderers>();

        var database = TextureDatabase.GetInstance();
        foreach (var renderer in __instance.gameObject.GetComponentsInChildren<Renderer>(true))
        {
            TextureUtils.ConvertRenderer(renderer, database);
        }
    }
}