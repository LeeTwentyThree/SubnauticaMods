using System;
using Nautilus.Utility;
using UnityEngine;
using Random = UnityEngine.Random;

namespace WeatherMod.Mono;

public class LightningSpawner : MonoBehaviour
{
    private const float MinInterval = 1f;
    private const float MaxInterval = 6f;
    private const float MinDistanceFromCamera = 20f;
    private const float MaxDistanceFromCamera = 400f;
    private const float SpawnHeightMin = 140;
    private const float SpawnHeightVariationMax = 180;
    private const float SpawnHeightAbsMax = 240;

    private float _timeSpawnLightningAgain;

    public bool useAltModel;

    private void ResetTimer() => _timeSpawnLightningAgain = Time.time + Random.Range(MinInterval, MaxInterval);

    private void Awake()
    {
        ResetTimer();
        _timeSpawnLightningAgain += 8; // delay initial lightning
    }

    private void Update()
    {
        if (Time.time < _timeSpawnLightningAgain) return;

        var spawnAngle = Random.value * Mathf.PI * 2f;
        var dist = Random.Range(MinDistanceFromCamera, MaxDistanceFromCamera);

        SpawnLightning(
            MainCamera.camera.transform.position + new Vector3(Mathf.Cos(spawnAngle), 0, Mathf.Sin(spawnAngle)) * dist,
            useAltModel);

        ResetTimer();
    }

    public static void SpawnLightning(Vector3 position, bool alwaysUseAltModel)
    {
        var camPos = MainCamera.camera.transform.position;

        var lightningSpawnPosition = new Vector3(position.x,
            Mathf.Clamp(camPos.y + Random.Range(SpawnHeightMin, SpawnHeightVariationMax),
                Ocean.GetOceanLevel() + SpawnHeightMin, Ocean.GetOceanLevel() + SpawnHeightAbsMax), position.z);

        var altModel = alwaysUseAltModel;
        if (Random.value <= 0.01f) altModel = true;

        var lightning =
            Instantiate(Plugin.AssetBundle.LoadAsset<GameObject>(altModel ? "VFX_FreddyLightning" : "VFX_Lightning"));

        lightning.transform.position = lightningSpawnPosition;

        foreach (var r in lightning.GetComponentsInChildren<Renderer>())
        {
            var m = r.material;
            m.shader = MaterialUtils.Shaders.ParticlesUBER;
        }

        /* Utils.PlayFMODAsset(Vector3.Distance(MainCamera.camera.transform.position, position) < 80
            ? WeatherAudio.ThunderSoundsNear.GetRandomUnity().Asset
            : WeatherAudio.ThunderSoundsFar.GetRandomUnity().Asset,
            position);
            */

        Destroy(lightning, 5f);
    }
}