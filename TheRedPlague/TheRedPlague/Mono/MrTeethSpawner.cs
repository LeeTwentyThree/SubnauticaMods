using System.Collections;
using Nautilus.Utility;
using UnityEngine;

namespace TheRedPlague.Mono;

public class MrTeethSpawner : MonoBehaviour
{
    public float maxDistance = 7f;
    public float minInterval = 10f;
    
    private float _timeMrTeethCanSpawnAgain;
    private GameObject _mrTeethInstance;
    
    private static readonly FMODAsset SpawnSound = AudioUtils.GetFmodAsset("MrTeethScream");

    private void Update()
    {
        if (!MrTeethCanSpawn()) return;
        foreach (var spawnPoint in MrTeethSpawnPoint.SpawnPoints)
        {
            if (Vector3.SqrMagnitude(spawnPoint.transform.position - Player.main.transform.position) <
                maxDistance * maxDistance)
            {
                SpawnMrTeeth(spawnPoint);
                break;
            }
        }
    }

    private bool MrTeethCanSpawn()
    {
        return Time.time > _timeMrTeethCanSpawnAgain && _mrTeethInstance == null;
    }
    
    private void SpawnMrTeeth(MrTeethSpawnPoint spawnPoint)
    {
        _timeMrTeethCanSpawnAgain = Time.time + minInterval;
        StartCoroutine(SpawnMrTeethCoroutine(spawnPoint.transform.position - spawnPoint.transform.forward * 2, spawnPoint.transform.forward));
    }

    private IEnumerator SpawnMrTeethCoroutine(Vector3 spawnPosition, Vector3 spawnDirection)
    {
        var task = CraftData.GetPrefabForTechTypeAsync(ModPrefabs.MrTeeth.TechType);
        yield return task;
        var mrTeeth = UWE.Utils.InstantiateDeactivated(task.GetResult());
        DestroyImmediate(mrTeeth.GetComponent<LargeWorldEntity>());
        DestroyImmediate(mrTeeth.GetComponent<PrefabIdentifier>());
        var mrTeethTransform = mrTeeth.transform;
        mrTeethTransform.position = spawnPosition;
        // mrTeethTransform.forward = spawnDirection;
        mrTeethTransform.LookAt(MainCamera.camera.transform);
        mrTeeth.GetComponent<Rigidbody>().velocity = mrTeethTransform.forward * 4;
        mrTeeth.SetActive(true);
        _mrTeethInstance = mrTeeth;
        Utils.PlayFMODAsset(SpawnSound, transform.position);
    }
}