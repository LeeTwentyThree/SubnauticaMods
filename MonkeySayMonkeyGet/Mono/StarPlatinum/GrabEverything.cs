using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MonkeySayMonkeyGet.Mono.StarPlatinum;

public class GrabEverything : OraSequence
{
    public List<GameObject> targetObjects;

    public Vector3 bringToPosition;

    public float positionVariation;

    private int TargetCount { get { return targetObjects.Count; } }

    private float _maxTime = 15f;

    private float _maxTimePerObject = 1.2f;

    private float _percentTimeSpentMoving = 0.1f;
    private float _percentTimeSpentGrabbing = 0.3f;
    private float _percentTimeSpentReturning = 0.3f;
    private float _percentTimeSpentIdleing = 0.3f;

    protected override IEnumerator LifetimeCoroutine()
    {
        var timeClamped = Mathf.Clamp(_maxTimePerObject * TargetCount, 0f, _maxTime);
        var actualTimePerObject = timeClamped / TargetCount;

        var forceEndTime = Time.realtimeSinceStartup + timeClamped + 1f;

        for (int i = 0; i < targetObjects.Count; i++)
        {
            var timeShouldEnd = Time.realtimeSinceStartup + actualTimePerObject;
            if (targetObjects[i] != null)
            {
                var bringToPos = bringToPosition + Random.insideUnitSphere * positionVariation;
                yield return GrabCoroutine(targetObjects[i], bringToPos, actualTimePerObject);
            }
            while (Time.realtimeSinceStartup < timeShouldEnd)
            {
                yield return null;
            }
            if (Time.realtimeSinceStartup > forceEndTime)
            {
                break;
            }
        }

        SetFlying(false);

        yield return new WaitForSeconds(2f);
    }

    private IEnumerator GrabCoroutine(GameObject target, Vector3 bringToPos, float maxTime)
    {
        if (target == null) yield break;
        var movingTime = maxTime * _percentTimeSpentMoving;
        var grabbingTime = maxTime * _percentTimeSpentGrabbing;
        var returnTime = maxTime * _percentTimeSpentReturning;
        var idleTime = maxTime * _percentTimeSpentIdleing;
        yield return FaceTargetCoroutine(target.transform.position, 0f);
        yield return MoveToPositionCoroutine(GetTargetPositionForGameObject(target), movingTime * 0.8f);
        if (target == null) yield break;
        yield return FaceTargetCoroutine(target.transform.position, movingTime * 0.2f);
        if (target == null) yield break;
        SetPunching(true);
        yield return new WaitForSecondsRealtime(grabbingTime);
        SetPunching(false);
        if (target == null) yield break;
        yield return FaceTargetCoroutine(bringToPos, 0f);
        FreezeGameObject(target.gameObject, true);
        target.transform.parent = transform;
        yield return MoveToPositionCoroutine(bringToPos, returnTime);
        SetPunching(true);
        yield return new WaitForSecondsRealtime(idleTime);
        SetPunching(false);
        if (target == null) yield break;
        target.transform.parent = null;
        var lwe = target.gameObject.GetComponent<LargeWorldEntity>();
        if (lwe) lwe.UpdateCell(LargeWorldStreamer.main);
        FreezeGameObject(target, false);
    }
}
