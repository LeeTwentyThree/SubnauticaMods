using Nautilus.Utility;
using UnityEngine;

namespace TheRedPlague.Mono;

public class DisableRigidbodyWhileOnScreen : MonoBehaviour
{
    private Rigidbody _rb;
    private Animator _animator;

    public bool playAttackSound = false;
    private static readonly FMODAsset AttackSound = AudioUtils.GetFmodAsset("ZombieRoar");

    private bool _wasAttackingPlayer;
    private AttackLastTarget _attack;
    private Creature _creature;

    private float _timeCanPlaySoundAgain;
    private float _timeUnfreezeEnd;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _animator = GetComponentInChildren<Animator>();
        _attack = GetComponent<AttackLastTarget>();
        _creature = GetComponent<Creature>();
    }

    private void Update()
    {
        var canMove = !JumpScareUtils.IsPositionOnScreen(transform.position, 0.2f) && Time.time > _timeUnfreezeEnd;
        _rb.isKinematic = !canMove;
        _animator.speed = canMove ? 1 : 0;

        if (!playAttackSound) return;
        
        var attacking = _creature.prevBestAction == _attack;
        
        if (!_wasAttackingPlayer && attacking && canMove && Time.time > _timeCanPlaySoundAgain)
        {
            Utils.PlayFMODAsset(AttackSound, transform.position);
            _timeCanPlaySoundAgain = Time.time + 5f;
        }

        _wasAttackingPlayer = attacking;
    }
    
    public void UnfreezeForDuration(float duration)
    {
        _timeUnfreezeEnd = Time.time + duration;
    }
}