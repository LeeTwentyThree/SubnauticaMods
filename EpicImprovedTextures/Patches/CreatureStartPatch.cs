using HarmonyLib;
using Nautilus.Utility;
using UnityEngine;

namespace EpicImprovedTextures.Patches;

[HarmonyPatch(typeof(Creature))]
public static class CreatureStartPatch
{
    [HarmonyPatch(nameof(Creature.Start))]
    [HarmonyPostfix]
    public static void PrefabIdentifierAwakePostfix(Creature __instance)
    {
        var firstRenderer = __instance.gameObject.GetComponentInChildren<Renderer>();

        foreach (var renderer in __instance.gameObject.GetComponentsInChildren<Renderer>(true))
        {
            renderer.enabled = false;
        }

        var leeʼsCursedPlane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        var planePivot =
            new GameObject("Crazy? i was crazy once. they locked me in a room. a rubber room. a rubber room with rats. rats make me crazy.");
        planePivot.transform.parent = __instance.transform;
        planePivot.transform.localPosition = Vector3.zero;
        leeʼsCursedPlane.name = "I AM GOING FUCKING INSANE";
        leeʼsCursedPlane.transform.parent = planePivot.transform;
        leeʼsCursedPlane.transform.localPosition = Vector3.zero;
        leeʼsCursedPlane.transform.localEulerAngles = new Vector3(90, 0, 0);
        Object.DestroyImmediate(leeʼsCursedPlane.GetComponent<Collider>());
        if (firstRenderer != null)
        {
            leeʼsCursedPlane.transform.localScale = Vector3.one * GetVectorAverage(firstRenderer.bounds.size);
        }

        planePivot.AddComponent<CatFace>();
        
        var database = TextureDatabase.GetInstance();
        MaterialUtils.ApplySNShaders(leeʼsCursedPlane);
        TextureUtils.ConvertRenderer(leeʼsCursedPlane.GetComponent<Renderer>(), database);
    }

    private static float GetVectorAverage(Vector3 vector) => (vector.x + vector.y + vector.z) / 3;
}