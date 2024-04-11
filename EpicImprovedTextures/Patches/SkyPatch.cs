using HarmonyLib;
using UnityEngine;

namespace EpicImprovedTextures.Patches;

[HarmonyPatch(typeof(uSkyManager))]
public static class SkyPatch
{
    [HarmonyPatch(nameof(uSkyManager.Start))]
    [HarmonyPostfix]
    public static void PlayerStartPostfix(uSkyManager __instance)
    {
        var database = TextureDatabase.GetInstance();
        TextureUtils.ConvertMaterial(__instance.SkyboxMaterial, database);
        TextureUtils.ConvertMaterial(__instance.m_starMaterial, database);
        __instance.MoonTexture = Plugin.AssetBundle.LoadAsset<Texture2D>("moon");
    }
}