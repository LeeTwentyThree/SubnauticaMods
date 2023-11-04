using Nautilus.Utility;
using Nautilus.Utility.MaterialModifiers;
using UnityEngine;

namespace TheRumbling.MaterialModifiers;

internal class TitanMaterialModifier : MaterialModifier
{
    public override void EditMaterial(Material material, Renderer renderer, int materialIndex, MaterialUtils.MaterialType materialType)
    {
        if (renderer is ParticleSystemRenderer) return;

        material.SetFloat("_SpecInt", 0.2f);
        material.SetFloat("_Shininess", 4.3f);
        material.SetFloat("_Fresnel", 0.69f);
        material.SetFloat("_GlowStrengthNight", 0.1f);
    }
}