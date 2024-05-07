using UnityEngine;

namespace TheRedPlague.Mono.FleshBlobs;

public class FleshBlobInfluenced : MonoBehaviour
{
    private float _timeQueryAgain;
    private float _gravitationalForce;
    private Transform _targetTransform;
    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _timeQueryAgain = Time.time + Random.value;
    }

    private void Update()
    {
        if (Time.time > _timeQueryAgain)
        {
            _timeQueryAgain = Time.time + 0.5f;
            var target = FleshBlobGravity.GetClosest(transform.position);
            if (target != null)
            {
                _targetTransform = target.transform;
                _gravitationalForce =
                    Mathf.Clamp(
                        target.gravitationalConstant /
                        Vector3.SqrMagnitude(transform.position - _targetTransform.position), 0, 50);
                if (Vector3.SqrMagnitude(target.transform.position - transform.position) < 30 * 30)
                {
                    var lm = GetComponent<LiveMixin>();
                    if (lm && lm.IsAlive()) lm.TakeDamage(10000);
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