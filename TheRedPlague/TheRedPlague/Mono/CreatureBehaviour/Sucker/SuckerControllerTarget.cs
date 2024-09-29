using System.Collections.Generic;
using UnityEngine;

namespace TheRedPlague.Mono.CreatureBehaviour.Sucker;

public class SuckerControllerTarget : MonoBehaviour
{
    private static List<SuckerControllerTarget> _all = new();
    
    private void OnEnable()
    {
        _all.Add(this);
    }

    private void OnDisable()
    {
        _all.Remove(this);
    }

    public static bool TryGetClosest(out SuckerControllerTarget result, Vector3 referencePos, float maxRange)
    {
        result = null;
        
        if (_all.Count == 0) return false;
        
        var closestSqrDistance = float.MaxValue;
        foreach (var target in _all)
        {
            var sqrDist = Vector3.SqrMagnitude(target.transform.position - referencePos);
            if (sqrDist > closestSqrDistance) continue;
            if (sqrDist > maxRange * maxRange) continue;
            closestSqrDistance = sqrDist;
            result = target;
        }

        return result != null;
    }
}