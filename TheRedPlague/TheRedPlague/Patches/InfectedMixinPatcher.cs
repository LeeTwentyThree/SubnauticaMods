using HarmonyLib;
using Newtonsoft.Json;
using UnityEngine;

namespace TheRedPlague.Patches;

[HarmonyPatch(typeof(InfectedMixin))]
public static class InfectedMixinPatcher
{
    private static readonly int InfectionHeightStrength = Shader.PropertyToID("_InfectionHeightStrength");
    private static readonly int SpecInt = Shader.PropertyToID("_SpecInt");
    private static readonly int SpecColor = Shader.PropertyToID("_SpecColor");
    private const int MinInfectionValue = 1;

    [HarmonyPatch(nameof(InfectedMixin.UpdateInfectionShading))]
    [HarmonyPostfix]
    public static void StartPostfix(InfectedMixin __instance)
    {
        if (__instance.materials == null)
            return;
        
        var techType = CraftData.GetTechType(__instance.gameObject);
        var hasOverridenInfectionSettings = InfectionSettingsDatabase.InfectionSettingsList.TryGetValue(techType, out var infectionSettings);

        foreach (var material in __instance.materials)
        {
            material.SetTexture(ShaderPropertyID._InfectionAlbedomap, Plugin.ZombieInfectionTexture);
            material.SetFloat(InfectionHeightStrength, hasOverridenInfectionSettings ? infectionSettings.InfectionHeight : -0.1f);
            if (hasOverridenInfectionSettings) material.SetVector("_InfectionScale", infectionSettings.InfectionScale);
            if (hasOverridenInfectionSettings) material.SetVector("_InfectionOffset", infectionSettings.InfectionOffset);
            if (hasOverridenInfectionSettings && __instance.infectedAmount >= MinInfectionValue)
                material.SetColor(ShaderPropertyID._Color, infectionSettings.InfectedBodyColor);

            if (__instance.infectedAmount < MinInfectionValue)
                continue;
            
            if (techType is TechType.GhostLeviathan or TechType.GhostLeviathanJuvenile)
            {
                material.SetColor(SpecColor, Color.black);
                material.SetColor(ShaderPropertyID._GlowColor, Color.black);
            }
            
            if (techType == TechType.Crash && material.name.Contains("Eye"))
            {
                material.SetColor(ShaderPropertyID._Color, Color.black);
                material.SetColor(SpecColor, Color.black);
                material.SetColor(ShaderPropertyID._GlowColor, Color.black);
            }
            
            if (techType == TechType.Mesmer)
            {
                material.SetColor(ShaderPropertyID._Color, Color.black);
                material.SetColor(SpecColor, new Color(1, 0.571429f, 1));
                material.SetColor(ShaderPropertyID._GlowColor, new Color(3, 0, 0));
            }
            
            if (techType == TechType.SpineEel && material.name.Contains("eye"))
            {
                material.SetColor(ShaderPropertyID._Color, new Color(5, 5, 5));
                material.SetColor(SpecColor, new Color(0, 5, 5));
            }
        }
        
        if (techType == TechType.SpineEel && __instance.infectedAmount >= MinInfectionValue)
            __instance.transform.Find("model/spine_eel_geo/Spine_eel_geo/Spine_eel_eye_L").gameObject.SetActive(false);
    }
}