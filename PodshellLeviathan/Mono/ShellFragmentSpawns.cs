using System.Collections;
using PodshellLeviathan.Prefabs;
using UnityEngine;

namespace PodshellLeviathan.Mono;

public class ShellFragmentSpawns : MonoBehaviour, IScheduledUpdateBehaviour
{
    public Transform[] spawnPositions;

    public float minSpawnInterval = 18f;
    public float maxSpawnInterval = 32f;
    public float chancePerAttempt = 0.6f;
    public Vector3 spawnScale;
    
    public int scheduledUpdateIndex { get; set; }

    private float _timeNextSpawnAttempt;
    
    private void OnEnable()
    {
        UpdateSchedulerUtils.Register(this);
    }

    private void OnDisable()
    {
        UpdateSchedulerUtils.Deregister(this);
    }

    public void ScheduledUpdate()
    {
        if (Time.time < _timeNextSpawnAttempt) return;
        
        _timeNextSpawnAttempt = Time.time + Random.Range(minSpawnInterval, maxSpawnInterval);

        if (Random.value < chancePerAttempt)
        {
            var randomSpawnPosition = spawnPositions[Random.Range(0, spawnPositions.Length)];
            StartCoroutine(SpawnFragmentCoroutine(randomSpawnPosition));
        }
    }

    private IEnumerator SpawnFragmentCoroutine(Transform spawnPosition)
    {
        var task = CraftData.GetPrefabForTechTypeAsync(ShellFragmentPrefab.Info.TechType);
        yield return task;
        var prefab = task.GetResult();
        
        var obj = Instantiate(prefab, spawnPosition.position, spawnPosition.rotation);
        obj.transform.localScale = spawnScale;
        LargeWorldStreamer.main.MakeEntityTransient(obj);
        obj.AddComponent<DestroyItemAfterDelay>().delay = 90;
    }
    
    public string GetProfileTag()
    {
        return "Podshell:ShellFragmentSpawns";
    }
}