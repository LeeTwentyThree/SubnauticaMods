using System.Collections;
using Nautilus.Handlers;
using Nautilus.Json;
using UnityEngine;
using UWE;

namespace DeExtinction.Mono;

public class SpawnDragonfliesAroundEscapePod : MonoBehaviour
{
    private static SaveDragonflySpawns _saveData = SaveDataHandler.RegisterSaveDataCache<SaveDragonflySpawns>();
    
    private void Start()
    {
        UWE.CoroutineHost.StartCoroutine(Spawn());
    }

    private IEnumerator Spawn()
    {
        yield return new WaitUntil(() => LargeWorldStreamer.main != null);
        
        if (_saveData.spawnedDragonfliesAbovePlayerSpawn)
        {
            yield break;
        }
        
        var streamer = LargeWorldStreamer.main;
        var birdSpawnCenter = transform.Find("birdSpawn").position;
        yield return new WaitUntil(() => streamer.IsReady() && streamer.IsBatchReadyToCompile(streamer.GetContainingBatch(birdSpawnCenter)));

        var task = PrefabDatabase.GetPrefabAsync(CreaturePrefabManager.Dragonfly.ClassID);
        yield return task;
        if (!task.TryGetPrefab(out var dragonFlyPrefab))
        {
            Plugin.Logger.LogError("Failed to load Dragonfly prefab!");
            yield break;
        }
        
        for (var i = 0; i < 4; i++)
        {
            var largeWorld = LargeWorldStreamer.main;
            var bird = Instantiate(dragonFlyPrefab, birdSpawnCenter + Random.onUnitSphere * 4, Quaternion.Euler(0, Random.value * 360, 0));
            if (largeWorld != null && largeWorld.cellManager != null)
            {
                largeWorld.cellManager.RegisterEntity(bird);
            }
        }

        _saveData.spawnedDragonfliesAbovePlayerSpawn = true;
    }
}

public class SaveDragonflySpawns : SaveDataCache
{
    public bool spawnedDragonfliesAbovePlayerSpawn;
}