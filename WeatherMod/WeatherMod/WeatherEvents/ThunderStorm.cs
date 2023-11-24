using UnityEngine;
using WeatherMod.Mono;

namespace WeatherMod.WeatherEvents;

public class Thunderstorm : WeatherEvent
{
    protected override GameObject EffectPrefab { get; } = Plugin.AssetBundle.LoadAsset<GameObject>("Weather_Thunderstorm");
    protected override float DestroyDelay { get; } = 18;
    protected override FogSettings Fog { get; } = new FogSettings(0.002f, new Color(0.1f, 0.1f, 0.1f), 0.2f, 1f, 0.4f);
    public override float MinDuration { get; } = 120;
    public override float MaxDuration { get; } = 160;
    public override WeatherEventAudio AmbientSound { get; } = new WeatherEventAudio(WeatherAudio.ThunderstormLoop, WeatherAudio.ThunderstormLoopInside, null);

    protected override void OnEventBegin(GameObject effectPrefab)
    {
        foreach (var renderer in effectPrefab.GetComponentsInChildren<Renderer>())
        {
            WeatherMaterialUtils.ApplyRainMaterial(renderer);
        }

        var smokeClouds = CloudUtils.GetStormCloudEffect();
        
        smokeClouds.transform.parent = effectPrefab.transform;
        smokeClouds.transform.localPosition = Vector3.up * 400;
        
        smokeClouds.gameObject.SetActive(true);
        
        effectPrefab.AddComponent<AlignWithCamera>();
        effectPrefab.AddComponent<LightningSpawner>();
        effectPrefab.AddComponent<LightningSpawner>();
        effectPrefab.AddComponent<DisableWhenCameraUnderwater>();
        
        effectPrefab.AddComponent<WaterSplashVfxController>().affectedTransform =
            effectPrefab.transform.Find("WaterSplashes");
    }

    protected override void OnEventEnd(GameObject effectPrefab)
    {
        foreach (var lightning in effectPrefab.GetComponents<LightningSpawner>())
        {
            Object.Destroy(lightning);
        }
    }
}