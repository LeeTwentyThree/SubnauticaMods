using System.Collections;
using Nautilus.Utility;
using UnityEngine;

namespace TheRedPlague.Mono.CreatureBehaviour.MrTeeth;

public class MrTeethAttackTrigger : MonoBehaviour
{
    public GameObject rootObject;
    public Animator animator;
    private float _timeAttackAgain;

    private static readonly FMODAsset GrabSound = AudioUtils.GetFmodAsset("MrTeethGrab");
    
    private void OnTriggerEnter(Collider other)
    {
        if (Time.time < _timeAttackAgain) return;
        if (other.gameObject != Player.main.gameObject) return;
        StartCoroutine(AttackPlayerCoroutine());
    }

    private IEnumerator AttackPlayerCoroutine()
    {
        _timeAttackAgain = Time.time + 5;
        
        animator.SetTrigger("bite");
        Utils.PlayFMODAsset(GrabSound, transform.position);
        Player.main.liveMixin.TakeDamage(15f);
        yield return new WaitForSeconds(0.3f);
        FadingOverlay.PlayFX(new Color(0.1f, 0f, 0f), 0.5f, 4f, 1f);
        yield return new WaitForSeconds(3f);
        Destroy(rootObject);
        if (!Player.main.IsFrozenStats() && MrTeethReturnPoint.TryGetClosest(transform.position, out var returnPosition))
        {
            Player.main.SetPosition(returnPosition + Vector3.up * 1.6f);
        }
    }
}