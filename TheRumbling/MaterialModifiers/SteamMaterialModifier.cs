using Nautilus.Utility;
using Nautilus.Utility.MaterialModifiers;
using UnityEngine;

namespace TheRumbling.MaterialModifiers;

internal class SteamMaterialModifier : MaterialModifier
{
    public override void EditMaterial(Material material, Renderer renderer, int materialIndex, MaterialUtils.MaterialType materialType)
    {
        if (renderer is ParticleSystemRenderer)
        {
            material.shader = MaterialUtils.Shaders.ParticlesUBER;
        }
    }

    public override bool BlockShaderConversion(Material material, Renderer renderer, MaterialUtils.MaterialType materialType)
    {
        return renderer is ParticleSystemRenderer;
    }
}
