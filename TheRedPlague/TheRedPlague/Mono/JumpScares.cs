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
    private float _minDelay = 60;
    private float _maxDelay = 90;
    private float _radius = 40;
    private float _timeSpawnAgain;

    private static FMODAsset jumpscareSound = AudioUtils.GetFmodAsset("WarperJumpscare");

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
        despawn.minDistance = 25;
        despawn.waitUntilSeen = true;
        despawn.moveInstead = true;
        despawn.moveRadius = 35f;
        despawn.disappearWhenLookedAtForTooLong = true;
        model.AddComponent<LookAtPlayer>();
        if (Player.main.IsInBase())
        {
            model.transform.localScale *= 0.4f;
        }
        return model;
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