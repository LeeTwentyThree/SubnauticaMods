using System.Collections;
using Story;
using UnityEngine;

namespace TheRedPlague.Mono;

public class RandomFishSpawner : MonoBehaviour
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
        StartCoroutine(SpawnFishAsync(_fishTechTypes[Random.Range(0, _fishTechTypes.Length)], camTransform.position + camTransform.forward * Random.Range(-2, -13)));
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