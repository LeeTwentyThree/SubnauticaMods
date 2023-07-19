using UnityEngine;

namespace DeExtinction.Mono;

internal class FiltorbHide : ReactToPredatorAction, IOnTakeDamage
{
    private Animator _animator;
    private Pickupable _pickupable;
    private DamageModifier _closedDamageModifier;
    private VFXSurface _vfxSurface;
    private bool _closed;

    public void SetClosed(bool newState)
    {
        if (_closed == newState)
        {
            return;
        }
        _closed = newState;
        if (_closed)
        {
            _closedDamageModifier.enabled = true;
            _pickupable.isPickupable = false;
            _animator.SetBool("open", false);
            _vfxSurface.surfaceType = VFXSurfaceTypes.metal;
        }
        else
        {
            _closedDamageModifier.enabled = false;
            _pickupable.isPickupable = true;
            _animator.SetBool("open", true);
            _vfxSurface.surfaceType = VFXSurfaceTypes.organic;
        }
    }

    private void Start()
    {
        _animator = GetComponentInChildren<Animator>();
        _pickupable = GetComponent<Pickupable>();
        _closedDamageModifier = gameObject.EnsureComponent<DamageModifier>();
        _closedDamageModifier.multiplier = 0.5f;
        _closedDamageModifier.damageType = DamageType.Normal;
        _vfxSurface = GetComponent<VFXSurface>();
    }

    public override void StartPerform(Creature creature, float time)
    {
        SetClosed(true);
    }

    public override void StopPerform(Creature creature, float time)
    {
        SetClosed(false);
    }

    private void OnKill()
    {
        SetClosed(false);
    }

    public void OnTakeDamage(DamageInfo damageInfo)
    {
        performingAction = true;
        timeStopAction = Time.time + 2f;
    }
}