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
        
        SpawnLightning(MainCamera.camera.transform.position + new Vector3(Mathf.Cos(spawnAngle), 0, Mathf.Sin(spawnAngle)) * dist);
        
        ResetTimer();
    }

    public static void SpawnLightning(Vector3 position)
    {
        var camPos = MainCamera.camera.transform.position;

        var lightningSpawnPosition = new Vector3(position.x, Mathf.Clamp(camPos.y + Random.Range(SpawnHeightMin, SpawnHeightVariationMax), Ocean.GetOceanLevel() + SpawnHeightMin, Ocean.GetOceanLevel() + SpawnHeightAbsMax), position.z);

        var lightning = Instantiate(Plugin.AssetBundle.LoadAsset<GameObject>("VFX_Lightning"));

        lightning.transform.position = lightningSpawnPosition;

        foreach (var r in lightning.GetComponentsInChildren<Renderer>())
        {
            var m = r.material;
            m.shader = MaterialUtils.Shaders.ParticlesUBER;
        }
        
        Destroy(lightning, 5f);
    }
}