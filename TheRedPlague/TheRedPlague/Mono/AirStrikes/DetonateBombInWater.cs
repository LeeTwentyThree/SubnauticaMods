using Nautilus.Utility;
using TheRedPlague.Mono.FleshBlobs;
using UnityEngine;

namespace TheRedPlague.Mono.AirStrikes;

public class DetonateBombInWater : MonoBehaviour
{
    public Rigidbody rigidbody;
    public GameObject explosionPrefab;
    public float maxDepth;

    private float _killTime;
    private float _maxLifeTime = 30;
    private float _explosionRadius = 27;

    private float _myActivationDepthOffset;

    private static FMODAsset _explodeSound = AudioUtils.GetFmodAsset("AirStrikeExplosion");

    private void Start()
    {
        _killTime = Time.time + _maxLifeTime;
        _myActivationDepthOffset = Random.Range(-15, 15);
    }

    private void OnDestroy()
    {
        Instantiate(explosionPrefab, transform.position, Quaternion.Euler(Vector3.up * Random.value * 360)).SetActive(true);
        DamageInRange(10000, _explosionRadius, true);
        DamageInRange(10000, _explosionRadius * 3, false);
        Utils.PlayFMODAsset(_explodeSound, transform.position);
        FleshBlobGrowth.GrowAllInRange(transform.position, 300, 0.1f);
    }

    private void DamageInRange(float damage, float range, bool affectPlayer)
    {
        var targets = UWE.Utils.OverlapSphereIntoSharedBuffer(transform.position, range, -1, QueryTriggerInteraction.Ignore);
        for (var i = 0; i < targets; i++)
        {
            var collider = UWE.Utils.sharedColliderBuffer[i];
            if (collider == null) continue;
            var liveMixin = collider.GetComponentInParent<LiveMixin>();
            if (liveMixin == null) continue;
            if (!affectPlayer && liveMixin.gameObject == Player.main.gameObject) continue;
            liveMixin.TakeDamage(damage, transform.position, DamageType.Explosive);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        Destroy(gameObject);
    }

    private void FixedUpdate()
    {
        if (rigidbody.velocity.sqrMagnitude < 0.2f)
        {
            Destroy(gameObject);   
        }

        if (transform.position.y < Ocean.GetOceanLevel())
        {
            rigidbody.AddForce(Vector3.down * 100, ForceMode.Acceleration);
        }
    }

    private void Update()
    {
        if (Time.time > _killTime || transform.position.y < maxDepth + _myActivationDepthOffset)
        {
            Destroy(gameObject);
        }
    }
}