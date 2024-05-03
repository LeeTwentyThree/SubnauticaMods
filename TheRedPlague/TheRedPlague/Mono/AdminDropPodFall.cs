using System.Collections;
using Nautilus.Utility;
using TheRedPlague.PrefabFiles;
using UnityEngine;
using UWE;

namespace TheRedPlague.Mono;

public class AdminDropPodFall : MonoBehaviour
{
    private static readonly Vector3 _spawnPos = new Vector3(180, 1000, 2316);
    private static readonly Vector3 _finalPos = new Vector3(-175.090f, -667.283f, 3285.797f);
    private static readonly float _silenceGrabDepth = -398;
    private static readonly Vector3 _finalEulerAngles = new Vector3(8, 34.3f, 0.36f);

    private AnimationState _state;

    private float _yVelocity;
    private float _airDrag = 0.2f;
    private float _underwaterDrag = 2f;
    private float _grabDuration = 30f;
    
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

        var fallingDropPod = Instantiate(prefab, _spawnPos, Quaternion.identity);
        fallingDropPod.AddComponent<AdminDropPodFall>();
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

    private static void SpawnPermanentBeacon()
    {
        
    }

    private void Update()
    {
        if (_state != AnimationState.Grabbed)
        {
            _yVelocity -= Time.deltaTime * 9.8f;
            _yVelocity += (_state == AnimationState.Falling ? _airDrag : _underwaterDrag) * (_yVelocity * _yVelocity) / 2f;
        }

        switch (_state)
        {
            case AnimationState.Falling:
                if (transform.position.y < Ocean.GetOceanLevel())
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
                    _grabStartAngle = Mathf.Atan2(_finalPos.z - _grabPathCenter.y, _finalPos.x - _grabPathCenter.x);
                }

                break;
            case AnimationState.Grabbed:
                if (Time.time >= Time.time - _grabStartTime)
                {
                    _state = AnimationState.SinkingToIsland;
                    break;
                }

                var angle = _grabStartAngle + (Time.time - _grabStartTime) * 3f * Mathf.PI / _grabDuration;
                
                transform.position = new Vector3(_grabPathCenter.x + Mathf.Cos(angle) * _grabPathRadius, _silenceGrabDepth,
                    _grabPathCenter.y + Mathf.Sin(angle) * _grabPathRadius);
                
                break;
            case AnimationState.SinkingToIsland:
                if (transform.position.y < _silenceGrabDepth)
                {
                    ErrorMessage.AddMessage("Lifepod landed");
                    SpawnPermanentBeacon();
                    _state = AnimationState.Landed;
                }

                break;
        }
    }

    private enum AnimationState
    {
        Falling,
        Submerged,
        Grabbed,
        SinkingToIsland,
        Landed
    }
}