using Nautilus.Utility;
using Nautilus.Utility.MaterialModifiers;
using UnityEngine;

namespace DeExtinction.MaterialModifiers;

public class FresnelModifier : MaterialModifier
{
    private readonly float _fresnel;
    
    private static readonly int Fresnel = Shader.PropertyToID("_Fresnel");

    public FresnelModifier(float fresnel)
    {
        _fresnel = fresnel;
    }

    public override void EditMaterial(Material material, Renderer renderer, int materialIndex, MaterialUtils.MaterialType materialType)
    {
        material.SetFloat(Fresnel, _fresnel);
    }
}