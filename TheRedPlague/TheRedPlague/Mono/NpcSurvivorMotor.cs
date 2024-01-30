using System;
using UnityEngine;

namespace TheRedPlague.Mono;

public class NpcSurvivorMotor : MonoBehaviour
{
    private Vector3 _targetPos;
    private bool _prawnSuit;
    private float _totalTime;
    private Vector3 _startPos;
    private float _t;

    private Rigidbody _rb;
    private Animator _animator;
    private static readonly int OnGround = Animator.StringToHash("onGround");

    public void SwimToPosition(Vector3 position, bool prawnSuit, float totalTime)
    {
        _startPos = transform.position;
        _targetPos = position;
        _prawnSuit = prawnSuit;
        _totalTime = totalTime;
        if (_prawnSuit)
        {
            _rb = gameObject.GetComponent<Rigidbody>();
            _animator = gameObject.GetComponent<Animator>();
        }
    }

    private void Update()
    {
        if (!_prawnSuit)
        {
            transform.position = Vector3.LerpUnclamped(_startPos, _targetPos, _t);
            _t += Time.deltaTime / _totalTime;
        }

        if (!_prawnSuit && transform.position.y > Ocean.GetOceanLevel() + 1f)
        {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        if (_prawnSuit)
        {
            var onGround = Physics.Raycast(transform.position + Vector3.up, Vector3.down, 8, -1, QueryTriggerInteraction.Ignore);
            if (Physics.Raycast(transform.position, transform.forward, 7, -1, QueryTriggerInteraction.Ignore) || (onGround && Vector3.Distance(transform.position, MainCamera.camera.transform.position) > 60))
            {
                if (_rb.velocity.y < 4)
                {
                    _rb.AddForce(transform.up * 40, ForceMode.Acceleration);
                }
            }
            else
            {
                _rb.AddForce(transform.forward * 25, ForceMode.Acceleration);
            }
            
            _animator.SetBool(OnGround, onGround);
        }
    }
}