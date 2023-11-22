using UnityEngine;
using WeatherMod.Mono;

namespace WeatherMod.WeatherEvents;

public class LightRain : WeatherEvent
{
    protected override GameObject EffectPrefab { get; } = Plugin.AssetBundle.LoadAsset<GameObject>("Weather_LightRain");
    protected override float DestroyDelay { get; } = 5;
    protected override FogSettings Fog { get; } = new FogSettings(0.0014f, new Color(0.18f, 0.26f, 0.45f), 0.5f);
    public override float MinDuration { get; } = 60;
    public override float MaxDuration { get; } = 60;
    public override float AboveWaterSunlightScale { get; } = 1f;
    public override float BelowWaterSunlightScale { get; } = 0.8f;

    protected override void OnEventBegin(GameObject effectPrefab)
    {
        effectPrefab.AddComponent<AlignWithCamera>();
        foreach (var renderer in effectPrefab.GetComponentsInChildren<Renderer>())
        {
            WeatherMaterialUtils.ApplyRainMaterial(renderer);
        }

        effectPrefab.AddComponent<DisableWhenCameraUnderwater>();
    }

    protected override void OnEventEnd(GameObject effectPrefab)
    {
        
    }
}