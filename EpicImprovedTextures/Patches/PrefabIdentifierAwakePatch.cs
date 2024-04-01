using HarmonyLib;
using UnityEngine;

namespace EpicImprovedTextures.Patches;

[HarmonyPatch(typeof(PrefabIdentifier))]
public static class PrefabIdentifierAwakePatch
{
    [HarmonyPatch(nameof(PrefabIdentifier.Awake))]
    [HarmonyPostfix]
    public static void PrefabIdentifierAwakePostfix(PrefabIdentifier __instance)
    {
        var database = TextureDatabase.GetInstance();
        foreach (var renderer in __instance.gameObject.GetComponentsInChildren<Renderer>(true))
        {
            TextureUtils.ConvertRenderer(renderer, database);
        }
    }
}