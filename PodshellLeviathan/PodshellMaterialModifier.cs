using Nautilus.Utility;
using Nautilus.Utility.MaterialModifiers;
using UnityEngine;

namespace PodshellLeviathan;

internal class PodshellMaterialModifier : MaterialModifier
{
    public override void EditMaterial(Material material, Renderer renderer, int materialIndex, MaterialUtils.MaterialType materialType)
    {
        if (renderer.gameObject.name == "head_low" && materialIndex == 1)
        {
            material.SetColor(ShaderPropertyID._Color, new Color(1, 1, 1, 0.9f));
            material.SetColor("_SpecColor", Color.white);
            material.SetFloat("_SpecInt", 50);
            material.SetFloat("_Shininess", 0.6f);
            material.SetFloat("_Fresnel", 0.7f);
            return;
        }

        material.SetColor(ShaderPropertyID._Color, new Color(2, 2, 2));
        material.SetColor("_SpecColor", new Color(3, 3, 3));
        material.SetFloat("_Shininess", 6);
        material.SetFloat("_Fresnel", 0.6f);
    }
}