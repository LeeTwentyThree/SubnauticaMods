using UnityEngine;

namespace DeExtinction.Mono;

public class FishBleedOut : MonoBehaviour
{
    private LiveMixin _liveMixin;
    
    private void Start()
    {
        InvokeRepeating(nameof(Damage), 0, 0.8f);
        _liveMixin = GetComponent<LiveMixin>();
    }

    private void Damage()
    {
        _liveMixin.TakeDamage(1);
    }
}