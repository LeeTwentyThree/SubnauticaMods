using System.Collections;
using UnityEngine;

namespace TheRedPlague.Mono;

public class DropAmalgamatedBoneOnDeath : MonoBehaviour
{
    public void OnKill()
    {
        UWE.CoroutineHost.StartCoroutine(SpawnDropsCoroutine(transform.position));
    }

    private IEnumerator SpawnDropsCoroutine(Vector3 position)
    {
        var task = CraftData.GetPrefabForTechTypeAsync(ModPrefabs.AmalgamatedBone.TechType);
        yield return task;
        var obj = Instantiate(task.GetResult(), position + Random.insideUnitSphere, Quaternion.identity);
        obj.SetActive(true);
        LargeWorld.main.streamer.cellManager.RegisterEntity(obj);
    }
}