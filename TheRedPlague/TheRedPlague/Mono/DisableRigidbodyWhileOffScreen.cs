using Nautilus.Utility;
using UnityEngine;

namespace TheRedPlague.Mono;

public class DisableRigidbodyWhileOffScreen : MonoBehaviour
{
    private Rigidbody _rb;
    private Animator _animator;

    public bool playAttackSound = true;
    public FMODAsset attackSound = AudioUtils.GetFmodAsset("ZombieRoar");

    private bool _wasAttackingPlayer;
    private AttackLastTarget _attack;

    private float _timeCanPlaySoundAgain;
    
    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _animator = GetComponentInChildren<Animator>();
        _attack = GetComponent<AttackLastTarget>();
    }

    private void Update()
    {
        var canMove = !JumpScareUtils.IsPositionOnScreen(transform.position);
        _rb.isKinematic = !canMove;
        _animator.speed = canMove ? 1 : 0;

        if (!playAttackSound) return;
        
        var attacking = Time.time > _attack.timeStartAttack && Time.time < _attack.timeStopAttack && _attack.currentTarget == Player.main.gameObject;
        
        if (!_wasAttackingPlayer && attacking && canMove && Time.time > _timeCanPlaySoundAgain)
        {
            Utils.PlayFMODAsset(attackSound, transform.position);
            _timeCanPlaySoundAgain = Time.time + 5f;
        }

        _wasAttackingPlayer = attacking;
    }
}