using System.Collections.Generic;
using UnityEngine;

namespace TheRedPlague.Mono.FleshBlobs;

public class FleshBlobGravity : MonoBehaviour
{
    public static float maxDistance = 200f;
    public float gravitationalConstant = 200000;
    
    private static List<FleshBlobGravity> _targets = new List<FleshBlobGravity>();

    private void OnEnable()
    {
        _targets.Add(this);
    }
    
    private void OnDisable()
    {
        _targets.Remove(this);
    }

    public static FleshBlobGravity GetClosest(Vector3 pos)
    {
        var closestDist = maxDistance * maxDistance;
        FleshBlobGravity closestBlob = null;

        foreach (var target in _targets)
        {
            var dist = Vector3.SqrMagnitude(pos - target.transform.position);
            if (dist < closestDist)
            {
                closestDist = dist;
                closestBlob = target;
            }
        }

        return closestBlob;
    }
}