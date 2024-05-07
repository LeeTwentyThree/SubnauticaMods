using Nautilus.Utility;
using Nautilus.Utility.MaterialModifiers;
using UnityEngine;

namespace TheRedPlague.MaterialModifiers;

public class AirStrikeDeviceModifier : MaterialModifier
{
    public override void EditMaterial(Material material, Renderer renderer, int materialIndex, MaterialUtils.MaterialType materialType)
    {
        // main
        if (materialIndex == 0)
        {
            material.SetColor("_SpecColor", new Color(0.5f, 0.5f, 0.5f));
            material.SetFloat("_GlowStrength", 5);
            material.SetFloat("_GlowStrengthNight", 3);
        }
        // screen
        else
        {
            material.SetFloat("_Shininess", 7.8f);
            material.SetFloat("_SpecInt", 0.1f);
        }
    }
}