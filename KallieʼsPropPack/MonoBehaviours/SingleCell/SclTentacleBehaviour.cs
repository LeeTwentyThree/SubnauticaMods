using System.Collections.Generic;
using Nautilus.Utility;
using UnityEngine;
using Random = UnityEngine.Random;

namespace KallieʼsPropPack.MonoBehaviours.SingleCell;

public class SclTentacleBehaviour : MonoBehaviour, IScheduledUpdateBehaviour
{
    private static readonly int IdleSpeed = Animator.StringToHash("idle_speed");
    
    public Animator animator;
    public Transform lightDetectionTransform;
    
    public Transform attackCenter;
    public float attackRadius = 20;

    public float minIdleSpeed = 0.5f;
    public float maxIdleSpeed = 1f;
    public float lightDetectionRange = 100;
    public float minLightIntensity = 0.1f;
    public float emergeDurationMin = 30;
    public float emergeDurationMax = 60;
    public float minBuryDuration = 7f;
    public float attackInterval = 10f;

    private float _timeLightLastSeen;
    private float _timeCanToggleBuryAgain;
    private bool _emerged;
    private float _myEmergeDuration;
    private float _timeFinishEmerge;

    private float _timeCanAttackAgain;
    
    public int scheduledUpdateIndex { get; set; }

    private bool _alwaysActive;

    private static readonly FMODAsset HitSound = AudioUtils.GetFmodAsset("SclTentacleHit");

    private void Start()
    {
        _myEmergeDuration = Random.Range(emergeDurationMin, emergeDurationMax);
        // _alwaysActive = Random.value < 0.8f;
        _alwaysActive = true;
        _emerged = _alwaysActive;
        animator.SetBool("emerged", _emerged);
        animator.SetFloat(IdleSpeed, Random.Range(minIdleSpeed, maxIdleSpeed));
        _timeFinishEmerge = Time.time + 9;
    }

    public void OnSensorFelt()
    {
        if (!_emerged)
            return;
        if (Time.time < _timeFinishEmerge)
            return;
        if (Time.time < _timeCanAttackAgain)
            return;
        animator.SetTrigger("attack");
        Invoke(nameof(DealDamage), 2);

        _timeCanAttackAgain = Time.time + attackInterval;
    }

    private void DealDamage()
    {
        var hits = UWE.Utils.OverlapSphereIntoSharedBuffer(attackCenter.position, attackRadius);
        var subs = new HashSet<SubRoot>();
        var vehicles = new HashSet<Vehicle>();
        for (int i = 0; i < hits; i++)
        {
            var collider = UWE.Utils.sharedColliderBuffer[i];
            if (collider == null) continue;
            var sub = collider.gameObject.GetComponentInParent<SubRoot>();
            if (sub != null)
            {
                subs.Add(sub);
                continue;
            }
            var vehicle = collider.gameObject.GetComponentInParent<Vehicle>();
            if (vehicle != null)
            {
                vehicles.Add(vehicle);
            }
        }

        bool dealtDamage = false;
        
        foreach (var sub in subs)
        {
            if (sub.live) sub.live.TakeDamage(37);
            if (sub.rb)
            {
                var direction = (sub.transform.position - transform.position).normalized;
                sub.rb.AddForce(direction * 8, ForceMode.VelocityChange);
            }
            dealtDamage = true;
        }

        foreach (var vehicle in vehicles)
        {
            if (vehicle.liveMixin) vehicle.liveMixin.TakeDamage(20);
            if (vehicle.useRigidbody)
            {
                var direction = (vehicle.transform.position - transform.position).normalized;
                vehicle.useRigidbody.AddForce(direction * 17, ForceMode.VelocityChange);
            }
            dealtDamage = true;
        }

        if (dealtDamage)
        {
            FMODUWE.PlayOneShot(HitSound, attackCenter.position);
        }
    }

    private void OnEnable()
    {
        UpdateSchedulerUtils.Register(this);
        animator.SetBool("emerged", _emerged);
    }

    private void OnDisable()
    {
        UpdateSchedulerUtils.Deregister(this);
    }

    public string GetProfileTag()
    {
        return "SclTentacle";
    }

    private void EvaluateRandom()
    {
        animator.SetFloat("random", Random.value);
    }

    public void ScheduledUpdate()
    {
        if (_alwaysActive)
            return;
        
        var nearestLight = RegistredLightSource.GetNearestLight(lightDetectionTransform.position, lightDetectionRange);
        if (nearestLight != null && nearestLight.GetIntensity() > minLightIntensity)
        {
            _timeLightLastSeen = Time.time;
        }

        if (Time.time > _timeCanToggleBuryAgain)
        {
            bool shouldEmerge = Time.time < _timeLightLastSeen + _myEmergeDuration;

            if (_emerged == shouldEmerge)
            {
                return;
            }
            
            _emerged = shouldEmerge;
            EvaluateRandom();
            animator.SetBool("emerged", _emerged);
            
            _timeCanToggleBuryAgain = Time.time + (_emerged ? emergeDurationMin : minBuryDuration);
        }
    }
}