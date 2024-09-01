using System.Collections;
using UnityEngine;

namespace TheRedPlague.Mono.CinematicEvents;

public class DomeExplosion : MonoBehaviour
{
    public static void ExplodeDome()
    {
        new GameObject("DomeExplosion").AddComponent<DomeExplosion>();
    }

    private IEnumerator Start()
    {
        var seamothTask = CraftData.GetPrefabForTechTypeAsync(TechType.Seamoth);
        yield return seamothTask;
        var seamothPrefab = seamothTask.GetResult();
        var destructionEffect = Object.Instantiate(seamothPrefab.GetComponent<SeaMoth>().destructionEffect);
        destructionEffect.SetActive(false);
        foreach (var ps in destructionEffect.GetComponentsInChildren<ParticleSystem>())
        {
            var main = ps.main;
            main.startSizeMultiplier *= 15;
            main.scalingMode = ParticleSystemScalingMode.Hierarchy;
        }
        destructionEffect.transform.GetChild(1).gameObject.SetActive(false);
        Destroy(destructionEffect.GetComponent<FMOD_StudioEventEmitter>());

        for (int i = 0; i < 100; i++)
        {
            yield return new WaitForSeconds(Random.Range(0.1f, 0.5f));
            var randomDomePos = Random.onUnitSphere * 2000;
            randomDomePos = new Vector3(randomDomePos.x, Mathf.Abs(randomDomePos.y), randomDomePos.z);
            var exp = Instantiate(destructionEffect, randomDomePos, Quaternion.identity);
            exp.SetActive(true);
            exp.transform.localScale *= Random.Range(5, 20);
            Destroy(exp, 20);
        }
        
        Destroy(gameObject);
        Destroy(destructionEffect);
        
        InfectionDomeController.main.ShatterDome();
    }
}