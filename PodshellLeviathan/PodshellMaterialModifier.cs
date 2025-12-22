using Nautilus.Utility;
using Nautilus.Utility.MaterialModifiers;
using UnityEngine;

namespace PodshellLeviathan;

internal class PodshellMaterialModifier : MaterialModifier
{
    private static readonly int SpecColor = Shader.PropertyToID("_SpecColor");
    private static readonly int Shininess = Shader.PropertyToID("_Shininess");
    private static readonly int Fresnel = Shader.PropertyToID("_Fresnel");
    private static readonly int GlowStrength = Shader.PropertyToID("_GlowStrength");
    private static readonly int GlowStrengthNight = Shader.PropertyToID("_GlowStrengthNight");
    private static readonly int SpecInt = Shader.PropertyToID("_SpecInt");
    private static readonly int MyCullVariable = Shader.PropertyToID("_MyCullVariable");

    public override void EditMaterial(Material material, Renderer renderer, int materialIndex, MaterialUtils.MaterialType materialType)
    {
        /*
        if (renderer.gameObject.name == "head_low" && materialIndex == 1)
        {
            material.SetColor(ShaderPropertyID._Color, new Color(1, 1, 1, 0.8f));
            material.SetColor(SpecColor, new Color(2, 2, 2));
            material.SetFloat(SpecInt, 50);
            material.SetFloat(Shininess, 5.1f);
            material.SetFloat(Fresnel, 0.7f);
            return;
        }
        */

        material.SetColor(ShaderPropertyID._Color, new Color(1, 1, 1));
        material.SetColor(SpecColor, new Color(2, 2, 2));
        material.SetFloat(Shininess, 6);
        material.SetFloat(SpecInt, 5);
        material.SetFloat(Fresnel, 0.24f);
        material.SetFloat(GlowStrength, 0.5f);
        material.SetFloat(GlowStrengthNight, 0.5f);
        material.SetFloat("_EmissionLM", 0.05f);
        material.SetFloat("_EmissionLMNight", 0.05f);
        
        if (renderer.gameObject.name == "head_low")
        {
            if (materialIndex == 1)
            {
                material.SetFloat(SpecInt, 2);
                material.SetFloat(Shininess, 8);
                material.SetFloat(Fresnel, 0f);
            }
            else
            {
                material.SetFloat(MyCullVariable, 0);
            }
        }

        if (renderer.gameObject.name.StartsWith("tailarmor"))
        {
            material.SetFloat(MyCullVariable, 0);
        }
        
        material.SetFloat("_InfectionHeightStrength", 0);
        material.SetVector("_InfectionScale", new Vector4(2, 2, 2, 2));
        material.SetVector("_InfectionOffset", new Vector4(0.2f, 0.2f, 0.2f, 0.2f));
    }
}