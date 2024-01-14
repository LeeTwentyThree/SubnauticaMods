using System.Collections;
using Story;
using UnityEngine;

namespace TheRedPlague.Mono;

public class DeadSeaEmperorSpawner : MonoBehaviour
{
    // gary
    private IEnumerator Start()
    {
        yield return new WaitUntil(() => StoryGoalManager.main.IsGoalComplete(StoryUtils.OpenAquariumTeleporterGoalKey));

        var lws = LargeWorldStreamer.main;
        yield return new WaitUntil(() => lws != null && lws.IsReady());
        
        yield return new WaitUntil(() => lws.IsBatchReadyToCompile(new Int3(12, 18, 12)) && lws.IsBatchReadyToCompile(new Int3(12, 19, 12)));

        var lw = LargeWorld.main;
        
        var emperorTask = CraftData.GetPrefabForTechTypeAsync(ModPrefabs.DeadSeaEmperorInfo.TechType);
        yield return emperorTask;
        
        var emperor = Instantiate(emperorTask.GetResult());
        emperor.transform.position = new Vector3(-44, -11, 20);
        emperor.transform.localEulerAngles = new Vector3(5, 0, 10);
        lw.streamer.cellManager.RegisterEntity(emperor);

        var enzymeParticleTask = CraftData.GetPrefabForTechTypeAsync(ModPrefabs.EnzymeParticleInfo.TechType);
        yield return enzymeParticleTask;
        var enzymeParticlePrefab = enzymeParticleTask.GetResult();
        var enzymeSpawnCenter = new Vector3(-54.508f, 20, -42.000f);
        for (var i = 0; i < 50; i++)
        {
            lw.streamer.cellManager.RegisterEntity(Instantiate(enzymeParticlePrefab, enzymeSpawnCenter + Random.insideUnitSphere * 20, Random.rotation));
        }
        
        Destroy(gameObject);
    }
}