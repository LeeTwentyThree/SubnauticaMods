using UnityEngine;

namespace DeExtinction.Mono;

public class SlowDrowning : MonoBehaviour
{
    public float damage = 2f;
    public float damageInterval = 1f;
    public float oxygenCapacitySeconds = 15f;
    public Animator animator;

    private bool _submerged;
    private bool _drowning;
    private float _timeNextDamage;
    private LiveMixin _liveMixin;
    private IDrownableCreature _creature;

    private float _timeStartedDrowning;
    
    private static readonly int DrowningParameter = Animator.StringToHash("drowning");

    private void Start()
    {
        _creature = GetComponent<IDrownableCreature>();
        _liveMixin = GetComponent<LiveMixin>();
    }

    private void Update()
    {
        var currentlySubmerged = Ocean.GetDepthOf(gameObject) > 0;
        if (!_submerged && currentlySubmerged)
        {
            _timeStartedDrowning = Time.time;
        }

        _submerged = currentlySubmerged;

        var shouldDrown = currentlySubmerged && Time.time > _timeStartedDrowning + oxygenCapacitySeconds;
        if (_drowning != shouldDrown)
        {
            _drowning = shouldDrown;
            _creature.drowning = _drowning;
            animator.SetBool(DrowningParameter, _drowning);
        }
        if (_drowning && Time.time > _timeNextDamage)
        {
            _timeNextDamage = Time.time + damageInterval;
            if (_liveMixin != null)
            {
                _liveMixin.TakeDamage(damage, transform.position);
            }
        }
    }
}