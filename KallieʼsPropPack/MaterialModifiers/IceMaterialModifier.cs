using Nautilus.Utility;
using Nautilus.Utility.MaterialModifiers;
using UnityEngine;

namespace KallieʼsPropPack.MaterialModifiers;

public class IceMaterialModifier : MaterialModifier
{
    private static readonly int Fresnel = Shader.PropertyToID("_Fresnel");
    private static readonly int IbLreductionAtNight = Shader.PropertyToID("_IBLreductionAtNight");
    private static readonly int GlowStrengthNight = Shader.PropertyToID("_GlowStrengthNight");
    private static readonly int GlowStrength = Shader.PropertyToID("_GlowStrength");
    private static readonly int MyCullVariable = Shader.PropertyToID("_MyCullVariable");
    private static readonly int Shininess = Shader.PropertyToID("_Shininess");

    private bool _dark;

    public IceMaterialModifier(bool dark)
    {
        _dark = dark;
    }
    
    public override void EditMaterial(Material material, Renderer renderer, int materialIndex, MaterialUtils.MaterialType materialType)
    {
        material.SetFloat(Fresnel, 0.63f);
        material.SetFloat(IbLreductionAtNight, 0.8f);
        material.SetFloat(GlowStrength, 0.6f);
        material.SetFloat(GlowStrengthNight, 0.15f);
        
        if (_dark)
        {
            var color = material.color;
            material.color = new Color(color.r / 2, color.g / 2, color.b / 2, color.a);
        }
        
        if (materialType == MaterialUtils.MaterialType.Transparent)
        {
            material.color = material.color.WithAlpha(0.6f);
            material.SetFloat(MyCullVariable, 0);
            material.SetFloat(Shininess, 6);
        }
    }
}