using System.Collections.Generic;
using UnityEngine;

namespace TheRedPlague.Mono.Tools;

public class BombExplodeOnImpact : MonoBehaviour
{
    public float minImpulseMagnitude = 10f;
    public float explosionRadius = 4f;
    public float damage = 120f;
    public GameObject explosionPrefab;

    private void OnCollisionEnter(Collision other)
    {
        if (other.impulse.sqrMagnitude > minImpulseMagnitude * minImpulseMagnitude)
        {
            Destroy(gameObject);
            DamageInRange();
            SpawnFX();
        }
    }

    private void SpawnFX()
    {
        var obj = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        obj.transform.GetChild(1).gameObject.SetActive(false);
        Destroy(obj, 10);
    }

    private void DamageInRange()
    {
        var colliderCount = UWE.Utils.OverlapSphereIntoSharedBuffer(transform.position, explosionRadius);
        for (var i = 0; i < colliderCount; i++)
        {
            var collider = UWE.Utils.sharedColliderBuffer[i];
            var hitLiveMixins = new List<LiveMixin>();
            var lm = collider.gameObject.GetComponentInParent<LiveMixin>();
            if (lm == null) continue;
            if (hitLiveMixins.Contains(lm)) continue;
            lm.TakeDamage(damage, transform.position, DamageType.Explosive, gameObject);
            hitLiveMixins.Add(lm);
        }
    }
}