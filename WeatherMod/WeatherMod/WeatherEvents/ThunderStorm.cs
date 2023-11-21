using UnityEngine;
using WeatherMod.Mono;

namespace WeatherMod.WeatherEvents;

public class ThunderStorm : WeatherEvent
{
    protected override GameObject EffectPrefab { get; } = Plugin.AssetBundle.LoadAsset<GameObject>("Weather_Rain");
    protected override float DestroyDelay { get; } = 5;
    protected override FogSettings Fog { get; } = new FogSettings(0.003f, new Color(0f, 0.01f, 0.03f), 0.1f);
    public override float MinDuration { get; } = 140;
    public override float MaxDuration { get; } = 160;

    protected override void OnEventBegin(GameObject effectPrefab)
    {
        effectPrefab.AddComponent<AlignWithCamera>();
    }

    protected override void OnEventEnd(GameObject effectPrefab)
    {
        
    }
}