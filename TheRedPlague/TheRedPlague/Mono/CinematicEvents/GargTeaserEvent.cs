using System.Collections;
using Nautilus.Utility;
using TheRedPlague.Mono.VFX;
using UnityEngine;
using WorldHeightLib;

namespace TheRedPlague.Mono.CinematicEvents;

public class GargTeaserEvent : MonoBehaviour
{
    private GameObject _fleshBlobPrefab;

    private GameObject[] _fleshBlobs;

    // act 1
    private const int FleshBlobCount = 7;
    private const float SpawnRadius = 70;
    private const float FleshBlobSpawnDepth = 15f;
    private const float FleshBlobSpawnDuration = 3f;
    private const float MeetRadius = 3;
    
    // act 2
    private const float LeaderScale = 1f;
    private const float LeaderMovementSpeed = 10;

    private Vector3 _meetPosition;
    private Vector3 _targetPosition = new Vector3(1650, 0, 1122);

    private GameObject _leader;
    
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

        HeightMap.Instance.TryGetValueAtPosition(new Vector2(_meetPosition.x, _meetPosition.z),
            out var leaderSpawnPosY);
        _leader = Instantiate(Plugin.AssetBundle.LoadAsset<GameObject>("FleshBlobLeaderPrefab"), new Vector3(_meetPosition.x, leaderSpawnPosY, _meetPosition.z), Quaternion.identity);
        _leader.transform.localScale = Vector3.zero;
        MaterialUtils.ApplySNShaders(_leader);
        _leader.AddComponent<SkyApplier>().renderers = _leader.GetComponentsInChildren<Renderer>();

        var cubeRenderer = _leader.transform.Find("VFX/PlagueTornado").GetComponent<Renderer>();
        var cubeMaterial = new Material(MaterialUtils.IonCubeMaterial);
        cubeMaterial.SetColor(ShaderPropertyID._Color, Color.black);
        cubeMaterial.SetColor(ShaderPropertyID._SpecColor, Color.black);
        cubeMaterial.SetColor(ShaderPropertyID._SpecColor, Color.black);
        cubeMaterial.SetColor(ShaderPropertyID._GlowColor, Color.red);
        cubeMaterial.SetFloat(ShaderPropertyID._GlowStrength, 2.2f);
        cubeMaterial.SetFloat(ShaderPropertyID._GlowStrengthNight, 2.2f);
        cubeMaterial.SetColor("_DetailsColor", Color.red);
        cubeMaterial.SetColor("_SquaresColor", new Color(3, 2, 1));
        cubeMaterial.SetFloat("_SquaresTile", 7.5f);
        cubeMaterial.SetFloat("_SquaresSpeed", 8.8f);
        cubeMaterial.SetVector("_NoiseSpeed", new Vector4(0.5f, 0.3f, 0f));
        cubeMaterial.SetVector("_FakeSSSParams", new Vector4(0.2f, 1f, 1f));
        cubeMaterial.SetVector("_FakeSSSSpeed", new Vector4(0.5f, 0.5f, 1.37f));
        cubeRenderer.material = cubeMaterial;

        _leader.transform.Find("VFX/PlagueTornado-Blobs").gameObject.AddComponent<InfectAnything>();

        var scale = _leader.AddComponent<ScaleToSize>();
        scale.scaleSpeed = 0.1f;

        var mergeCount = GetMergedFleshBlobCountAndUpdateSizes();
        while (mergeCount < FleshBlobCount)
        {
            yield return new WaitForSeconds(0.1f);
            scale.SetNewSize(LeaderScale * mergeCount / FleshBlobCount / 3f);
            mergeCount = GetMergedFleshBlobCountAndUpdateSizes();
        }

        foreach (var blob in _fleshBlobs)
        {
            var blobScaler = blob.AddComponent<ScaleToSize>();
            blobScaler.scaleSpeed = 0.2f;
            blobScaler.SetNewSize(0);
            Destroy(blob, 6f);
        }

        scale.scaleSpeed = 0.05f;
        
        scale.SetNewSize(LeaderScale);

        var walk = _leader.AddComponent<WalkerBehavior>();
        walk.depth = 10;
        walk.horizontalMoveSpeed = LeaderMovementSpeed;
        walk.upwardsNormalFactor = 1;
        walk.maxVerticalMoveSpeed = 10;
        walk.rotateSpeed = 0.02f;
        
        walk.SetTargetPosition(_targetPosition);

        yield break;
    }

    private int GetMergedFleshBlobCountAndUpdateSizes()
    {
        int count = 0;
        foreach (var blob in _fleshBlobs)
        {
            if (blob == null)
            {
                // just do it anyway i guess
                count++;
            }
            var blobPosition = blob.transform.position;
            var sqrDist =
                (blobPosition.x - _meetPosition.x) * (blobPosition.x - _meetPosition.x) +
                (blobPosition.z - _meetPosition.z) * (blobPosition.z - _meetPosition.z);
            if (sqrDist < MeetRadius * MeetRadius)
            {
                count++;
                blob.GetComponent<ScaleToSize>().SetNewSize(0);
            }
        }

        return count;
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
        _fleshBlobPrefab.transform.localScale = Vector3.one * 4.5f;
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
            var rotation = Random.value * 360;
            blob.transform.Rotate(Vector3.up, rotation, Space.Self);
            blob.SetActive(true);

            var walker = blob.GetComponent<WalkerBehavior>();
            walker.SetTargetPosition(_meetPosition);
            walker.angleOffset = rotation;

            blob.transform.localScale *= Random.Range(0.9f, 1.1f);
            
            _fleshBlobs[i] = blob;
        }
    }

    private void OnDestroy()
    {
        Destroy(_fleshBlobPrefab);
    }
}