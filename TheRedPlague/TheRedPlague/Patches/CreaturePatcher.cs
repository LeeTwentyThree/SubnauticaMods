using System.Linq;
using HarmonyLib;
using UnityEngine;

namespace TheRedPlague.Patches;

[HarmonyPatch(typeof(Creature))]
public static class CreaturePatcher
{
    [HarmonyPatch(nameof(Creature.OnEnable))]
    [HarmonyPostfix]
    public static void OnEnablePostfix(Creature __instance)
    {
        var infectedMixin = __instance.gameObject.GetComponent<InfectedMixin>();
        if (infectedMixin != null)
            return;
        infectedMixin = __instance.gameObject.AddComponent<InfectedMixin>();
        infectedMixin.renderers = __instance.GetComponentsInChildren<Renderer>(true);
    }
}