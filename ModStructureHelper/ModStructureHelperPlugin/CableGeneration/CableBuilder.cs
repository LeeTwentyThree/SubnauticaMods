using System;
using System.Collections;
using System.Collections.Generic;
using ModStructureHelperPlugin.Tools;
using UnityEngine;
using UWE;
using Random = UnityEngine.Random;

namespace ModStructureHelperPlugin.CableGeneration;

public class CableBuilder : MonoBehaviour
{
    private string _endPointAClassId;
    private string[] _middleClassIds = new string[]
    {
        "69cd7462-7cd2-456c-bfff-50903c391737",
        "94933bb3-0587-4e8d-a38d-b7ec4c859b1a",
        "31f84eba-d435-438c-a58e-f3f7bae8bfbd"
    };
    private string _endPointBClassId;
    public float Scale { get; set; }
    public float Spacing { get; set; }
    
    [SerializeField] Bezier3D _bezierCurve = new Bezier3D();
    
    // Key: Class ID, Value: Object list
    private readonly Dictionary<CableLocation, ObjectPool> _objectPools = new()
    {
        { CableLocation.Start, new ObjectPool() },
        { CableLocation.Middle, new ObjectPool() },
        { CableLocation.End, new ObjectPool() }
    };
    
    private readonly Dictionary<CableLocation, List<GameObject>> _segmentPrefabs = new();

    private readonly List<Transform> _controlPoints = new();

    private bool _cableActive;
    
    public void GenerateNewCable(string endPointA, string[] middlePoints, string endPointB)
    {
        _endPointAClassId = endPointA;
        _middleClassIds = middlePoints;
        _endPointBClassId = endPointB;

        _controlPoints.ForEach(controlPoint => Destroy(controlPoint.gameObject));
        _controlPoints.Clear();
        
        AddControlPoint();
        AddControlPoint();
        
        StartCoroutine(GenerateCablesCoroutine());
    }

    private IEnumerator GenerateCablesCoroutine()
    {
        foreach (var segmentPrefab in _segmentPrefabs)
        {
            segmentPrefab.Value?.ForEach(Destroy);
        }
        _segmentPrefabs.Clear();
        
        yield return StartCoroutine(LoadSegmentPrefab(_endPointAClassId, CableLocation.Start));
        foreach (var point in _middleClassIds)
        {
            yield return StartCoroutine(LoadSegmentPrefab(point, CableLocation.Middle));
        }
        yield return StartCoroutine(LoadSegmentPrefab(_endPointBClassId, CableLocation.End));
        
        foreach (var prefabGroup in _segmentPrefabs)
        {
            _objectPools[prefabGroup.Key].SetPrefabs(prefabGroup.Value.ToArray());
        }

        _cableActive = true;
    }

    public void AddControlPoint()
    {
        var controlPoint = Instantiate(Plugin.AssetBundle.LoadAsset<GameObject>("CableHandlePrefab"));
        var cameraTransform = MainCamera.camera.transform;
        
        controlPoint.transform.position = cameraTransform.position +
                                          cameraTransform.forward * 5 +
                                          Random.insideUnitSphere * 3;

        controlPoint.gameObject.AddComponent<TransformableObject>();
        
        if (_controlPoints.Count <= 1)
            _controlPoints.Add(controlPoint.transform);
        else _controlPoints.Insert(_controlPoints.Count - 2, controlPoint.transform);

        _bezierCurve.ControlPoints = _controlPoints.ToArray();
    }

    public void RemoveControlPoint()
    {
        if (_controlPoints.Count <= 2)
        {
            ErrorMessage.AddMessage("Cannot remove any more control points!");
            return;
        }

        var removeIndex = _controlPoints.Count - 2;
        Destroy(_controlPoints[removeIndex].gameObject);
        _controlPoints.RemoveAt(removeIndex);
        
        _bezierCurve.ControlPoints = _controlPoints.ToArray();
    }
    
    private IEnumerator LoadSegmentPrefab(string classId, CableLocation location)
    {
        var request = PrefabDatabase.GetPrefabAsync(classId);
        yield return request;
        if (!request.TryGetPrefab(out var prefab))
        {
            ErrorMessage.AddMessage($"Failed to load prefab by Class ID {classId}");
            yield break;
        }

        var strippedPrefab = Instantiate(prefab);
        Destroy(strippedPrefab.GetComponent<PrefabIdentifier>());
        Destroy(strippedPrefab.GetComponent<LargeWorldEntity>());
        strippedPrefab.GetComponentsInChildren<Collider>().ForEach(c => c.enabled = false);
        strippedPrefab.GetComponentsInChildren<Renderer>().ForEach(r => r.SetFadeAmount(0.5f));
        strippedPrefab.SetActive(false);
        if (!_segmentPrefabs.TryGetValue(location, out var objectList))
        {
            objectList = new List<GameObject>();
            _segmentPrefabs.Add(location, objectList);
        }
        objectList.Add(strippedPrefab);
    }

    private static int ApproximateTimeMapQualityBasedOnLength(float length)
    {
        return Mathf.Clamp(Mathf.RoundToInt(length * 3), 10, 5000);
    }

    private void Update()
    {
        if (!_cableActive) return;
        Build();
        foreach (var point in _controlPoints)
            point.localScale = Vector3.one * Scale;
    }

    public void DeleteCable()
    {
        _cableActive = false;
        CleanUp();
    }

    public void SaveCable()
    {
        ErrorMessage.AddMessage("Failed to save cable!!!");
        DeleteCable();
    }

    public void Build()
    {
        var length = _bezierCurve.GetCurveLength(1000);
        var arcLengthMapSize = ApproximateTimeMapQualityBasedOnLength(length);
        var arcLengths = new(float,float)[arcLengthMapSize];
        _bezierCurve.CalculateArcLengthTimeMap(arcLengths, 500);
        
        _objectPools.ForEach(pool => pool.Value.DisableAllObjects());

        SpawnCableSegmentAtPoint(CableLocation.Start, 0);
        
        var l = 0f;
        while (l < length)
        {
            var lowIndex = 0;
            for (var i = 0; i < arcLengths.Length; i++)
            {
                if (arcLengths[i].Item1 >= l)
                {
                    lowIndex = i;
                    break;
                }
            }
            
            var highIndex = lowIndex + 1;

            if (lowIndex >= arcLengths.Length || highIndex >= arcLengths.Length)
            {
                break;
            }

            var t = (arcLengths[highIndex].Item1 - l) * arcLengths[lowIndex].Item2 + (1f - (arcLengths[highIndex].Item1 - l)) * arcLengths[highIndex].Item2;

            SpawnCableSegmentAtPoint(CableLocation.Middle, t);

            l += Spacing;
        }
        
        SpawnCableSegmentAtPoint(CableLocation.End, _bezierCurve.GetPosition(1), -_bezierCurve.GetApproximateDirection(1));
    }

    private void SpawnCableSegmentAtPoint(CableLocation location, float t)
    {
        SpawnCableSegmentAtPoint(location, _bezierCurve.GetPosition(t), _bezierCurve.GetApproximateDirection(t));
    }
    
    private void SpawnCableSegmentAtPoint(CableLocation location, Vector3 pos, Vector3 forward)
    {
        var pool = _objectPools[location];
        var obj = pool.RequestObject();
        obj.transform.position = pos;
        obj.transform.right = forward;
        obj.transform.localScale = Vector3.one * Scale;
    }

    private void OnDestroy() => CleanUp();
    
    private void CleanUp()
    {
        foreach (var prefab in _segmentPrefabs)
        {
            prefab.Value?.ForEach(Destroy);
        }
        _segmentPrefabs.Clear();

        foreach (var pool in _objectPools)
        {
            pool.Value.ClearPool();
        }

        foreach (var controlPoint in _controlPoints)
        {
            Destroy(controlPoint.gameObject);
        }
        _controlPoints.Clear();
    }

    private class ObjectPool
    {
        private List<GameObject>[] _objects;
        private GameObject[] _prefabs;
        private int[] _objectIndex;
        private int _prefabIndex;

        public void ClearPool()
        {
            foreach (var list in _objects)
            {
                foreach (var obj in list)
                {
                    Destroy(obj);
                }
                list.Clear();
            }
            _objectIndex = new int[_prefabs.Length];
            _prefabIndex = 0;
        }

        public void SetPrefabs(GameObject[] prefabs)
        {
            _objects = new List<GameObject>[prefabs.Length];
            _prefabs = prefabs;
            for (var i = 0; i < prefabs.Length; i++)
            {
                _objects[i] = new List<GameObject>();
            }
            ClearPool();
        }

        public void DisableAllObjects()
        {
            foreach (var list in _objects)
            {
                foreach (var obj in list)
                {
                    obj.SetActive(false);
                }
            }
            _objectIndex = new int[_prefabs.Length];
            _prefabIndex = 0;
        }

        public GameObject RequestObject()
        {
            GameObject result;
            var prefabToUse = GetPrefabToUse();
            if (_objectIndex[prefabToUse] >= _objects[prefabToUse].Count)
            {
                var prefab = _prefabs[prefabToUse];
                var obj = Instantiate(prefab);
                _objects[prefabToUse].Add(obj);
                result = obj;
            }
            else
            {
                result = _objects[prefabToUse][_objectIndex[prefabToUse]];
            }

            _objectIndex[prefabToUse]++;
            result.SetActive(true);
            return result;
        }
        
        private int GetPrefabToUse()
        {
            if (_prefabs.Length == 1) return 0;
            var result = _prefabIndex;
            _prefabIndex++;
            if (_prefabIndex >= _prefabs.Length) _prefabIndex = 0;
            return result;
        }
    }
}