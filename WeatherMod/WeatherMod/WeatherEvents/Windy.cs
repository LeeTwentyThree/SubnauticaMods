using UnityEngine;
using WeatherMod.Mono;

namespace WeatherMod.WeatherEvents;

public class Windy : WeatherEvent
{
    protected override GameObject EffectPrefab { get; } = null;
    protected override float DestroyDelay { get; } = 0;
    protected override FogSettings Fog { get; } = new FogSettings(0.0007f, new Color(0.18f, 0.26f, 0.45f), 0.8f, 0.6f, 1f);
    public override float MinDuration { get; } = 60;
    public override float MaxDuration { get; } = 90;
    public override WeatherEventAudio AmbientSound { get; } = new WeatherEventAudio(WeatherAudio.WindyLoop, WeatherAudio.WindyLoopInside, null);

    protected override void OnEventBegin(GameObject effectPrefab) { }

    protected override void OnEventEnd(GameObject effectPrefab) { }
}