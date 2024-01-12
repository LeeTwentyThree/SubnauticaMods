using UnityEngine;

namespace WeatherMod;

public static class CloudUtils
{
    private static GameObject _smokeVfxOriginal;
    private static GameObject _stormCloudPrefab;

    public static GameObject GetStormCloudEffect()
    {
        if (_stormCloudPrefab != null)
            return _stormCloudPrefab;
        
        EnsureOriginalCloudFX();

        _stormCloudPrefab = Object.Instantiate(_smokeVfxOriginal);
        
        _stormCloudPrefab.SetActive(false);
        
        var ps = _stormCloudPrefab.GetComponent<ParticleSystem>();
        var main = ps.main;
        main.startLifetimeMultiplier = 18;
        main.startSpeedMultiplier = 0;
        main.startSizeMultiplier = 500;
        main.simulationSpace = ParticleSystemSimulationSpace.World;
        main.maxParticles = 750;
        main.prewarm = false;
        var velocity = ps.velocityOverLifetime;
        velocity.xMultiplier = 15;
        velocity.yMultiplier = 0;
        var emission = ps.emission;
        emission.rateOverTimeMultiplier = 200;
        var shape = ps.shape;
        shape.shapeType = ParticleSystemShapeType.Circle;
        shape.radius = 1600;
        shape.rotation = Vector3.left * 90;
        var anim = ps.textureSheetAnimation;
        anim.frameOverTimeMultiplier = 1f;

        var renderer = _stormCloudPrefab.GetComponent<ParticleSystemRenderer>();
        renderer.sortingFudge = 0;
        
        _stormCloudPrefab.transform.localEulerAngles = Vector3.zero;

        var r = _stormCloudPrefab.GetComponent<Renderer>();
        r.material.SetColor(ShaderPropertyID._Color, new Color(1, 1, 1, 0.5f));
        
        return _stormCloudPrefab;
    }

    private static void EnsureOriginalCloudFX()
    {
        if (_smokeVfxOriginal != null) return;
        
        if (CrashedShipExploder.main == null) Plugin.Logger.LogError("Weather mod error: AURORA NOT FOUND! Where did it go lol?");
        
        // "Failed cloud T_T X_X smh lol"
        _smokeVfxOriginal = CrashedShipExploder.main.transform
            .Find("unexplodedFX/Ship_Exterior_CrashedFX(Clone)/xSmkColumn2").gameObject;
    }
}