using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UWE;

namespace ModStructureHelperPlugin.Tools;

public class CableGeneration : MonoBehaviour
{
    private string _endPointAClassId;
    private string[] _middleClassIds;
    private string _endPointBClassId;
    
    // Key: Class ID, Value: Object list
    private readonly Dictionary<string, List<GameObject>> _objectPools = new();
    private readonly Dictionary<string, GameObject> _segmentPrefabs = new();
    
    public void SetUpPrefabClassIds(string endPointA, string[] middlePoints, string endPointB)
    {
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
}