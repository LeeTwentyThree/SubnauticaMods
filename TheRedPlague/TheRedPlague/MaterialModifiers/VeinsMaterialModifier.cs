using Nautilus.Utility;
using Nautilus.Utility.MaterialModifiers;
using UnityEngine;

namespace TheRedPlague.MaterialModifiers;

public class VeinsMaterialModifier : MaterialModifier
{
    private static readonly int SpecInt = Shader.PropertyToID("_SpecInt");
    private static readonly int Shininess = Shader.PropertyToID("_Shininess");
    private static readonly int Fresnel = Shader.PropertyToID("_Fresnel");

    public override void EditMaterial(Material material, Renderer renderer, int materialIndex, MaterialUtils.MaterialType materialType)
    {
        material.color *= 1.2f;
        material.SetFloat(SpecInt, 15);
        material.SetFloat(Shininess, 8);
        material.SetFloat(Fresnel, 0);
    }
}