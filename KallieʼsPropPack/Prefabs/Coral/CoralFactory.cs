using System.Collections;
using KallieʼsPropPack.PrefabLoading;
using Nautilus.Utility;
using Nautilus.Utility.MaterialModifiers;
using UnityEngine;

namespace KallieʼsPropPack.Prefabs.Coral;

public class CoralFactory : IEpicPrefabFactory
{
    private static readonly int ObjectUp = Shader.PropertyToID("_ObjectUp");
    private static readonly int Speed = Shader.PropertyToID("_Speed");
    private static readonly int Falloff = Shader.PropertyToID("_Fallof");
    private static readonly int GlowStrength = Shader.PropertyToID("_GlowStrength");
    private static readonly int GlowStrengthNight = Shader.PropertyToID("_GlowStrengthNight");

    public IEnumerator BuildVariant(GameObject prefab, LoadedPrefabRegistrationData.Parameter[] parameters)
    {
        yield break;
    }

    public MaterialModifier[] MaterialModifiers { get; } =
    {
        new CoralMaterialModifier(),
        new DoubleSidedModifier(MaterialUtils.MaterialType.Cutout)
    };

    private class CoralMaterialModifier : MaterialModifier
    {
        public override void EditMaterial(Material material, Renderer renderer, int materialIndex,
            MaterialUtils.MaterialType materialType)
        {
            var name = renderer.gameObject.name;
            var isGrass = name.StartsWith("Coral_3");
            var isSmallCoral = name.StartsWith("Coral_13") || name.StartsWith("Glowthorn_Group1");
            var isSeaweed = name.StartsWith("Seaweed");

            if (isGrass || isSmallCoral || isSeaweed)
            {
                material.EnableKeyword("UWE_WAVING");
                material.SetVector(ObjectUp, Vector3.up);
            }

            if (isSmallCoral)
            {
                material.SetVector(Speed, new Vector2(0.18f, 0.1f));
                material.SetFloat(Falloff, 0.8f);
            }
            else if (isGrass)
            {
                material.SetVector(Speed, new Vector2(0.2f, 0.1f));
            }
            else if (isSeaweed)
            {
                material.SetVector(ObjectUp, Vector3.up);
                material.SetFloat(Falloff, 0.7f);
            }

            if (name.StartsWith("Coral7_Group"))
            {
                material.SetFloat(GlowStrength, 0.3f);
                material.SetFloat(GlowStrengthNight, 0.3f);
            }
            
            if (name.StartsWith("Coral1_"))
            {
                material.SetFloat(GlowStrength, 0.05f);
                material.SetFloat(GlowStrengthNight, 0.05f);
            }
        }
    }
}