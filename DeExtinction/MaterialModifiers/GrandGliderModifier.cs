using Nautilus.Utility;
using Nautilus.Utility.MaterialModifiers;
using UnityEngine;

namespace DeExtinction.MaterialModifiers;

public class GrandGliderModifier : MaterialModifier
{
    private static readonly int Shininess = Shader.PropertyToID("_Shininess");
    private static readonly int SpecInt = Shader.PropertyToID("_SpecInt");

    public override void EditMaterial(Material material, Renderer renderer, int materialIndex, MaterialUtils.MaterialType materialType)
    {
        switch (materialIndex)
        {
            case 0:
                material.color = new Color(1.5f, 1.5f, 1.5f);
                material.SetFloat(SpecInt, 3);
                material.SetFloat(Shininess, 4);
                break;
            case 1:
                material.SetFloat(SpecInt, 1);
                material.SetFloat(Shininess, 7);
                break;
            case 2:
                material.color = new Color(1, 1, 1, 0.9f);
                material.SetFloat(SpecInt, 1);
                material.SetFloat(Shininess, 7.5f);
                break;
        }
    }
}