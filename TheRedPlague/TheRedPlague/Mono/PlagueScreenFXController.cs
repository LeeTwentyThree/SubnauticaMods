using System;
using System.Collections;
using TheRedPlague.Mono.FleshBlobs;
using TheRedPlague.PrefabFiles;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TheRedPlague.Mono;

public class PlagueScreenFXController : MonoBehaviour
{
    public float effectStrengthMultiplier = 0.85f;

    public float minEffect = 0.3f;
    public float maxEffect = 0.4f;
    
    public float minDistance = 20;
    public float maxDistance = 150;

    public float fadeDuration = 0.25f;

    private float prevAmount;

    private float animTime;

    private RadiationsScreenFX fx;

    private float effectStrength;

    private bool _ready;
    
    // my attempt not to mess with the RadiationsScreenFXController class:
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(1f);
        var existingRadiationScreenFx = gameObject.GetComponent<RadiationsScreenFX>();
        fx = gameObject.AddComponent<RadiationsScreenFX>();
        fx.shader = existingRadiationScreenFx.shader;
        fx.color = Color.red;
        InvokeRepeating(nameof(UpdateEffect), Random.value, 0.5f);
        _ready = true;
    }

    private void Update()
    {
        if (!_ready) return;
        
        if (effectStrength >= prevAmount && effectStrength > 0f)
        {
            animTime += Time.deltaTime / fadeDuration;
        }
        else
        {
            animTime -= Time.deltaTime / fadeDuration;
        }
        animTime = Mathf.Clamp01(animTime);
        fx.noiseFactor = Mathf.Min(effectStrength * effectStrengthMultiplier + minEffect * animTime,maxEffect);
        if (fx.noiseFactor > 0f && !fx.enabled)
        {
            fx.enabled = true;
        }
        prevAmount = effectStrength;
    }

    private void UpdateEffect()
    {
        effectStrength = CalculateEffectStrength();
    }

    private float CalculateEffectStrength()
    {
        var plagueHeart = PlagueHeartBehavior.main;
        if (plagueHeart && Vector3.Distance(plagueHeart.transform.position, transform.position) < 10)
        {
            return 1f;
        }
        var closestFleshBlob = FleshBlobGravity.GetStrongest(transform.position);
        if (closestFleshBlob == null) return 0f;
        return Mathf.InverseLerp(maxDistance, minDistance, Vector3.Distance(closestFleshBlob.transform.position, transform.position) * closestFleshBlob.growth.Size);
    }
}