using System.Collections.Generic;
using Nautilus.Utility;
using UnityEngine;

namespace TheRedPlague.Mono;

public class SuckerDamageable : MonoBehaviour
{
    private static readonly List<SuckerDamageable> Suckers = new();

    private static readonly FMODAsset DeathSound = AudioUtils.GetFmodAsset("SuckerDeath");
    
    public static bool DamageSuckersInRange(Vector3 center, float range)
    {
        var damaged = false;
        
        foreach (var sucker in Suckers)
        {
            if (!sucker) continue;
            if (Vector3.Distance(center, sucker.transform.position) > range) continue;
            sucker.Kill();
            damaged = true;
        }

        return damaged;
    }

    private void OnEnable()
    {
        Suckers.Add(this);
    }
    
    private void OnDisable()
    {
        Suckers.Remove(this);
    }

    public void Kill()
    {
        var rb = GetComponent<Rigidbody>();
        if (rb) rb.isKinematic = false;
        transform.Find("BlockTriggers").gameObject.SetActive(false);
        Utils.PlayFMODAsset(DeathSound, transform.position);
    }
}