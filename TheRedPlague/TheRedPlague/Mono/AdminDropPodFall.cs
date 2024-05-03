using System.Collections;
using Nautilus.Utility;
using TheRedPlague.PrefabFiles;
using UnityEngine;
using UWE;

namespace TheRedPlague.Mono;

public class AdminDropPodFall : MonoBehaviour
{
    private static readonly Vector3 _spawnPos = new Vector3(180, 4000, 2316);
    private static readonly Vector3 _finalPos = new Vector3(-175.090f, -667.283f, 3285.797f);
    private static readonly float _silenceGrabDepth = -20;
    private static readonly Vector3 _finalEulerAngles = new Vector3(8, 34.3f, 0.36f);

    private AnimationState _state;

    private float _yVelocity = -500;
    private float _airDrag = 0.002f;
    private float _underwaterDrag = 0.05f;
    private float _grabDuration = 30f;
    private float _floatDuration = 4.6f;

    private float _floatStartTime;
    
    private float _grabStartTime;
    private Vector2 _grabPathCenter;
    private float _grabPathRadius;
    private float _grabStartAngle;

    public static void SpawnAdministratorDropPod()
    {
        UWE.CoroutineHost.StartCoroutine(LoadAndSpawnAdminDropPod());
        SpawnFallingPod();
    }

    private static IEnumerator LoadAndSpawnAdminDropPod()
    {
        var task = CraftData.GetPrefabForTechTypeAsync(AdministratorDropPod.Info.TechType);
        yield return task;
        var prefab = task.GetResult();
        var stationaryDropPod = Instantiate(prefab, _finalPos, Quaternion.Euler(_finalEulerAngles));
        stationaryDropPod.SetActive(true);
        LargeWorldStreamer.main.cellManager.RegisterGlobalEntity(stationaryDropPod);
    }

    private static void SpawnFallingPod()
    {
        var fallingPod = Instantiate(Plugin.AssetBundle.LoadAsset<GameObject>("FallingAdminPod"));
        fallingPod.SetActive(false);
        fallingPod.transform.position = _spawnPos;
        fallingPod.AddComponent<SkyApplier>().renderers = fallingPod.GetComponentsInChildren<Renderer>(true);
        MaterialUtils.ApplySNShaders(fallingPod);
        fallingPod.AddComponent<AdminDropPodFall>();
        var ping = fallingPod.AddComponent<PingInstance>();
        ping.SetType(AdministratorDropPod.PingType);
        ping.origin = fallingPod.transform;
        fallingPod.SetActive(true);
    }

    private static IEnumerator SpawnPermanentBeacon()
    {
        var task = CraftData.GetPrefabForTechTypeAsync(AdminDropPodBeacon.Info.TechType);
        yield return task;
        var prefab = task.GetResult();
        var beacon = Instantiate(prefab, _finalPos + Vector3.up * 8, Quaternion.identity);
        beacon.SetActive(true);
        LargeWorldStreamer.main.cellManager.RegisterGlobalEntity(beacon);
    }

    private void Update()
    {
        if (_state is AnimationState.Falling or AnimationState.Submerged or AnimationState.SinkingToIsland)
        {
            _yVelocity -= Time.deltaTime * 9.8f;
            _yVelocity =
                Mathf.Min(_yVelocity +
                    Time.deltaTime * (_state == AnimationState.Falling ? _airDrag : _underwaterDrag) *
                    (_yVelocity * _yVelocity) / 2f, 0);
            transform.position += new Vector3(0, _yVelocity, 0) * Time.deltaTime;
        }

        switch (_state)
        {
            case AnimationState.Falling:
                if (transform.position.y < Ocean.GetOceanLevel())
                {
                    _state = AnimationState.Floating;
                    _floatStartTime = Time.time;
                    transform.position = new Vector3(transform.position.x, Ocean.GetOceanLevel(), transform.position.z);
                    _yVelocity = 0;
                }

                break;
            case AnimationState.Floating:
                if (Time.time > _floatStartTime + _floatDuration)
                {
                    _state = AnimationState.Submerged;
                }

                break;
            case AnimationState.Submerged:
                if (transform.position.y < _silenceGrabDepth)
                {
                    _state = AnimationState.Grabbed;
                    _yVelocity = 0f;
                    _grabStartTime = Time.time;
                    _grabPathRadius = Vector2.Distance(new Vector2(_spawnPos.x, _spawnPos.z),
                        new Vector2(_finalPos.x, _finalPos.z)) / 2f;
                    _grabPathCenter = (new Vector2(_spawnPos.x, _spawnPos.z) + new Vector2(_finalPos.x, _finalPos.z)) / 2f;
                    _grabStartAngle = Mathf.Atan2(_spawnPos.z - _grabPathCenter.y, _spawnPos.x - _grabPathCenter.x);
                }

                break;
            case AnimationState.Grabbed:
                if (Time.time >= _grabStartTime + _grabDuration)
                {
                    _state = AnimationState.SinkingToIsland;
                    break;
                }

                var angle = _grabStartAngle + (Time.time - _grabStartTime) * 3f * Mathf.PI / _grabDuration;
                
                transform.position = new Vector3(_grabPathCenter.x + Mathf.Cos(angle) * _grabPathRadius, _silenceGrabDepth,
                    _grabPathCenter.y + Mathf.Sin(angle) * _grabPathRadius);
                
                break;
            case AnimationState.SinkingToIsland:
                if (transform.position.y <= _finalPos.y)
                {
                    CoroutineHost.StartCoroutine(SpawnPermanentBeacon());
                    _state = AnimationState.Landed;
                    Destroy(gameObject);
                }

                break;
        }
    }

    private enum AnimationState
    {
        Falling,
        Floating,
        Submerged,
        Grabbed,
        SinkingToIsland,
        Landed
    }
}