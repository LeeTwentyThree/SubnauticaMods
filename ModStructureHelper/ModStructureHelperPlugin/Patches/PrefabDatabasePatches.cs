using UnityEngine;
using HarmonyLib;
using UWE;

namespace ModStructureHelperPlugin.Patches;

[HarmonyPatch(typeof(PrefabDatabase))]
public static class PrefabDatabasePatches
{
    [HarmonyPostfix]
    [HarmonyPatch(nameof(PrefabDatabase.LoadPrefabDatabase))]
    public static void LoadPostfix()
    {
        Object.Instantiate(Plugin.AssetBundle.LoadAsset<GameObject>("EntityDatabase"));
    }
}