using System;
using System.Collections;
using Nautilus.Utility;
using Story;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TheRedPlague.Mono;

// har har har har har har har har  har har har har har har har
public class JumpScares : MonoBehaviour, IStoryGoalListener
{
    private float _minDelay = 300;
    private float _maxDelay = 900;
    private float _radius = 40;
    private float _timeSpawnAgain;

    private static FMODAsset jumpscareSound = AudioUtils.GetFmodAsset("WarperJumpscare");

    private string[] _corpseClassIDs = new string[] {"InfectedCorpse", "SkeletonCorpse"};

    public static JumpScares main;

    private void Awake()
    {
        main = this;
    }

    public void JumpScareNow()
    {
        JumpScare();
    }

    private IEnumerator Start()
    {
        yield return new WaitUntil(() =>
            StoryGoalManager.main.completedGoals != null && StoryGoalManager.main.completedGoals.Count > 0);
        if (StoryGoalManager.main.IsGoalComplete(StoryUtils.ForceFieldLaserDisabled.key))
        {
            enabled = false;
            yield break;
        }
        StoryGoalManager.main.AddListener(this);
    }

    private GameObject Spawn()
    {
        var model = Instantiate(Plugin.AssetBundle.LoadAsset<GameObject>("SharkJumpscare"));
        MaterialUtils.ApplySNShaders(model);
        var despawn = model.AddComponent<DespawnWhenOffScreen>();
        despawn.despawnIfTooClose = true;
        despawn.minDistance = 4;
        despawn.waitUntilSeen = true;
        despawn.moveInstead = true;
        despawn.moveRadius = 35f;
        despawn.disappearWhenLookedAtForTooLong = true;
        despawn.jumpscareWhenTooClose = true;
        model.AddComponent<LookAtPlayer>();
        if (Player.main.IsInBase())
        {
            model.transform.localScale *= 0.7f;
        }
        return model;
    }

    private IEnumerator SpawnCorpse(Vector3 pos)
    {
        var classID = _corpseClassIDs[Random.Range(0, _corpseClassIDs.Length)];
        var task = UWE.PrefabDatabase.GetPrefabAsync(classID);
        yield return task;
        task.TryGetPrefab(out var prefab);
        var spawned = Instantiate(prefab, pos, Random.rotation);
        spawned.SetActive(true);
        var despawn = spawned.AddComponent<DespawnWhenOffScreen>();
        despawn.initialDelay = 20;
        despawn.waitUntilSeen = true;
        despawn.despawnIfTooClose = true;
        despawn.minDistance = 3;
        despawn.jumpscareWhenTooClose = true;
        despawn.rareJumpscare = true;
    }

    private void Update()
    {
        if (Time.time > _timeSpawnAgain)
        {
            _timeSpawnAgain = Time.time + Random.Range(_minDelay, _maxDelay);
            JumpScare();
        }
    }

    private void JumpScare()
    {
        var jumpscarePosition = MainCamera.camera.transform.position;
        if (JumpScareUtils.TryGetSpawnPosition(out jumpscarePosition, _radius, 50, _radius / 2f))
        {
            var model = Spawn();
            model.transform.position = jumpscarePosition;
            var diff = jumpscarePosition - Player.main.transform.position;
            diff.y = 0;
            model.transform.forward = diff.normalized;
            StartCoroutine(SpawnCorpse(jumpscarePosition + Vector3.up * 2));
        }
        Utils.PlayFMODAsset(jumpscareSound, jumpscarePosition);
    }
    
    
    public void NotifyGoalComplete(string key)
    {
        if (key == StoryUtils.ForceFieldLaserDisabled.key)
        {
            enabled = false;
        }
    }
    
    private void OnDestroy()
    {
        StoryGoalManager main = StoryGoalManager.main;
        if (main)
        {
            main.RemoveListener(this);
        }
    }
}