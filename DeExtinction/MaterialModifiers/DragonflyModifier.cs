using Nautilus.Utility;
using Nautilus.Utility.MaterialModifiers;
using UnityEngine;

namespace DeExtinction.MaterialModifiers;

public class DragonflyModifier : MaterialModifier
{
    private static readonly int Shininess = Shader.PropertyToID("_Shininess");
    private static readonly int SpecInt = Shader.PropertyToID("_SpecInt");

    public override void EditMaterial(Material material, Renderer renderer, int materialIndex, MaterialUtils.MaterialType materialType)
    {
        if (material.name.Contains("Eye"))
        {
            material.SetFloat(SpecInt, 10);
            material.SetFloat(Shininess, 6);
        }
    }
}