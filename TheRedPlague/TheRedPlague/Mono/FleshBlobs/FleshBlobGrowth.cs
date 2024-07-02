using System;
using System.Collections.Generic;
using Nautilus.Handlers;
using Nautilus.Json;
using UnityEngine;

namespace TheRedPlague.Mono.FleshBlobs;

public class FleshBlobGrowth : MonoBehaviour
{
    private static FleshBlobGrowthData _growthData = SaveDataHandler.RegisterSaveDataCache<FleshBlobGrowthData>();

    private PrefabIdentifier _identifier;

    private string Id => _identifier.Id;
    public float Size => _currentSize;

    private float _startSize = 0.15f;
    private float _finalSize = 1f;
    
    private float _currentSize;
    private float _targetSize;

    private float _growthRate = 0.03f;

    private static List<FleshBlobGrowth> _registry = new List<FleshBlobGrowth>();

    private void OnEnable()
    {
        _registry.Add(this);
    }

    private void OnDisable()
    {
        _registry.Remove(this);
    }

    public void Grow(float newSize)
    {
        _targetSize = newSize;
        _growthData.SizeDataSafe[Id] = newSize;
        UpdateScale();
    }

    private void Start()
    {
        if (_growthData.sizeData == null) _growthData.Load();
        _identifier = GetComponent<PrefabIdentifier>();
        // hacky solution but it works
        if (_identifier.ClassId[_identifier.ClassId.Length - 1] == '7')
        {
            _startSize = 0.4f;
        }
        var sizeData = _growthData.SizeDataSafe;
        if (sizeData.TryGetValue(Id, out var size))
        {
            // ErrorMessage.AddMessage($"{Id} is using loaded size of {size}");
            _currentSize = size;
            _targetSize = size;
            UpdateScale();
        }
        else
        {
            // ErrorMessage.AddMessage($"No save data found for {Id}!");
            _currentSize = _startSize;
            Grow(_startSize);
        }

    }

    private void Update()
    {
        if (Math.Abs(_currentSize - _targetSize) >= 0.001f)
        {
            _currentSize = Mathf.MoveTowards(_currentSize, _targetSize, Time.deltaTime * _growthRate);
            UpdateScale();
        }
    }

    private void UpdateScale()
    {
        transform.localScale = Vector3.one * _currentSize;
    }

    public static void GrowAllInRange(Vector3 position, float range, float growthPercent)
    {
        foreach (var blob in _registry)
        {
            if (blob == null) continue;
            if (Vector3.Distance(blob.transform.position, position) > range) continue;
            blob.Grow(Mathf.Clamp(blob._targetSize + growthPercent, blob._startSize, blob._finalSize));
        }
    }

    public static FleshBlobGrowth GetClosestForDroneStrike(Vector3 dronePosition, float maxDistance)
    {
        var minDist = maxDistance * maxDistance;
        FleshBlobGrowth closest = null;
        
        foreach (var blob in _registry)
        {
            if (blob == null) continue;
            var dist = Vector2.SqrMagnitude(new Vector2(blob.transform.position.x, blob.transform.position.z) - new Vector2(dronePosition.x, dronePosition.z));
            if (dist < minDist)
            {
                minDist = dist;
                closest = blob;
            }
        }

        return closest;
    }
}

public class FleshBlobGrowthData : SaveDataCache
{
    public Dictionary<string, float> sizeData;
    
    public Dictionary<string, float> SizeDataSafe
    {
        get
        {
            if (sizeData == null)
            {
                sizeData = new Dictionary<string, float>();
            }

            return sizeData;
        }
    }
}