using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SeaVoyager.Mono
{
    public class SkyraySpawner : MonoBehaviour
    {
        private List<Transform> _spawnPoints;
        private GameObject _prefab;

        private IEnumerator Start()
        {
            _spawnPoints = new List<Transform>();
            foreach(Transform child in transform)
            {
                _spawnPoints.Add(child);
            }

            var task = CraftData.GetPrefabForTechTypeAsync(TechType.Skyray);
            yield return task;
            _prefab = task.GetResult();
        }

        public void SpawnSkyrays(int amount)
        {
            var possibleSpawnPoints = new List<Transform>(_spawnPoints);
            var spawned = 0;
            while (possibleSpawnPoints.Count > 0 && spawned < amount)
            {
                Transform random = possibleSpawnPoints[Random.Range(0, possibleSpawnPoints.Count)];
                Instantiate(_prefab, random.position, Quaternion.identity);
                possibleSpawnPoints.Remove(random);
                spawned++;
            }
        }
    }
}
