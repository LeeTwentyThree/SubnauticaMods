namespace TheRedPlague.Mono.PlagueGarg.GargBehavior.GargEssentials;

using System.Collections.Generic;
using UnityEngine;
    
public class GargantuanBehaviour : MonoBehaviour
{
    public Transform EyeTrackTarget { get; private set; }

    public GameObject CachedBloodPrefab { get; private set; }
        
    public GargantuanRoar roar;
    public SwimBehaviour swim;
    public LastTarget lastTarget;
    public float timeSpawnBloodAgain;
    public float bloodDestroyTime;
    public float timeCanAttackAgain;
        
    internal Creature creature;
    internal GargantuanGrab grab;
    private float _timeUpdateEyeTargetAgain;
    private float _timeCanPerformRangedAttackAgain;
    private const float kUpdateEyeTargetInterval = 0.2f;

    private float _timeUnpacify;

    private const float kRangedAttackCooldown = 30f;
    private const float kLookAtPlayerMaxDistance = 500f;

    private List<Transform> _spineBoneList;

    void Start()
    {
        grab = GetComponent<GargantuanGrab>();
        creature = GetComponent<Creature>();
        roar = GetComponent<GargantuanRoar>();
        swim = GetComponent<SwimBehaviour>();
        lastTarget = gameObject.GetComponent<LastTarget>();
    }

    public List<Transform> GetSpineBoneList()
    {
        if (_spineBoneList == null)
        {
            _spineBoneList = new List<Transform>();
            var spineRoot = gameObject.GetComponentInChildren<Animator>().transform.Find("Armature/Head/Spine");
            if (spineRoot == null)
            {
                return _spineBoneList;
            }
            Transform currentSpine = spineRoot;
            while (true)
            {
                if (currentSpine == null)
                {
                    break;
                }
                bool reachedEnd = true;
                foreach (Transform child in currentSpine)
                {
                    if (child.name.ToLower().Contains("spine"))
                    {
                        currentSpine = child;
                        _spineBoneList.Add(currentSpine);
                        reachedEnd = false;
                    }
                }
                if (reachedEnd)
                {
                    break;
                }
            }
        }
        return _spineBoneList;
    }

    public void PacifyForSeconds(float seconds)
    {
        _timeUnpacify = Time.time + seconds;
        creature.Aggression.Value = 0f;
    }

    public bool IsPacified()
    {
        return Time.time < _timeUnpacify;
    }

    public void SetRangedAttackCooldown()
    {
        _timeCanPerformRangedAttackAgain = Time.time + kRangedAttackCooldown;
    }

    public bool CanPerformRangedAttack()
    {
        return Time.time > _timeCanPerformRangedAttackAgain && CanPerformAttack();
    }

    public bool CanPerformAttack()
    {
        if (IsPacified())
        {
            return false;
        }
        return Time.time > timeCanAttackAgain;
    }

    public bool CanEat(GameObject target)
    {
        return target.GetComponent<Creature>() || target.GetComponent<Player>() || target.GetComponent<Vehicle>() || target.GetComponent<SubRoot>();
    }

    private void Update()
    {
        if (Time.time > _timeUpdateEyeTargetAgain)
        {
            EyeTrackTarget = FindEyeTarget();
            _timeUpdateEyeTargetAgain = Time.time + kUpdateEyeTargetInterval;
        }
    }

    private Transform FindEyeTarget()
    {
        if (lastTarget.target != null && creature.lastAction is AttackLastTarget)
        {
            return lastTarget.target.transform;
        }
        var mainCamera = MainCameraControl.main.transform;
        if (mainCamera != null && Vector3.Distance(mainCamera.position, transform.position) < kLookAtPlayerMaxDistance)
        {
            return MainCameraControl.main.transform;
        }
        return null;
    }

    public bool GetBloodEffectFromCreature(GameObject creature, float startSizeScale, float lifetimeScale)
    {
        Destroy(CachedBloodPrefab); // don't want a build-up of inactive prefabs!

        if (creature == null)
        {
            return false;
        }
        LiveMixin lm = creature.GetComponent<LiveMixin>();
        if (lm == null)
        {
            return false;
        }
        if (lm.data == null)
        {
            return false;
        }
        GameObject prefab = lm.data.damageEffect;
        if (prefab == null)
        {
            return false;
        }
        CachedBloodPrefab = Instantiate(prefab);
        CachedBloodPrefab.SetActive(false);
        foreach (ParticleSystem ps in CachedBloodPrefab.GetComponentsInChildren<ParticleSystem>(true))
        {
            var main = ps.main;
            main.startLifetime = new ParticleSystem.MinMaxCurve(main.startLifetime.constant * lifetimeScale);
            main.startSize = new ParticleSystem.MinMaxCurve(main.startSize.constant * startSizeScale);
        }
        bloodDestroyTime = 10f * lifetimeScale;
        foreach (var destroyAfterSeconds in CachedBloodPrefab.GetComponentsInChildren<VFXDestroyAfterSeconds>(true)) // vfxdestroyafterseconds is bad, is timescale-independent, and we get more control if we don't want the *prefab* to destroy itself anyway
        {
            Destroy(destroyAfterSeconds);
        }

        return true;
    }
        
    public Quaternion InverseRotation(Quaternion input)
    {
        return Quaternion.Euler(input.eulerAngles + new Vector3(0f, 180f, 0f));
    }
    public Quaternion FixSmallFishRotation(Quaternion input)
    {
        return Quaternion.Euler(input.eulerAngles + new Vector3(0f, 0f, 90f));
    }
    public Vector3 FixJuvenileFishHoldPosition(Transform holdPoint, Vector3 input)
    {
        return input + (holdPoint.up * 3f) + (holdPoint.forward * -5f);
    }

    public void OnDamagedByArchElectricity()
    {
        if (grab.HeldVehicle != null)
        {
            grab.ReleaseHeld();
        }
        else
        {
            creature.Scared.Value = 1f;
            creature.Aggression.Value = 0f;
            timeCanAttackAgain = Time.time + 5f;
        }
        if (lastTarget != null) lastTarget.target = null;
    }

    private void OnDestroy()
    {
        Destroy(CachedBloodPrefab);
    }
}