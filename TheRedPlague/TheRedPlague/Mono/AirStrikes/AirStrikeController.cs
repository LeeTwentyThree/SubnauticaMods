using System.Collections;
using System.Linq;
using UnityEngine;

namespace TheRedPlague.Mono.AirStrikes;

public class AirStrikeController : MonoBehaviour
{
    private float _spawnHeight = 200;
    private float _spawnRadius = 800;
    private float _dropDelay = 21;
    private int _spawnLocationAttempts = 50;
    private int _lineFormationUnits = 9;
    private float _lineFormationSpacing = 13;
    private float _lineFormationDepth = 35;
    
    public float DroneVelocity => _spawnRadius / _dropDelay;

    private GameObject _dronePrefab;

    private static AirStrikeController _main;

    public static AirStrikeController GetOrCreateInstance()
    {
        if (_main == null)
        {
            _main = new GameObject("AirStrikeController").AddComponent<AirStrikeController>();
        }

        return _main;
    }
    
    public GameObject BombModel { get; private set; }
    public GameObject ExplosionEffect { get; private set; }
    
    // Load essential prefabs/models
    private IEnumerator Start()
    {
        var task = CraftData.GetPrefabForTechTypeAsync(TechType.Seamoth);
        yield return task;
        var seamothPrefab = task.GetResult();
        
        // drone
        _dronePrefab = Instantiate(seamothPrefab.transform.Find("Model/Submersible_SeaMoth").gameObject);
        _dronePrefab.SetActive(false);
        _dronePrefab.transform.localScale *= 2;
        _dronePrefab.AddComponent<SkyApplier>().renderers = _dronePrefab.GetComponentsInChildren<Renderer>(true);
        
        // explosion
        ExplosionEffect = Instantiate(seamothPrefab.GetComponent<SeaMoth>().destructionEffect);
        ExplosionEffect.SetActive(false);
        var explosionFx = ExplosionEffect.GetComponentsInChildren<ParticleSystem>(true);
        foreach (var ps in explosionFx)
        {
            var main = ps.main;
            main.scalingMode = ParticleSystemScalingMode.Hierarchy;
        }
        DestroyImmediate(ExplosionEffect.transform.Find("x_SeamothFrags").gameObject);

        ExplosionEffect.transform.localScale *= 9;
        
        // bomb
        var whirlpoolTorpedo = seamothPrefab.GetComponent<SeaMoth>().torpedoTypes
            .First(t => t.techType == TechType.WhirlpoolTorpedo);
        BombModel = Instantiate(whirlpoolTorpedo.prefab.transform.GetChild(0).gameObject);
        BombModel.SetActive(false);
        BombModel.transform.localScale *= 3f;
        var bombCollider = BombModel.AddComponent<CapsuleCollider>();
        bombCollider.direction = 2;
        var bombRb = BombModel.AddComponent<Rigidbody>();
        bombRb.useGravity = false;
        bombRb.mass = 100;
        bombRb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        var bombWf = BombModel.AddComponent<WorldForces>();
        bombWf.useRigidbody = bombRb;
        var detonation = BombModel.AddComponent<DetonateBombInWater>();
        detonation.rigidbody = bombRb;
        detonation.explosionPrefab = ExplosionEffect;
    }

    public void AirStrikePreciseLocation(Vector3 targetPosition)
    {
        if (_dronePrefab == null)
        {
            Plugin.Logger.LogError("Drone prefab not yet initialized!");
        }
        var startLocation = DetermineStartLocation(targetPosition);
        var droneDirection =
            (new Vector3(targetPosition.x, _spawnHeight, targetPosition.z) - startLocation).normalized;
        var right = Vector3.Cross(droneDirection, Vector3.down);
        for (int i = 0; i < _lineFormationUnits; i++)
        {
            var progress = (float) i / (_lineFormationUnits - 1) - 0.5f;
            var distanceRight = progress * (_lineFormationUnits - 1) * _lineFormationSpacing;
            var distanceForward = Mathf.Abs(progress) * 2f * _lineFormationDepth;
            SpawnDroneAtPoint(startLocation + right * distanceRight - droneDirection * distanceForward, droneDirection, targetPosition.y);
        }
    }

    private void SpawnDroneAtPoint(Vector3 point, Vector3 direction, float targetYPos)
    {
        var drone = Instantiate(_dronePrefab, point, Quaternion.LookRotation(direction));
        drone.AddComponent<AirStrikeDrone>().SetUp(_dropDelay, DroneVelocity, targetYPos - 10f);
        drone.SetActive(true);
    }

    private Vector3 DetermineStartLocation(Vector3 targetPosition)
    {
        var camTransform = MainCamera.camera.transform;
        var camPos = camTransform.position;

        // Try heuristically to find a random spawn location
        for (int i = 0; i < _spawnLocationAttempts; i++)
        {
            var randomPos = GetRandomPotentialStartLocation(targetPosition);
            if (Vector3.Dot(camTransform.forward, (randomPos - camPos).normalized) < -0.01f)
            {
                return randomPos;
            }
        }

        // Otherwise just give up...
        return GetRandomPotentialStartLocation(targetPosition);
    }

    private Vector3 GetRandomPotentialStartLocation(Vector3 targetPosition)
    {
        var randomAngle = Random.value * Mathf.PI * 2f;
        return new Vector3(targetPosition.x + Mathf.Cos(randomAngle) * _spawnRadius, _spawnHeight, targetPosition.z + Mathf.Sin(randomAngle) * _spawnRadius);
    }

    private void OnDestroy()
    {
        Destroy(_dronePrefab);
        Destroy(BombModel);
        Destroy(ExplosionEffect);
    }
}