using System.Collections;
using System.Collections.Generic;
using RootMotion.FinalIK;
using Story;
using UnityEngine;
using UWE;

namespace TheRedPlague.Mono;

public class NpcSurvivor : MonoBehaviour
{
    public ModelType model;
    public string survivorName;
    public string requiredStoryGoal;
    public string deathStoryGoal;
    public float minInterval = 60;
    public float maxInterval = 80;
    public float swimVelocity = 4f;

    private bool _forceSpawn;

    private bool _started;
    private bool _died;

    private bool _performing;

    private float _timeReadyAgain;
    
    private float _timeTryAgain;

    private GameObject _instance;

    private Vector3 _pointA;
    private Vector3 _pointB;

    private List<System.Type> _typesToRemoveForScubaDiver = new List<System.Type>()
    {
        typeof(ConditionRules),
        typeof(ArmsController),
        typeof(FullBodyBipedIK),
        typeof(AimIK),
        typeof(SwimWaterSplash),
        typeof(PlayerAnimationEventManager)
    };

    private List<string> _activeModels = new List<string>()
    {
        "diveSuit",
        "scubaSuit"
    };

    private void Start()
    {
        if (model == ModelType.PrawnSuit)
        {
            enabled = false;
        }
        _timeReadyAgain = Time.time + Random.Range(minInterval, maxInterval);
    }

    private bool IsReadyToSpawn()
    {
        //if (_performing)
        //    return false;
        
        if (_forceSpawn && _instance == null)
            return true;

        if (Time.time < _timeReadyAgain)
            return false;
        
        if (!_started)
        {
            if (string.IsNullOrEmpty(requiredStoryGoal))
            {
                _started = true;
            }
            else if (StoryGoalManager.main.IsGoalComplete(requiredStoryGoal))
            {
                _started = true;
            }
        }
        
        if (!_died && !string.IsNullOrEmpty(deathStoryGoal) && StoryGoalManager.main.IsGoalComplete(deathStoryGoal))
        {
            _died = true;
        }
        
        return _started && !_died && _instance == null;
    }

    public void ForceSpawnWithCommand()
    {
        _forceSpawn = true;
    }

    private void Update()
    {
        if (Time.time > _timeTryAgain)
        {
            if (IsReadyToSpawn())
            {
                AttemptSpawn();
            }
            _timeTryAgain = Time.time + 0.5f;
        }
    }

    private void AttemptSpawn()
    {
        if (TryGetValidPath())
        {
            StartCoroutine(Perform());
        }
    }

    private IEnumerator Perform()
    {
        _performing = true;

        yield return SpawnInstance();

        _instance.transform.position = _pointA;
        if (model == ModelType.Diver) _instance.transform.LookAt(_pointB);
        else
        {
            var diff = _pointB - _pointA;
            diff.y = 0;
            _instance.transform.forward = diff.normalized;
        }
        if (model == ModelType.PrawnSuit) _instance.transform.position += Vector3.up;
        var motor = _instance.AddComponent<NpcSurvivorMotor>();
        var duration = Vector3.Distance(_pointA, _pointB) / swimVelocity;
        motor.SwimToPosition(_pointB, model == ModelType.PrawnSuit, duration);
        yield return new WaitForSeconds(duration);
        if (_instance)
        {
            var despawn = _instance.AddComponent<DespawnWhenOffScreen>();
            despawn.despawnIfViewNotClear = true;
            despawn.despawnIfTooClose = true;
            despawn.jumpscareWhenTooClose = true;
            despawn.minDistance = 10;
        }
        else
        {
            Plugin.Logger.LogError($"NPC survivor {survivorName} instance was destroyed!");
        }
        
        _performing = false;
        _forceSpawn = false;
        
        _timeReadyAgain = Time.time + Random.Range(minInterval, maxInterval);
    }

    private bool TryGetValidPath()
    {
        var cameraTransform = MainCamera.camera.transform;
        var cameraPos = cameraTransform.position;
        var inFrontOfCameraPos = cameraPos + cameraTransform.forward * 3;
        
        bool foundMidPos = false;
        Vector3 midPos = default;
        for (int i = 0; i < 20; i++)
        {
            var pos = cameraPos + cameraTransform.forward * Random.Range(25, 50) + Random.insideUnitSphere * 10;
            if (JumpScareUtils.IsPositionOnScreen(pos) && !Physics.SphereCast(new Ray(pos, (inFrontOfCameraPos - pos).normalized), 1, (inFrontOfCameraPos - pos).magnitude, -1, QueryTriggerInteraction.Ignore))
            {
                midPos = pos;
                foundMidPos = true;
                break;
            }
        }

        if (!foundMidPos)
        {
            // ErrorMessage.AddMessage("Failed to find mid pos");
            return false;
        }
        
        bool foundStartPos = false;
        Vector3 startPos = default;
        for (int i = 0; i < 20; i++)
        {
            var pos = midPos + Random.onUnitSphere * 20;
            if (Physics.SphereCast(new Ray(pos, (inFrontOfCameraPos - pos).normalized), 1, (inFrontOfCameraPos - pos).magnitude, -1, QueryTriggerInteraction.Ignore))
            {
                startPos = pos;
                foundStartPos = true;
                break;
            }
        }

        if (!foundStartPos)
        {
            // ErrorMessage.AddMessage("Failed to find start pos");
            return false;
        }

        if (Physics.Raycast(startPos, Vector3.down, out var hit, 20, -1, QueryTriggerInteraction.Ignore))
        {
            startPos = hit.point;
        }
        
        bool foundEndPos = false;
        Vector3 endPos = default;
        for (int i = 0; i < 20; i++)
        {
            var pos = midPos + Random.onUnitSphere * 20;
            if ((model == ModelType.PrawnSuit || !Physics.SphereCast(new Ray(startPos, (pos - startPos).normalized), 1, (pos - startPos).magnitude, -1, QueryTriggerInteraction.Ignore))
                && Physics.SphereCast(new Ray(pos, (inFrontOfCameraPos - pos).normalized), 1, (inFrontOfCameraPos - pos).magnitude, -1, QueryTriggerInteraction.Ignore))
            {
                endPos = pos;
                foundEndPos = true;
                break;
            }   
        }

        if (!foundEndPos)
        {
            // ErrorMessage.AddMessage("Failed to find end pos");
            return false;
        }

        _pointA = startPos;
        _pointB = endPos;
        
        return true;
    }

    private IEnumerator SpawnInstance()
    {
        if (model == ModelType.Diver)
        {
            _instance = Instantiate(Player.main.transform.Find("body/player_view").gameObject);
            _instance.SetActive(false);
            DestroyImmediate(_instance.GetComponent<ArmsController>());
            foreach (var component in _instance.GetComponentsInChildren<Component>())
            {
                if (_typesToRemoveForScubaDiver.Contains(component.GetType()))
                {
                    DestroyImmediate(component);
                }
            }

            var geo = _instance.transform.Find("male_geo");
            foreach (Transform child in geo)
            {
                child.gameObject.SetActive(_activeModels.Contains(child.gameObject.name));
            }
            foreach (Transform child in _instance.transform.Find("export_skeleton/head_rig/neck/chest/clav_R/clav_R_aim/shoulder_R/elbow_R/hand_R/attach1"))
            {
                child.gameObject.SetActive(false);
            }
            _instance.SetActive(true);
            var animator = _instance.gameObject.GetComponent<Animator>();
            animator.SetBool("is_underwater", true);
            animator.SetFloat("move_speed_z", 5);
            var headRenderer = geo.transform.Find("scubaSuit/scuba_head").gameObject.GetComponent<Renderer>();
            var material = headRenderer.materials[0];
            material.color = new Color(0, 0, 0, 1);
            material.SetFloat("_Cutoff", 0);
        }
        else if (model == ModelType.PrawnSuit)
        {
            var task = CraftData.GetPrefabForTechTypeAsync(TechType.Exosuit);
            yield return task;
            var exo = task.GetResult();
            var arm = exo.GetComponent<Exosuit>().armPrefabs[0].prefab;
            _instance = Instantiate(exo.transform.Find("exosuit_01").gameObject);
            _instance.SetActive(false);
            var collider = _instance.AddComponent<CapsuleCollider>();
            collider.center = Vector3.zero;
            collider.radius = 2;
            collider.height = 6;
            var leftArm = Instantiate(arm, _instance.transform.Find("root/geoChildren/lArm_clav")).transform;
            leftArm.localPosition = Vector3.zero;
            var rightArm = Instantiate(arm, _instance.transform.Find("root/geoChildren/rArm_clav"));
            rightArm.transform.localPosition = Vector3.zero;
            rightArm.transform.localScale = new Vector3(-1, 1, 1);
            var rb = _instance.AddComponent<Rigidbody>();
            rb.useGravity = false;
            rb.mass = 800;
            rb.freezeRotation = true;
            var wf = _instance.AddComponent<WorldForces>();
            wf.useRigidbody = rb;
            wf.underwaterGravity = 7;
            wf.underwaterDrag = 2;
            _instance.SetActive(true);
            var animator = _instance.GetComponent<Animator>();
            animator.SetBool("onGround", true);
            animator.SetBool("enterAnimation", true);
            animator.SetBool("player_in", true);
            animator.SetFloat("move_speed_z", 3);
        }
    }

    public enum ModelType
    {
        Diver,
        PrawnSuit
    }
}