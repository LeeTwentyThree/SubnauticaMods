using System;
using UnityEngine;

namespace DeExtinction.Mono;

public class AcidDamageOverTime : MonoBehaviour
{
    public float damagePerTick = 6;
    public float damageInterval = 1.3f;
    
    private float _timeEnd;
    private float _timeDamageAgain;
    private LiveMixin _liveMixin;

    private void Start()
    {
        _liveMixin = GetComponent<LiveMixin>();
    }

    public void ResetTimer(float duration)
    {
        _timeEnd = Time.time + duration;
    }
    
    private void Update()
    {
        if (Time.time > _timeDamageAgain)
        {
            if (_liveMixin) _liveMixin.TakeDamage(damagePerTick, transform.position, DamageType.Acid);
            _timeDamageAgain = Time.time + damageInterval;
        }
        if (Time.time > _timeEnd)
        {
            Destroy(this);
        }
    }
}