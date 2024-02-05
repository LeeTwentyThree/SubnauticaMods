using UnityEngine;

namespace TheRedPlague.Mono;

public class DisableRigidbodyWhileOffScreen : MonoBehaviour
{
    private Rigidbody _rb;
    private Animator _animator;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        bool canMove = !JumpScareUtils.IsPositionOnScreen(transform.position);
        _rb.isKinematic = !canMove;
        _animator.speed = canMove ? 1 : 0;
    }
}