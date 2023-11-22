using UnityEngine;
using WeatherMod.Mono;

namespace WeatherMod.WeatherEvents;

public class Thunderstorm : WeatherEvent
{
    protected override GameObject EffectPrefab { get; } = Plugin.AssetBundle.LoadAsset<GameObject>("Weather_Thunderstorm");
    protected override float DestroyDelay { get; } = 5;
    protected override FogSettings Fog { get; } = new FogSettings(0.0015f, new Color(0.1f, 0.1f, 0.1f), 0.2f);
    public override float MinDuration { get; } = 140;
    public override float MaxDuration { get; } = 160;
    public override float AboveWaterSunlightScale { get; } = 1f;
    public override float BelowWaterSunlightScale { get; } = 0.2f;

    protected override void OnEventBegin(GameObject effectPrefab)
    {
        foreach (var renderer in effectPrefab.GetComponentsInChildren<Renderer>())
        {
            WeatherMaterialUtils.ApplyRainMaterial(renderer);
        }

        var smokeClouds = CloudUtils.GetStormCloudEffect();
        
        smokeClouds.transform.parent = effectPrefab.transform;
        smokeClouds.transform.localPosition = Vector3.up * 300;
        
        smokeClouds.gameObject.SetActive(true);
        
        effectPrefab.AddComponent<AlignWithCamera>();
        effectPrefab.AddComponent<LightningSpawner>();
        effectPrefab.AddComponent<LightningSpawner>();
        effectPrefab.AddComponent<DisableWhenCameraUnderwater>();
    }

    protected override void OnEventEnd(GameObject effectPrefab)
    {
        
    }
}