using System.Collections;
using TheRedPlague.PrefabFiles;
using UnityEngine;

namespace TheRedPlague.Mono;

public class AdminDropPodFall : MonoBehaviour
{
    private static readonly Vector3 _spawnPos = new Vector3();
    private static readonly Vector3 _finalPos = new Vector3(-175.090f, -667.283f, 3285.797f);
    private static readonly float _silenceGrabDepth = 398;
    private static readonly Vector3 _finalEulerAngles = new Vector3(8, 34.3f, 0.36f);
    
    public static void SpawnAdministratorDropPod()
    {
        UWE.CoroutineHost.StartCoroutine(LoadAndSpawnAdminDropPod());
    }

    private static IEnumerator LoadAndSpawnAdminDropPod()
    {
        var task = CraftData.GetPrefabForTechTypeAsync(AdministratorDropPod.Info.TechType);
        yield return task;
        var prefab = task.GetResult();
        var stationaryDropPod = Instantiate(prefab, _finalPos, Quaternion.Euler(_finalEulerAngles));
        stationaryDropPod.SetActive(true);
        LargeWorldStreamer.main.cellManager.RegisterGlobalEntity(stationaryDropPod);
        var fallingDropPod = Instantiate(prefab, _spawnPos, Quaternion.identity);
        fallingDropPod.AddComponent<AdminDropPodFall>();
        var ping = fallingDropPod.AddComponent<PingInstance>();
        ping.SetType(AdministratorDropPod.PingType);
    }
}