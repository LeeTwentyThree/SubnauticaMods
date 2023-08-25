using System.Collections;
using UnityEngine;

namespace MonkeySayMonkeyGet.Mono.StarPlatinum;

public class KillSmall : OraSequence
{
    public GameObject targetObject;

    protected override IEnumerator LifetimeCoroutine()
    {
        yield return FaceTargetCoroutine(targetObject.transform.position, 0f);
        FreezeGameObject(targetObject, true);
        yield return MoveToPositionCoroutine(GetTargetPositionForGameObject(targetObject), 0.5f);
        yield return FaceTargetCoroutine(targetObject.transform.position, 0.1f);
        SetPunching(true);
        Utils.PlaySoundEffect("OraMedium", 8f);
        var lm = targetObject.GetComponent<LiveMixin>();
        var creature = targetObject.GetComponent<Creature>();
        var flinch = targetObject.AddComponent<CreatureFlinch>();
        flinch.creature = creature;
        for (int i = 0; i < 8; i++)
        {
            yield return new WaitForSecondsRealtime(4f / 8f);
            if (lm) lm.TakeDamage(5f, ClosestPointForGameObject(targetObject, 1f));
            FreezeGameObject(targetObject, true);
        }
        SetPunching(false);
        PerformSinglePunch();
        yield return new WaitForSecondsRealtime(bigPunchDelay);
        FreezeGameObject(targetObject, false);
        lm.TakeDamage(10000f);
        AddHitForce(targetObject, 12000f, 100000f);
        Destroy(flinch);
    }
}
