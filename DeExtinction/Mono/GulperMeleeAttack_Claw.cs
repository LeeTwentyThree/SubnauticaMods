using ECCLibrary.Mono;
using Nautilus.Extensions;
using Nautilus.Utility;
using UnityEngine;

namespace DeExtinction.Mono;

public class GulperMeleeAttackClaw : MeleeAttack
{
    public string animationTriggerName;
    public GameObject clawObject;
    public bool isBaby;

    LiveMixin _damagingTarget;
    private FMOD_CustomEmitter _clawEmitter;
    private FMODAsset _clawSound = AudioUtils.GetFmodAsset("GulperClawAttack");

    void Start()
    {
        _clawEmitter = gameObject.AddComponent<FMOD_CustomEmitter>();
        _clawEmitter.followParent = true;
        _clawEmitter.SetAsset(_clawSound);
    }

    public override void OnTouch(Collider collider)
    {
        if (frozen) return;
        if (liveMixin.IsAlive() && Time.time > timeLastBite + biteInterval)
        {
            _damagingTarget = collider.GetComponent<LiveMixin>();
            if (_damagingTarget == null || _damagingTarget.maxHealth < 100f)
            {
                return;
            }

            Player player = collider.gameObject.GetComponent<Player>();
            if (player != null && (!player.CanBeAttacked() || isBaby))
            {
                return;
            }

            animator.SetTrigger(animationTriggerName);
            _clawEmitter.Play();
            Invoke("DamageTarget", 0.6f);
            timeLastBite = Time.time;
        }
    }

    void DamageTarget()
    {
        if (_damagingTarget != null)
        {
            _damagingTarget.TakeDamage(25f, clawObject.transform.position, DamageType.Normal, gameObject);
        }
    }
}