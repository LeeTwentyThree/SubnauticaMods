using System.Collections.Generic;
using UnityEngine;

namespace TheRedPlague.Mono.CreatureBehaviour.MrTeeth;

public class MrTeethReturnPoint : MonoBehaviour
{
    private static readonly List<MrTeethReturnPoint> ReturnPoints = new();

    private void OnEnable()
    {
        ReturnPoints.Add(this);
    }

    private void OnDisable()
    {
        ReturnPoints.Remove(this);
    }

    public static bool TryGetClosest(Vector3 mrTeethPosition, out Vector3 returnPosition)
    {
        var closest = float.MaxValue;
        var success = false;
        returnPosition = default;
        foreach (var returnPoint in ReturnPoints)
        {
            var distSqr = (returnPoint.transform.position - mrTeethPosition).sqrMagnitude;
            if (distSqr < closest)
            {
                returnPosition = returnPoint.transform.position;
                closest = distSqr;
                success = true;
            }
        }

        return success;
    }
}