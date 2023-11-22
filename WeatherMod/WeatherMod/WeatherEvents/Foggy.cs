using UnityEngine;
using WeatherMod.Mono;

namespace WeatherMod.WeatherEvents;

public class Foggy : WeatherEvent
{
    protected override GameObject EffectPrefab { get; } = null;
    protected override float DestroyDelay { get; } = 0;
    protected override FogSettings Fog { get; } = new FogSettings(0.03f, new Color(0.2f, 0.2f, 0.2f), 0.2f);
    public override float MinDuration { get; } = 60;
    public override float MaxDuration { get; } = 60;
    
    public override float AboveWaterSunlightScale { get; } = 1f;
    
    public override float BelowWaterSunlightScale { get; } = 1f;

    protected override void OnEventBegin(GameObject effectPrefab) { }

    protected override void OnEventEnd(GameObject effectPrefab) { }
}