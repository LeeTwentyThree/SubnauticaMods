using UnityEngine;

namespace ModStructureHelperPlugin.CableGeneration;

[System.Serializable]
public class Bezier3D
{
    public Transform[] ControlPoints { get; set; }

    public Vector3 GetPosition(float t)
    {
        return new Vector3(GetBezierComponent(t, 0), GetBezierComponent(t, 1), GetBezierComponent(t, 2));
    }

    public Vector3 GetFirstDerivative(float t)
    {
        return new Vector3(GetFirstDerivativeBezierComponent(t, 0), GetFirstDerivativeBezierComponent(t, 1), GetFirstDerivativeBezierComponent(t, 2));
    }
    
    public Vector3 GetDirection(float t)
    {
        return GetFirstDerivative(t).normalized;
    }
    
    public Vector3 GetApproximateDirection(float t)
    {
        return (GetPosition(t + 0.0001f) - GetPosition(t - 0.0001f)).normalized;
    }
    
    public float GetCurveLength(int quality = 500, float endTime = 1f)
    {
        var distance = 0f;
        var lastPosition = GetPosition(0f);
        
        for (var t = 1f / quality; t <= endTime; t += 1f / quality)
        {
            var pos = GetPosition(t);
            distance += Vector3.Distance(lastPosition, pos);
            lastPosition = pos;
        }

        return distance;
    }

    // Length : time
    public void CalculateArcLengthTimeMap((float,float)[] array, int quality)
    {
        for (var i = 0; i < array.Length; i++)
        {
            var t = (float) i / array.Length;
            array[i] = (GetCurveLength(quality, t), t);
        }
    }

    private float GetBezierComponent(float t, int dimension)
    {
        var component = 0f;
        var n = ControlPoints.Length - 1;
        for (var i = 0; i < ControlPoints.Length; i++)
        {
            var factor = (float) Factorial(n) / (Factorial(i) * Factorial(n - i));
            component += factor * Mathf.Pow(1 - t, n - i) * Mathf.Pow(t, i) * ControlPoints[i].position[dimension];
        }

        return component;
    }
    
    private float GetFirstDerivativeBezierComponent(float t, int dimension)
    {
        var component = 0f;
        var n = ControlPoints.Length - 1;
        for (var i = 0; i < ControlPoints.Length - 1; i++)
        {
            var factor = (float) Factorial(n - 1) / (Factorial(i) * Factorial(n - 1 - i));
            component += factor * Mathf.Pow(1 - t, n - 1 - i) * Mathf.Pow(t, i) * n * (ControlPoints[i + 1].position[dimension] - ControlPoints[i].position[dimension]);
        }

        return component;
    }

    private static int Factorial(int n)
    {
        var f = 1;
        while (n > 0)
        {
            f *= n;
            n--;
        }

        return f;
    }
}