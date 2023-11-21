using UnityEngine;
using WeatherMod.Mono;

namespace WeatherMod.WeatherEvents;

public class LightRain : WeatherEvent
{
    protected override GameObject EffectPrefab { get; } = Plugin.AssetBundle.LoadAsset<GameObject>("Weather_Rain");
    protected override float DestroyDelay { get; } = 5;
    protected override FogSettings Fog { get; } = new FogSettings(0.002f, new Color(42 / 255f, 85 / 255f, 120 / 255f));
    public override float MinDuration { get; } = 60;
    public override float MaxDuration { get; } = 60;

    protected override void OnEventBegin(GameObject effectPrefab)
    {
        effectPrefab.AddComponent<AlignWithCamera>();
    }

    protected override void OnEventEnd(GameObject effectPrefab)
    {
        
    }
}