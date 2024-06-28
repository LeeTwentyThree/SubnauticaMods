using System.Linq;
using UnityEngine;

namespace ModStructureHelperPlugin.CableGeneration;

public class Bezier3D
{
    public Vector3[] ControlPoints { get; set; }

    public Vector3 GetPosition(float t)
    {
        return new Vector3(GetBezierComponent(t, 0), GetBezierComponent(t, 1), GetBezierComponent(t, 2));
    }
    
    public Vector3 GetDirection(float t)
    {
        return (GetPosition(t + 0.01f) - GetPosition(t - 0.01f)).normalized;
    }

    private float GetBezierComponent(float t, int dimension)
    {
        var component = 0f;
        for (var i = 0; i < ControlPoints.Length; i++)
        {
            component += ControlPoints[i][dimension] * Mathf.Pow(1 - t, ControlPoints.Length - i) * Mathf.Pow(t, i);
        }

        return component;
    }

    public float GetCurveLength(int quality = 500)
    {
        var distance = 0f;
        var lastPosition = GetPosition(0f);
        
        for (var t = 1f / quality; t <= 1f; t += 1f / quality)
        {
            var pos = GetPosition(t);
            distance += Vector3.Distance(lastPosition, pos);
            lastPosition = pos;
        }

        return distance;
    }
}