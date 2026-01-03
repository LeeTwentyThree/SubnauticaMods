using System;
using System.Collections;
using System.Collections.Generic;
using ModStructureHelperPlugin.Editing.Managers;
using ModStructureHelperPlugin.Editing.Tools;
using ModStructureHelperPlugin.StructureHandling;
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
    [SerializeField] float previewUpdateRate = 0.1f;
    
    // Key: Class ID, Value: Object list
    private readonly Dictionary<CableLocation, ObjectPool> _objectPools = new()
    {
        { CableLocation.Start, new ObjectPool() },
        { CableLocation.Middle, new ObjectPool() },
        { CableLocation.End, new ObjectPool() }
    };
    
    private readonly Dictionary<CableLocation, List<SegmentPrefab>> _segmentPrefabs = new();

    private readonly List<Transform> _controlPoints = new();

    private Vector3[] _previousControlPointPositions = Array.Empty<Vector3>();

    private bool _cableActive;

    private bool _flipStart;
    private bool _flipEnd;
    private float _timeUpdatePreviewAgain;
    
    public void GenerateNewCable(string endPointA, string[] middlePoints, string endPointB)
    {
        if (StructureInstance.Main == null)
        {
            ErrorMessage.AddMessage("Cannot create a cable while not editing any structure!");
            return;
        }

        if (_cableActive)
        {
            ErrorMessage.AddMessage("Please save or remove the current cable before creating a new one!");
            return;
        }
        
        _endPointAClassId = endPointA;
        _middleClassIds = middlePoints;
        _endPointBClassId = endPointB;

        _flipStart = _endPointAClassId == "a0a9237e-dee3-4efa-81ff-fea3893a6eb7";
        _flipEnd = _endPointBClassId == "a0a9237e-dee3-4efa-81ff-fea3893a6eb7";

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
            segmentPrefab.Value?.ForEach(prefab => Destroy(prefab.Object));
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
        else _controlPoints.Insert(_controlPoints.Count - 1, controlPoint.transform);

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

    private void Awake()
    {
        StructureInstance.OnStructureInstanceChanged += OnStructureInstanceChanged;
    }

    private bool ShouldUpdatePreview()
    {
        if (Time.time < _timeUpdatePreviewAgain)
            return false;
        
        _timeUpdatePreviewAgain = Time.time + previewUpdateRate;
        return GetControlPointPositionsChanged();
    }

    private bool GetControlPointPositionsChanged()
    {
        int count = _controlPoints.Count;
        if (_previousControlPointPositions.Length != count)
            return true;

        for (int i = 0; i < count; i++)
        {
            if (_controlPoints[i].position != _previousControlPointPositions[i])
                return true;
        }

        return false;
    }

    private void OnStructureInstanceChanged(StructureInstance instance)
    {
        if (instance == null)
        {
            DeleteCable();
        }
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
            objectList = new List<SegmentPrefab>();
            _segmentPrefabs.Add(location, objectList);
        }
        objectList.Add(new SegmentPrefab(strippedPrefab, classId));
    }

    private static int ApproximateTimeMapQualityBasedOnLength(float length)
    {
        return Mathf.Clamp(Mathf.RoundToInt(length * 3), 10, 5000);
    }

    private void Update()
    {
        if (!_cableActive) return;
        if (ShouldUpdatePreview())
            Build(false);
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
        Build(true);
        DeleteCable();
    }

    public void Build(bool instantiateIntoStructure)
    {
        var length = _bezierCurve.GetCurveLength(instantiateIntoStructure ? 10000 : 500);
        var arcLengthMapSize = ApproximateTimeMapQualityBasedOnLength(length) * (instantiateIntoStructure ? 5 : 1);
        var arcLengths = new(float,float)[arcLengthMapSize];
        _bezierCurve.CalculateArcLengthTimeMap(arcLengths, instantiateIntoStructure ? 5000 : 400);
        
        _objectPools.ForEach(pool => pool.Value.DisableAllObjects());

        SpawnCableSegmentAtPoint(CableLocation.Start, 0, instantiateIntoStructure);
        
        var l = 0f;
        while (l < length)
        {
            var lowIndex = BinarySearch(arcLengths, l);
            
            var highIndex = lowIndex + 1;

            if (lowIndex >= arcLengths.Length || highIndex >= arcLengths.Length)
            {
                break;
            }

            var t = (arcLengths[highIndex].Item1 - l) * arcLengths[lowIndex].Item2 + (1f - (arcLengths[highIndex].Item1 - l)) * arcLengths[highIndex].Item2;

            SpawnCableSegmentAtPoint(CableLocation.Middle, t, instantiateIntoStructure);

            l += Spacing;
        }
        
        SpawnCableSegmentAtPoint(CableLocation.End, _bezierCurve.GetPosition(1), -_bezierCurve.GetApproximateDirection(1), instantiateIntoStructure);
    }
    
    private static int BinarySearch((float, float)[] inputArray, float key)
    {
        var min = 0;
        var max = inputArray.Length - 1;
        while (min <= max)
        {
            var mid = (min + max) / 2;
            if (Mathf.Abs(key - inputArray[mid].Item1) < 0.01f)
            {
                return ++mid;
            }
            else if (key < inputArray[mid].Item1)
            {
                max = mid - 1;
            }
            else
            {
                min = mid + 1;
            }
        }

        return (min + max) / 2;
    }

    private void SpawnCableSegmentAtPoint(CableLocation location, float t, bool instantiateIntoStructure)
    {
        SpawnCableSegmentAtPoint(location, _bezierCurve.GetPosition(t), _bezierCurve.GetApproximateDirection(t), instantiateIntoStructure);
    }
    
    private void SpawnCableSegmentAtPoint(CableLocation location, Vector3 pos, Vector3 forward, bool instantiateIntoStructure)
    {
        var right = location switch
        {
            CableLocation.Start => _flipStart ? -forward : forward,
            CableLocation.End => _flipEnd ? -forward : forward,
            _ => forward
        };
        var scale = Vector3.one * Scale;
        var pool = _objectPools[location];
        if (instantiateIntoStructure)
        {
            var classId = pool.RequestObjectIdWithoutInstantiation();
            CoroutineHost.StartCoroutine(SpawnCablePrefabInWorldForStructure(classId, pos, right, scale));
            return;
        }
        var obj = pool.RequestObject();
        obj.transform.position = pos;
        obj.transform.right = right;
        obj.transform.localScale = scale;
    }

    private static IEnumerator SpawnCablePrefabInWorldForStructure(string classId, Vector3 pos, Vector3 right, Vector3 scale)
    {
        var request = PrefabDatabase.GetPrefabAsync(classId);
        yield return request;
        if (!request.TryGetPrefab(out var prefab))
        {
            ErrorMessage.AddMessage($"Failed to load prefab by ID '{classId}'!");
            yield break;
        }

        var obj = Instantiate(prefab);
        obj.transform.position = pos;
        obj.transform.right = right;
        obj.transform.localScale = scale;

        var prefabIdentifer = obj.GetComponent<PrefabIdentifier>();
        StructureInstance.Main.RegisterNewEntity(prefabIdentifer, true);
    }

    private void OnDestroy()
    {
        StructureInstance.OnStructureInstanceChanged -= OnStructureInstanceChanged;
        CleanUp();
    }

    private void CleanUp()
    {
        foreach (var segmentPrefab in _segmentPrefabs)
        {
            segmentPrefab.Value?.ForEach(prefab => Destroy(prefab.Object));
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
        private SegmentPrefab[] _prefabs;
        private int[] _objectIndex;
        private int _prefabIndex;

        public void ClearPool()
        {
            if (_objects != null)
            {
                foreach (var list in _objects)
                {
                    if (list == null) continue;
                    foreach (var obj in list)
                    {
                        Destroy(obj);
                    }

                    list.Clear();
                }   
            }

            if (_prefabs != null)
                _objectIndex = new int[_prefabs.Length];
            else _objectIndex = new int[0];
            _prefabIndex = 0;
        }

        public void SetPrefabs(SegmentPrefab[] prefabs)
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
                var obj = Instantiate(prefab.Object);
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

        public string RequestObjectIdWithoutInstantiation()
        {
            var prefabToUse = GetPrefabToUse();
            return _prefabs[prefabToUse].PrefabClassId;
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

    private readonly struct SegmentPrefab
    {
        public GameObject Object { get; }
        public string PrefabClassId { get; }

        public SegmentPrefab(GameObject obj, string prefabClassId)
        {
            Object = obj;
            PrefabClassId = prefabClassId;
        }
    }
}