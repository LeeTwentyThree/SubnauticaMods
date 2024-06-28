using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UWE;

namespace ModStructureHelperPlugin.CableGeneration;

public class CableBuilder : MonoBehaviour
{
    private string _endPointAClassId;
    private string[] _middleClassIds;
    private string _endPointBClassId;
    private Bezier3D _bezierCurve = new Bezier3D();
    
    // Key: Class ID, Value: Object list
    private readonly Dictionary<string, List<GameObject>> _objectPools = new();
    private readonly Dictionary<string, GameObject> _segmentPrefabs = new();
    
    public void SetUpPrefabClassIds(string endPointA, string[] middlePoints, string endPointB)
    {
        StopAllCoroutines();
        ClearAllObjects();
        _endPointAClassId = endPointA;
        _middleClassIds = middlePoints;
        _endPointBClassId = endPointB;
        StartCoroutine(LoadSegmentPrefab(endPointA));
        middlePoints.ForEach(id => StartCoroutine(LoadSegmentPrefab(id)));
        StartCoroutine(LoadSegmentPrefab(endPointB));
    }
    
    private IEnumerator LoadSegmentPrefab(string classId)
    {
        var request = PrefabDatabase.GetPrefabAsync(classId);
        yield return request;
        if (!request.TryGetPrefab(out var prefab))
        {
            ErrorMessage.AddMessage($"Failed to load prefab by Class ID {classId}");
            yield break;
        }
        _segmentPrefabs.Add(classId, prefab);
    }

    private void ClearAllObjects()
    {
        _segmentPrefabs.Clear();
        foreach (var pool in _objectPools)
        {
            foreach (var objectList in pool.Value)
            {
                Destroy(objectList);
            }
        }
        _objectPools.Clear();
    }

    public void Build()
    {
        var length = _bezierCurve.GetCurveLength();
        for (float l = 0; l < length; l += 0.5f)
        {
            var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.position = _bezierCurve.GetPosition(l / length);
            cube.transform.forward = _bezierCurve.GetDirection(l / length);
        }
    }
}