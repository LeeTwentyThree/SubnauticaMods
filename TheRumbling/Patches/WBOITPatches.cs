using HarmonyLib;
using UnityEngine;

namespace TheRumbling.Patches;

[HarmonyPatch(typeof(WBOIT))]
internal class WBOITPatches
{
    [HarmonyPatch(nameof(WBOIT.UpdateMaterialShaderParameters))]
    [HarmonyPrefix]
    public static bool Prefix(WBOIT __instance)
    {
        var mainCamera = MainCamera.camera;
        if (mainCamera == null)
        {
            return true;
        }

        var w = __instance; // shorthand

        var cameraPos = mainCamera.transform.position;

        var closestTitan = RumblingManager.GetNearestTitan(cameraPos);
        if (closestTitan == null)
        {
            return true;
        }
        float distanceToTitan = Vector3.Distance(cameraPos, closestTitan.heatEmitter.position);

        if (distanceToTitan > Balance.DistortionEffectMaxDistance)
        {
            return true;
        }

        if (Time.time > __instance.nextTemperatureUpdate)
        {
            w.temperatureScalar = Balance.HeatDistortionEffectStrength * (1 - distanceToTitan / Balance.DistortionEffectMaxDistance);
            w.nextTemperatureUpdate = Time.time + Random.value;
        }
        if (w.temperatureScalar > 0f && w.temperatureRefractTex != null)
        {
            if (!w.temperatureRefractEnabled)
            {
                w.compositeMaterial.EnableKeyword("FX_TEMPERATURE_REFRACT");
                w.temperatureRefractEnabled = true;
            }
            w.compositeMaterial.SetTexture(w.temperatureTexPropertyID, w.temperatureRefractTex);
            w.compositeMaterial.SetFloat(w.temperaturePropertyID, w.temperatureScalar);
            return false;
        }
        if (w.temperatureRefractEnabled)
        {
            w.compositeMaterial.DisableKeyword("FX_TEMPERATURE_REFRACT");
            w.temperatureRefractEnabled = false;
        }
        return false;
    }
}