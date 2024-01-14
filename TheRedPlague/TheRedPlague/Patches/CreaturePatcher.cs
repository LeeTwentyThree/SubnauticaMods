using System.Linq;
using HarmonyLib;
using TheRedPlague.Mono;
using UnityEngine;

namespace TheRedPlague.Patches;

[HarmonyPatch(typeof(Creature))]
public static class CreaturePatcher
{
    [HarmonyPatch(nameof(Creature.OnEnable))]
    [HarmonyPostfix]
    public static void OnEnablePostfix(Creature __instance)
    {
        __instance.gameObject.EnsureComponent<InfectionStrikeTarget>();

        var infectedMixin = __instance.gameObject.GetComponent<InfectedMixin>();
        if (infectedMixin != null)
        {
            if (infectedMixin.renderers == null || infectedMixin.renderers.Length == 0)
            {
                infectedMixin.renderers = __instance.GetComponentsInChildren<Renderer>(true);
            }
            return;
        }
        infectedMixin = __instance.gameObject.AddComponent<InfectedMixin>();
        infectedMixin.renderers = __instance.GetComponentsInChildren<Renderer>(true);
    }
}