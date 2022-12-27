using DebugHelper.Systems;
using HarmonyLib;
using UnityEngine;

namespace DebugHelper.Patches
{
    [HarmonyPatch(typeof(LiveMixin))]
    internal static class LiveMixinPatches
    {
        [HarmonyPatch(nameof(LiveMixin.TakeDamage))]
        [HarmonyPostfix()]
        public static void LiveMixinTakeDamagePostfix(LiveMixin __instance, float originalDamage, DamageType type, Vector3 position)
        {
            if (Main.config.ShowDamageInfo) RenderedDamageInfo.Create(__instance, originalDamage, type, position);
        }
    }
}