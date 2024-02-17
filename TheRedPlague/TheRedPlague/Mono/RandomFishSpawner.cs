using System.Collections;
using Story;
using UnityEngine;

namespace TheRedPlague.Mono;

public class RandomFishSpawner : MonoBehaviour, IStoryGoalListener
{
    private float _minInterval = 40;
    private float _maxInterval = 60f;

    private float _timeJumpScareAgain;

    private TechType[] _fishTechTypes = new TechType[]
    {
        TechType.SpineEel,
        TechType.CrabSquid,
        TechType.Warper,
        TechType.Warper,
        TechType.Shocker
    };
    
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
    
    public void NotifyGoalComplete(string key)
    {
        if (key == StoryUtils.ForceFieldLaserDisabled.key)
        {
            enabled = false;
        }
    }

    private bool CanJumpscare()
    {
        return Player.main.IsSwimming() && !Player.main.IsInside() && !Player.main.justSpawned && !StoryGoalManager.main.IsGoalComplete(StoryUtils.EnzymeRainEnabled.key);
    }

    private void Update()
    {
        if (Time.time < _timeJumpScareAgain)
        {
            return;
        }
        
        if (CanJumpscare())
        {
            Jumpscare();
        }

        _timeJumpScareAgain = Time.time + Random.Range(_minInterval, _maxInterval);
    }

    private void Jumpscare()
    {
        var camTransform = MainCamera.camera.transform;
        StartCoroutine(SpawnFishAsync(GetRandomFishTechType(), camTransform.position + camTransform.forward * Random.Range(-2, -13)));
    }

    private TechType GetRandomFishTechType()
    {
        if (Player.main.transform.position.y < -300)
        {
            if (Random.value < 0.2f)
            {
                return Random.value > 0.5f ? ModPrefabs.MutantDiver1.TechType : ModPrefabs.MutantDiver2.TechType;
            }
        }

        return _fishTechTypes[Random.Range(0, _fishTechTypes.Length)];
    }

    private IEnumerator SpawnFishAsync(TechType techType, Vector3 location)
    {
        var task = CraftData.GetPrefabForTechTypeAsync(techType);
        yield return task;
        var result = task.GetResult();
        if (!result) yield break;
        var fish = Instantiate(result, location, Quaternion.identity);
        fish.SetActive(true);
        ZombieManager.Zombify(fish);
        var despawn = fish.AddComponent<DespawnWhenOffScreen>();
        despawn.initialDelay = 20f;
        fish.AddComponent<PlaySoundWhenSeen>();
    }
}