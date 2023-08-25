using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace MonkeySayMonkeyGet.Mono.StarPlatinum;

public abstract class OraSequence : MonoBehaviour
{
    public Animator animator;

    public List<GameObject> armVFX;

    public float defaultVelocity = 20f;
    public Vector3 genericOffset = new Vector3(0f, -7f, 0f);
    public Vector3 genericOffsetWithZ = new Vector3(0f, -7f, -2f);
    public float centerY = 7f;
    public float distanceFromTarget = 2f;
    public float bigPunchDelay = 1.65f;

    private bool _punching;
    private float _timeShakeCameraAgain;

    private void Start()
    {
        StartCoroutine(InternalLifetimeCoroutine());
    }

    private IEnumerator InternalLifetimeCoroutine()
    {
        yield return LifetimeCoroutine();
        EndSequence();
    }

    protected abstract IEnumerator LifetimeCoroutine();

    protected void SetPunching(bool state)
    {
        animator.SetBool("punching", state);
        _punching = state;
        if (state)
        {
            EnableFists(true);
        }
        else
        {
            EnableFists(false);
        }
    }

    protected void SetFlying(bool state)
    {
        animator.SetBool("flying", state);
        if (state)
        {
            EnableFists(false);
        }
    }

    protected void PerformSinglePunch()
    {
        animator.SetTrigger("big_punch");
        Invoke(nameof(BigPunchShake), bigPunchDelay);
        EnableFists(false);
    }

    public void EndSequence()
    {
        Destroy(this);
        SetPunching(false);
        SetFlying(false);
    }

    protected IEnumerator MoveToPositionCoroutine(Vector3 position, float maxTravelTime)
    {
        SetFlying(true);
        float distance = Vector3.Distance(transform.position, position);
        var travelTime = distance / defaultVelocity;
        var timeClamped = Mathf.Clamp(travelTime, 0f, maxTravelTime);
        var speedScale = 1f - (timeClamped - travelTime);
        var velocity = defaultVelocity * speedScale;
        var timeEnd = Time.realtimeSinceStartup + timeClamped;
        while (Time.realtimeSinceStartup < timeEnd)
        {
            yield return null;
            transform.position = Vector3.MoveTowards(transform.position, position, Time.unscaledDeltaTime * velocity);
        }
        transform.position = position;
        SetFlying(false);
    }

    protected IEnumerator FaceTargetCoroutine(Vector3 objectPosition, float duration)
    {
        var timeEnd = Time.realtimeSinceStartup + duration;
        var direction = objectPosition - (transform.position + transform.up * centerY);
        direction.y = 0f;
        direction.Normalize();
        var targetRot = Quaternion.LookRotation(direction);
        var angle = Quaternion.Angle(transform.rotation, targetRot);
        while (Time.realtimeSinceStartup < timeEnd)
        {
            yield return null;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, Time.unscaledDeltaTime * angle / duration);
        }
        transform.rotation = targetRot;
    }

    protected void FreezeGameObject(GameObject obj, bool freeze)
    {
        if (obj == null)
        {
            return;
        }
        var rb = obj.GetComponent<Rigidbody>();
        if (rb)
        {
            rb.isKinematic = freeze;
        }
    }

    protected void AddHitForce(GameObject obj, float force, float heavyForce)
    {
        var rb = obj.GetComponent<Rigidbody>();
        if (rb)
        {
            var hitForce = force;
            if (rb.mass > 1000f)
            {
                hitForce = heavyForce;
            }
            rb.AddForce(transform.forward * hitForce, ForceMode.Impulse);
        }
    }

    protected Vector3 GetTargetPositionForGameObject(GameObject targetObject)
    {
        if (targetObject == null)
        {
            return transform.position + Random.onUnitSphere;
        }
        var creature = targetObject.GetComponent<Creature>();
        var mainCollider = targetObject.GetComponent<Collider>();
        if (creature && mainCollider)
        {
            return mainCollider.ClosestPoint(targetObject.transform.position + (targetObject.transform.forward * 10f)) + targetObject.transform.forward * distanceFromTarget + genericOffset;
        }
        var player = targetObject.GetComponent<Player>();
        if (mainCollider)
        {
            var closestPoint = mainCollider.ClosestPoint(transform.position);
            var directionFromCenterToClosestPoint = (closestPoint - targetObject.transform.position).normalized;
            var finalPos = targetObject.transform.position + directionFromCenterToClosestPoint * distanceFromTarget + genericOffset;
            if (player != null)
            {
                finalPos += Vector3.up * 2f;
            }
            return finalPos;
        }
        var directionFromCenterToMyself = (targetObject.transform.position - transform.position).normalized;
        return targetObject.transform.position + directionFromCenterToMyself * distanceFromTarget + genericOffset;
    }

    protected Vector3 ClosestPointForGameObject(GameObject targetObject, float variation)
    {
        var vectorRandom = Random.insideUnitSphere * variation;
        if (targetObject == null)
        {
            return transform.position + transform.TransformPoint(genericOffsetWithZ) + vectorRandom;
        }
        var mainCollider = targetObject.GetComponent<Collider>();
        if (mainCollider)
        {
            return mainCollider.ClosestPoint(transform.position) + vectorRandom;
        }
        return targetObject.transform.position + vectorRandom;
    }

    protected void EnableFists(bool enable)
    {
        foreach (var arm in armVFX)
        {
            arm.SetActive(enable);
        }
    }

    private void Update()
    {
        if (_punching)
        {
            if (Time.time > _timeShakeCameraAgain)
            {
                MainCameraControl.main.ShakeCamera(2f, 0.3f, MainCameraControl.ShakeMode.Quadratic, 2f);
                _timeShakeCameraAgain = Time.time + 0.3f;
            }
        }
    }

    private void BigPunchShake()
    {
        if (_punching)
        {
            return;
        }
        MainCameraControl.main.ShakeCamera(8f, 3f, MainCameraControl.ShakeMode.Quadratic, 1.6f);
    }
}
