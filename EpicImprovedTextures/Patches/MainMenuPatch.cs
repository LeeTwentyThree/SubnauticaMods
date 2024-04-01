using HarmonyLib;
using UnityEngine;

namespace EpicImprovedTextures.Patches;

[HarmonyPatch(typeof(uGUI_MainMenu))]
public static class MainMenuPatch
{
    [HarmonyPatch(nameof(uGUI_MainMenu.Start))]
    [HarmonyPostfix]
    public static void MainMenuStartPostfix()
    {
        var textureUpdaterObject = new GameObject("TextureUpdater");
        textureUpdaterObject.AddComponent<PeriodicallyUpdateRenderers>();
    }
}