using UnityEngine;

namespace DeExtinction.Mono;

public class FishBleedOut : MonoBehaviour
{
    public GameObject dealer;
    
    private LiveMixin _liveMixin;
    
    private void Start()
    {
        InvokeRepeating(nameof(Damage), 0, 0.8f);
        _liveMixin = GetComponent<LiveMixin>();
    }

    private void Damage()
    {
        if (_liveMixin == null) return;
        if (dealer == null)
            _liveMixin.TakeDamage(1);
        else
            _liveMixin.TakeDamage(1, dealer.transform.position, DamageType.Normal, dealer);
    }
}