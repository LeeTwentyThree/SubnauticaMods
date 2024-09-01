using System.Collections;
using Nautilus.Utility;
using TheRedPlague.Mono.PlagueGarg;
using TheRedPlague.Mono.VFX;
using UnityEngine;
using UWE;
using WorldHeightLib;

namespace TheRedPlague.Mono.CinematicEvents;

public class GargTeaserEvent : MonoBehaviour
{
    private GameObject _fleshBlobPrefab;

    private GameObject[] _fleshBlobs;

    // act 1
    private const int FleshBlobCount = 9;
    private const float SpawnRadius = 120;
    private const float FleshBlobSpawnDepth = 15f;
    private const float FleshBlobSpawnDuration = 3f;
    private const float FleshBlobScale = 4.5f;
    private const float MeetRadius = 11;
    
    // act 2
    private const float LeaderScale = 0.5f;
    private const float LeaderMovementSpeed = 10;
    private const int LeaderMistSpawns = 40;
    private const float LeaderMistRadius = 15;
    private const float LeaderMistInterval = 0.1f;
    
    // act 3
    private const float GargVelocity = 30f;
    private const float LeaderTrackOffset = 30f;

    private Vector3 _meetPosition = new Vector3(-1125, -350, 1129);
    private Vector3 _targetPosition = new Vector3(1650, 0, 1122);
    private Vector3 _gargSpawnPosition = new Vector3(1790, -360, 1514);
    private Vector3 _gargDespawnPosition = new Vector3(2000, -1500, 1600);

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
        scale.scaleSpeed = 0.05f;

        var mergeCount = GetMergedFleshBlobCountAndUpdateSizes();
        while (mergeCount < FleshBlobCount)
        {
            yield return new WaitForSeconds(0.1f);
            scale.SetNewSize(LeaderScale * mergeCount / FleshBlobCount);
            mergeCount = GetMergedFleshBlobCountAndUpdateSizes();
        }

        foreach (var blob in _fleshBlobs)
        {
            var blobScaler = blob.EnsureComponent<ScaleToSize>();
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
        walk.maxVerticalMoveSpeed = 25;
        walk.rotateSpeed = 1f;
        
        walk.SetTargetPosition(_targetPosition);

        while (Vector2.Distance(new Vector2(_leader.transform.position.x, _leader.transform.position.z),
                   new Vector2(_targetPosition.x, _targetPosition.z)) > 1)
        {
            yield return new WaitForSeconds(0.5f);
        }

        yield return new WaitForSeconds(4f);

        StartCoroutine(SpawnLeaderBlood());
        
        yield return new WaitForSeconds(10f);
        
        var gargRequest = PrefabDatabase.GetPrefabAsync("UninfectedGarg");
        yield return gargRequest;
        gargRequest.TryGetPrefab(out var gargPrefab);
        var garg = Instantiate(gargPrefab, _gargSpawnPosition, Quaternion.LookRotation(Vector3.up));
        var swimToPos = garg.AddComponent<ForceCreatureToSwimToPoint>();
        swimToPos.position = _leader.transform.position + new Vector3(0, LeaderTrackOffset, 0);
        swimToPos.swimVelocity = GargVelocity;

        while (Vector3.Distance(_leader.transform.position + new Vector3(0, LeaderTrackOffset, 0), garg.transform.position) > 60)
        {
            yield return new WaitForSeconds(0.1f);
        }

        var gargAnimator = garg.GetComponentInChildren<Animator>();
        gargAnimator.SetTrigger("cin_deathroll");
        yield return new WaitForSeconds(0.3f);

        scale.scaleSpeed = 0.1f;
        scale.SetNewSize(0);
        
        // DestroyImmediate(_leader.GetComponent<WalkerBehavior>());
        // _leader.transform.parent = gargAnimator.transform.Find("Armature/Head");
        // _leader.transform.localPosition = Vector3.zero;

        yield return new WaitForSeconds(2f);

        swimToPos.position = _gargDespawnPosition;
        
        while (Vector3.Distance(garg.transform.position, _gargDespawnPosition) > 100)
        {
            yield return new WaitForSeconds(0.1f);
        }
        
        Destroy(garg);
        Destroy(gameObject);
    }

    private IEnumerator SpawnLeaderBlood()
    {
        var gasopodTask = CraftData.GetPrefabForTechTypeAsync(TechType.Gasopod);
        yield return gasopodTask;
        var gasopodGas = gasopodTask.GetResult().GetComponent<GasoPod>().gasFXprefab;
        var mistPrefab = Instantiate(gasopodGas);
        mistPrefab.SetActive(false);
        foreach (var renderer in mistPrefab.GetComponentsInChildren<Renderer>(true))
        {
            renderer.material.color = new Color(0.5f, 0.1f, 0.1f, 0.6f);
            renderer.material.SetColor("_ColorStrengthAtNight", Color.gray);
        }

        foreach (var ps in mistPrefab.GetComponentsInChildren<ParticleSystem>(true))
        {
            var main = ps.main;
            main.startSizeMultiplier *= 15f;
            main.startLifetimeMultiplier *= 10f;
            var sizeOverLifetime = ps.sizeOverLifetime;
            sizeOverLifetime.size = new ParticleSystem.MinMaxCurve(1f,
                new AnimationCurve(new(0, 0.3f), new(1, 1)));
        }

        foreach (var trail in mistPrefab.GetComponentsInChildren<Trail_v2>(true))
        {
            trail.gameObject.SetActive(false);
        }

        var destroyAfterSeconds = mistPrefab.GetComponent<VFXDestroyAfterSeconds>();
        destroyAfterSeconds.lifeTime = 20f;
        
        mistPrefab.transform.Find("xGasopodSmoke/xSmkMesh").gameObject.SetActive(false);

        for (int i = 0; i < LeaderMistSpawns; i++)
        {
            var mist = Instantiate(mistPrefab, _leader.transform.position + Random.insideUnitSphere * LeaderMistRadius,
                Quaternion.identity);
            mist.SetActive(true);
            mist.AddComponent<ConstantMotion>().motionPerSecond = new Vector3(0, 6, 0);
            yield return new WaitForSeconds(LeaderMistInterval);
        }
        
        Destroy(mistPrefab);
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
                continue;
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
        walker.maxVerticalMoveSpeed = 10f;
        walker.horizontalMoveSpeed = 2;
        walker.upwardsNormalFactor = 0.8f;
        walker.depth = 3;
        walker.enabled = false;
        _fleshBlobPrefab.transform.localScale = Vector3.one * FleshBlobScale;
        var scaleToSize = _fleshBlobPrefab.AddComponent<ScaleToSize>();
        scaleToSize.scaleSpeed = 0.1f;
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
            var pos = new Vector3(xPos, yPos, zPos);
            
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
        Destroy(_leader);
    }
}