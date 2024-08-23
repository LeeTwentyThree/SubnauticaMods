using System;
using UnityEngine;

namespace TheRedPlague.Mono.Tools;

public class DrifterCannonProjectile : MonoBehaviour
{
    public float damage = 200f;

    private void Start()
    {
        Destroy(gameObject, 10);
    }

    private void OnCollisionEnter(Collision other)
    {
        var lm = other.rigidbody.gameObject.GetComponent<LiveMixin>();
        if (lm != null)
            lm.TakeDamage(damage);
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        // death fx?
    }
}