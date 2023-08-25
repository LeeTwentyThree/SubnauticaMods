using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MonkeySayMonkeyGet.Mono.StarPlatinum;

public class KillList : OraSequence
{
    public List<GameObject> targetObjects;

    private int TargetCount { get { return targetObjects.Count; } }

    private float _totalTime = 19f;

    private float _percentTimeSpentMoving = 0.15f;
    private float _percentTimeSpentPunching = 0.85f;
    private int _hitsPerAttack = 8;

    protected override IEnumerator LifetimeCoroutine()
    {
        SetPunching(true);

        Utils.PlaySoundEffect("Ora21Seconds", 30f);

        float timePerOne = _totalTime / TargetCount;

        var forceEndTime = Time.realtimeSinceStartup + _totalTime + 1f;

        for (int i = 0; i < targetObjects.Count; i++)
        {
            var timeShouldEnd = Time.realtimeSinceStartup + timePerOne;
            if (targetObjects[i] != null)
            {
                yield return DamageCoroutine(targetObjects[i], timePerOne, i == targetObjects.Count - 1);
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

        SetPunching(false);
        SetFlying(false);

        yield return new WaitForSeconds(2f);
    }

    private IEnumerator DamageCoroutine(GameObject target, float maxTime, bool lastOne)
    {
        if (target == null) yield break;
        var movingTime = maxTime * _percentTimeSpentMoving;
        var punchingTime = maxTime * _percentTimeSpentPunching;
        SetPunching(false);
        SetFlying(true);
        yield return FaceTargetCoroutine(target.transform.position, 0f);
        FreezeGameObject(target, true);
        yield return MoveToPositionCoroutine(GetTargetPositionForGameObject(target), movingTime * 0.8f);
        if (target == null) yield break;
        yield return FaceTargetCoroutine(target.transform.position, movingTime * 0.2f);
        if (target == null) yield break;
        SetPunching(true);
        SetFlying(false);
        var lm = target.GetComponent<LiveMixin>();
        var creature = target.GetComponent<Creature>();
        var flinch = target.AddComponent<CreatureFlinch>();
        flinch.creature = creature;
        for (int i = 0; i < _hitsPerAttack; i++)
        {
            yield return new WaitForSecondsRealtime(punchingTime / _hitsPerAttack);
            if (lm) lm.TakeDamage(5f, ClosestPointForGameObject(target, 1f));
            FreezeGameObject(target, true);
        }
        if (target == null) yield break;
        if (lastOne)
        {
            SetPunching(false);
            PerformSinglePunch();
            yield return new WaitForSecondsRealtime(bigPunchDelay);
        }
        FreezeGameObject(target, false);
        if (lm) lm.TakeDamage(10000f);
        AddHitForce(target, 6000f, 100000f);
        Destroy(flinch);
    }
}
