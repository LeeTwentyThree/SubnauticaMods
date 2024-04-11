using HarmonyLib;
using UnityEngine;

namespace EpicImprovedTextures.Patches;

[HarmonyPatch(typeof(UniqueIdentifier))]
public static class UniqueIdentifierAwakePatch
{
    [HarmonyPatch(nameof(UniqueIdentifier.Awake))]
    [HarmonyPostfix]
    public static void PrefabIdentifierAwakePostfix(UniqueIdentifier __instance)
    {
        var database = TextureDatabase.GetInstance();
        foreach (var renderer in __instance.gameObject.GetComponentsInChildren<Renderer>(true))
        {
            TextureUtils.ConvertRenderer(renderer, database);
        }
    }
}