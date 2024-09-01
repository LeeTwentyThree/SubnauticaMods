using UnityEngine;

namespace TheRedPlague.Mono.PlagueGarg.GargBehavior;

class PulseAttackFreeze : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private Creature _creature;
    private Animator _animator;

    private const float kShockInterval = 0.9f;
    private const float kShockDamage = 0.05f;

    private float _timeNextShock;

    public void Initialize(Creature myCreatureComponent, float length)
    {
        _creature = myCreatureComponent;
        _animator = myCreatureComponent.GetAnimator();
        _rigidbody = gameObject.GetComponent<Rigidbody>();
        if (_rigidbody) _rigidbody.isKinematic = true;
        if (_creature is Shocker)
        {
            DisableShockerFX(false);
        }
        Destroy(this, length);
    }

    void DisableShockerFX(bool active)
    {
        foreach (var onTouch in gameObject.GetComponentsInChildren<OnTouch>())
        {
            if (onTouch.gameObject.name != "Mouth")
            {
                onTouch.enabled = active;
            }
            transform.GetChild(0).gameObject.SetActive(active);
        }
    }

    void Update()
    {
        if (_creature != null)
        {
            if (_animator)
            {
                _animator.SetFloat(AnimatorHashID.flinch_damage, 69);
                _animator.SetTrigger(AnimatorHashID.flinch);
            }
            if (Time.time > _timeNextShock)
            {
                _creature.liveMixin.TakeDamage(kShockDamage, type: DamageType.Electrical);
                _timeNextShock = Time.time + kShockInterval;
            }
        }
    }

    void OnDestroy()
    {
        _rigidbody.isKinematic = false;
        if (_creature != null && _creature is Shocker)
        {
            DisableShockerFX(true);
        }
    }
}