using UnityEngine;
using WeatherMod.Mono;

namespace WeatherMod.WeatherEvents;

public class GoldenThunderstorm : WeatherEvent
{
    protected override GameObject EffectPrefab { get; } = Plugin.AssetBundle.LoadAsset<GameObject>("Weather_Freddy");
    protected override float DestroyDelay { get; } = 18;
    protected override FogSettings Fog { get; } = new FogSettings(0.002f, new Color(0.4f, 0.4f, 0.1f), 0.2f, 1f, 0.5f);
    public override float MinDuration { get; } = 105.87f;
    public override float MaxDuration { get; } = 105.87f;
    public override WeatherEventAudio AmbientSound { get; } = new WeatherEventAudio(WeatherAudio.GoldenThunderstormLoop, WeatherAudio.GoldenThunderstormLoop, WeatherAudio.GoldenThunderstormLoop);

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
        effectPrefab.AddComponent<LightningSpawner>().useAltModel = true;
        effectPrefab.AddComponent<LightningSpawner>().useAltModel = true;
        effectPrefab.AddComponent<DisableWhenCameraUnderwater>();
    }

    protected override void OnEventEnd(GameObject effectPrefab)
    {
        foreach (var lightning in effectPrefab.GetComponents<LightningSpawner>())
        {
            Object.Destroy(lightning);
        }
    }
}