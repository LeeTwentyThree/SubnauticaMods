using System;
using System.Collections;
using Nautilus.Utility;
using UnityEngine;
using WorldHeightLib;

namespace TheRedPlague.Mono.CinematicEvents;

public class GargTeaserEvent : MonoBehaviour
{
    private GameObject _fleshBlobPrefab;

    private GameObject[] _fleshBlobs;

    private const int FleshBlobCount = 7;
    private const float SpawnRadius = 70;
    private const float FleshBlobSpawnDepth = 15f;
    private const float FleshBlobSpawnDuration = 3f;

    private Vector3 _meetPosition;
    
    public static void PlayCinematic()
    {
        new GameObject("GargTeaserEvent").AddComponent<GargTeaserEvent>();
    }
    
    private IEnumerator Start()
    {
        LoadFleshBlobPrefab();
        SpawnFleshBlobs();

        var timeEnd = Time.time + FleshBlobSpawnDuration;
        var raiseDelta = FleshBlobSpawnDepth / FleshBlobSpawnDuration;
        while (Time.time < timeEnd)
        {
            foreach (var blob in _fleshBlobs)
            {
                blob.transform.localPosition += new Vector3(0, raiseDelta * Time.deltaTime, 0);
            }
            yield return null;
        }
        foreach (var blob in _fleshBlobs)
        {
            blob.GetComponent<WalkerBehavior>().enabled = true;
        }

        yield break;
    }

    private void LoadFleshBlobPrefab()
    {
        _fleshBlobPrefab = Instantiate(Plugin.AssetBundle.LoadAsset<GameObject>("FleshBlobPrefab"));
        _fleshBlobPrefab.SetActive(false);
        MaterialUtils.ApplySNShaders(_fleshBlobPrefab);
        _fleshBlobPrefab.AddComponent<SkyApplier>().renderers = _fleshBlobPrefab.GetComponentsInChildren<Renderer>();
        var walker = _fleshBlobPrefab.AddComponent<WalkerBehavior>();
        walker.maxHeightUpdateDelay = 0.1f;
        walker.maxVerticalMoveSpeed = 1f;
        walker.horizontalMoveSpeed = 2;
        walker.upwardsNormalFactor = 1f;
        walker.depth = 3;
        walker.enabled = false;
        _fleshBlobPrefab.transform.localScale = Vector3.one * 3.5f;
    }

    private void SpawnFleshBlobs()
    {
        _fleshBlobs = new GameObject[FleshBlobCount];
        for (var i = 0; i < _fleshBlobs.Length; i++)
        {
            // Position
            var angle = (float)i / FleshBlobCount * Mathf.PI * 2f;
            var xPos = Mathf.Cos(angle) * SpawnRadius + _meetPosition.x;
            var zPos = Mathf.Sin(angle) * SpawnRadius + _meetPosition.z;
            if (!HeightMap.Instance.TryGetValueAtPosition(new Vector2(xPos, zPos), out var yPos))
                yPos = _meetPosition.y;
            yPos -= FleshBlobSpawnDepth;
            var pos = _meetPosition + new Vector3(xPos, yPos, zPos);
            
            // Normal
            if (!NormalMap.Instance.TryGetValueAtPosition(new Vector2(xPos, yPos), out var upDirection))
                upDirection = Vector3.up;
            
            var blob = Instantiate(_fleshBlobPrefab, pos, Quaternion.identity);
            blob.transform.up = upDirection;
            blob.SetActive(true);
            
            blob.GetComponent<WalkerBehavior>().SetTargetPosition(_meetPosition);
            
            _fleshBlobs[i] = blob;
        }
    }

    private void OnDestroy()
    {
        Destroy(_fleshBlobPrefab);
    }
}