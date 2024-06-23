using System;
using UnityEngine;
using UnityEngine.EventSystems;
using Object = UnityEngine.Object;

namespace ModStructureHelperPlugin;

public static class ObjectStripper
{
    private static readonly Type[] WhitelistedTypes = new[]
    {
        typeof(Transform),
        typeof(Renderer),
        typeof(MeshRenderer),
        typeof(MeshFilter),
        typeof(ParticleSystem),
        typeof(ParticleSystemForceField),
        typeof(UIBehaviour),
        typeof(Canvas),
        typeof(CanvasRenderer),
        typeof(SkyApplier)
    };
    
    public static void StripAllChildren(GameObject obj)
    {
        foreach (var component in obj.GetComponentsInChildren<Component>(true))
        {
            var whitelisted = false;
            var type = component.GetType();
            foreach (var whitelistedType in WhitelistedTypes)
            {
                if (whitelistedType.IsAssignableFrom(type)) whitelisted = true;
            }

            if (whitelisted) continue;
            switch (component)
            {
                case Rigidbody rb:
                    rb.isKinematic = true;
                    break;
                case Behaviour behaviour:
                    behaviour.enabled = false;
                    break;
            }
            Object.Destroy(component);
        }
    }
}