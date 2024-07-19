using UnityEngine;

namespace TheRedPlague.Mono.FleshBlobs;

public class FleshBlobInfluenced : MonoBehaviour
{
    private float _timeQueryAgain;
    private float _gravitationalForce;
    private Transform _targetTransform;
    private Rigidbody _rb;

    private bool _died;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _timeQueryAgain = Time.time + Random.value;
    }

    private void Update()
    {
        if (_died) return;
        
        if (Time.time > _timeQueryAgain)
        {
            _timeQueryAgain = Time.time + 0.5f;
            var target = FleshBlobGravity.GetStrongest(transform.position);
            if (target != null)
            {
                _targetTransform = target.transform;
                _gravitationalForce = target.GetGravitationalForceMagnitude(transform.position);
                if (Vector3.SqrMagnitude(_targetTransform.position - transform.position) < 30 * 30 * target.growth.Size * target.growth.Size)
                {
                    var lm = GetComponent<LiveMixin>();
                    if (lm && lm.IsAlive())
                    {
                        lm.TakeDamage(10000);
                        if (GetComponent<Creature>() != null && lm.maxHealth < 3000) Destroy(gameObject);
                        _died = true;
                    }
                }
            }
            else
            {
                _gravitationalForce = 0;
                _targetTransform = null;
            }
        }
    }
    
    private void FixedUpdate()
    {
        if (_gravitationalForce > 0.01f && _targetTransform != null && _rb != null)
        {
            var direction = (_targetTransform.position - transform.position).normalized;
            _rb.AddForce(direction * _gravitationalForce, ForceMode.Acceleration);
        }
    }
}