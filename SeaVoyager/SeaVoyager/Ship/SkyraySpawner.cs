using System.Collections;
using UnityEngine;

namespace SeaVoyager.Ship
{
    public class SkyraySpawner : MonoBehaviour
    {
        List<Transform> spawnPoints;
        GameObject prefab;

        IEnumerator Start()
        {
            spawnPoints = new List<Transform>();
            foreach(Transform child in transform)
            {
                spawnPoints.Add(child);
            }
            var task = CraftData.GetPrefabForTechTypeAsync(TechType.Skyray);
            yield return task;
            prefab = task.GetResult();
        }

        public void SpawnSkyrays(int amount)
        {
            if (prefab == null) return;
            List<Transform> possibleSpawnPoints = new List<Transform>(spawnPoints);
            int spawned = 0;
            while (possibleSpawnPoints.Count > 0 && spawned < amount)
            {
                Transform random = possibleSpawnPoints[Random.Range(0, possibleSpawnPoints.Count)];
                Instantiate(prefab, random.position, Quaternion.identity);
                possibleSpawnPoints.Remove(random);
                spawned++;
            }
        }
    }
}
