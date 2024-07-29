using System.Collections.Generic;
using UnityEngine;

namespace TheRedPlague.Mono.CreatureBehaviour.MrTeeth;

public class MrTeethSpawnPoint : MonoBehaviour
{
    private static readonly List<MrTeethSpawnPoint> MrTeethSpawnPoints = new();

    public static IEnumerable<MrTeethSpawnPoint> SpawnPoints => MrTeethSpawnPoints;

    private void OnEnable()
    {
        if (transform.position.y < 0)
            MrTeethSpawnPoints.Add(this);
    }
    
    private void OnDisable()
    {
        MrTeethSpawnPoints.Remove(this);
    }

    public static MrTeethSpawnPoint GetRandom()
    {
        return MrTeethSpawnPoints.Count == 0 ? null : MrTeethSpawnPoints[Random.Range(0, MrTeethSpawnPoints.Count)];
    }
}