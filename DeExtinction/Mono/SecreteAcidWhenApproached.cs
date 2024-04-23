using UnityEngine;

namespace DeExtinction.Mono;

public class SecreteAcidWhenApproached : ReactToPredatorAction
{
    public float cooldown = 5;
    public GameObject acidPrefab;
    private float _timeStartPerformed;

    private Color[] _acidColors = new Color[]
    {
        new(2, 1, 0),
        new(1, 1, 3),
        new(1, 0.5f, 4),
        new(0, 2, 1)
    };
    
#if SUBNAUTICA
    public override void StartPerform(Creature creature, float time)
#elif BELOWZERO
    public override void StartPerform(float time)
#endif
    {
        _timeStartPerformed = Time.time;
        SpawnAcid();
    }

    protected override bool CanPerform()
    {
        return Time.time > _timeStartPerformed + cooldown;
    }

    private void SpawnAcid()
    {
        for (var i = 0; i < _acidColors.Length; i++)
        {
            SpawnColoredAcidParticle(_acidColors[i], transform.TransformPoint(0, 0, -i * 2));
        }
    }

    private void SpawnColoredAcidParticle(Color color, Vector3 position)
    {
        var acid = Instantiate(acidPrefab, position, Random.rotation);
        acid.SetActive(true);
        var collider = acid.AddComponent<SphereCollider>();
        collider.isTrigger = true;
        collider.radius = 3f;
        acid.AddComponent<AcidDamageTrigger>();
        var renderers = acid.GetComponentsInChildren<Renderer>();
        var firstRendererModified = false;
        foreach (var renderer in renderers)
        {
            if (!firstRendererModified)
            {
                renderer.material.color = color;
                renderer.transform.localScale *= 3;
                firstRendererModified = true;
                var main = renderer.gameObject.GetComponent<ParticleSystem>().main;
                main.startSizeMultiplier *= 3;
                main.scalingMode = ParticleSystemScalingMode.Local;
                main.startLifetimeMultiplier *= 3;
                continue;
            }

            renderer.enabled = false;
        }

        acid.GetComponentInChildren<VFXDestroyAfterSeconds>().lifeTime *= 3;
    }
}