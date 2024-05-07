using Nautilus.Utility;
using Nautilus.Utility.MaterialModifiers;
using UnityEngine;

namespace TheRedPlague.MaterialModifiers;

public class DomeMaterialModifier : MaterialModifier
{
    public override void EditMaterial(Material material, Renderer renderer, int materialIndex, MaterialUtils.MaterialType materialType)
    {
        if (materialIndex == 0 || materialIndex == 2)
        {
            material.EnableKeyword("MARMO_SPECMAP");
            material.SetColor("_Color", new Color(0, 0.142858f, 0.285714f));
            material.SetColor("_SpecColor", new Color(0, 0.976190f, 0.595238f));
            material.SetFloat("_SpecInt", 1f);
            material.SetFloat("_Shininess", 6f);
            material.SetFloat("_Fresnel", 0.57f);
        }
    }

    public override bool BlockShaderConversion(Material material, Renderer renderer, MaterialUtils.MaterialType materialType)
    {
        return material.name.Contains("Glass");
    }
}