using System;
using UnityEngine;

namespace TheRedPlague.Mono.Tools;

public class DrifterCannonProjectile : MonoBehaviour
{
    public float damage = 200f;

    private bool _struck;

    private void Start()
    {
        Destroy(gameObject, 15);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (_struck) return;
        if (other.rigidbody != null)
        {
            var lm = other.rigidbody.gameObject.GetComponent<LiveMixin>();
            if (lm != null)
                lm.TakeDamage(damage);
        }
        Destroy(gameObject, 5);
        _struck = true;
    }

    private void OnDestroy()
    {
        // death fx?
    }
}