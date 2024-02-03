using System.Collections;
using UnityEngine;

namespace TheRedPlague.Mono;

public class WarperDropHeartOnDeath : MonoBehaviour
{
    public void OnKill()
    {
        UWE.CoroutineHost.StartCoroutine(SpawnHeartCoroutine(transform.position));
    }

    private IEnumerator SpawnHeartCoroutine(Vector3 position)
    {
        var task = CraftData.GetPrefabForTechTypeAsync(ModPrefabs.WarperHeart.TechType);
        yield return task;
        var obj = Instantiate(task.GetResult(), position, Quaternion.identity);
        obj.SetActive(true);
        LargeWorld.main.streamer.cellManager.RegisterEntity(obj);
    }
}