using UnityEngine;
using Random = UnityEngine.Random;

namespace KallieʼsPropPack.MonoBehaviours.SingleCell;

public class SclTentacleBehaviour : MonoBehaviour, IScheduledUpdateBehaviour
{
    private static readonly int IdleSpeed = Animator.StringToHash("idle_speed");
    
    public Animator animator;
    public Transform lightDetectionTransform;

    public float minIdleSpeed = 0.5f;
    public float maxIdleSpeed = 1f;
    public float lightDetectionRange = 100;
    public float minLightIntensity = 0.1f;
    public float emergeDurationMin = 30;
    public float emergeDurationMax = 60;
    public float minBuryDuration = 7f;

    private float _timeLightLastSeen;
    private float _timeCanToggleBuryAgain;
    private bool _emerged;
    private float _myEmergeDuration;
    
    public int scheduledUpdateIndex { get; set; }

    private bool _alwaysActive;

    private void Start()
    {
        _myEmergeDuration = Random.Range(emergeDurationMin, emergeDurationMax);
        // _alwaysActive = Random.value < 0.8f;
        _alwaysActive = true;
        _emerged = _alwaysActive;
        animator.SetBool("emerged", _emerged);
        animator.SetFloat(IdleSpeed, Random.Range(minIdleSpeed, maxIdleSpeed));
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