using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ShipMod.Ship
{
    public class SkyraySpawner : MonoBehaviour
    {
        List<Transform> spawnPoints;
        GameObject prefab;

        void Awake()
        {
            spawnPoints = new List<Transform>();
            foreach(Transform child in transform)
            {
                spawnPoints.Add(child);
            }
            prefab = CraftData.GetPrefabForTechType(TechType.Skyray);
        }

        public void SpawnSkyrays(int amount)
        {
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
