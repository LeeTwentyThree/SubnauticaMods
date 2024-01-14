using System.Collections;
using Story;
using UnityEngine;

namespace TheRedPlague.Mono;

public class EnzymeRainController : MonoBehaviour, IStoryGoalListener
{
    private bool _raining;
    private bool _rainStopped;

    private float _timeSpawnAgain;

    private GameObject _prefab;
    private bool _prefabLoaded;
    
    private IEnumerator Start()
    {
        yield return new WaitUntil(() =>
            StoryGoalManager.main.completedGoals != null && StoryGoalManager.main.completedGoals.Count > 0);
        StoryGoalManager.main.AddListener(this);
        if (StoryGoalManager.main.IsGoalComplete(StoryUtils.DisableDome.key))
        {
            _rainStopped = true;
        }
        else if (StoryGoalManager.main.IsGoalComplete(StoryUtils.EnzymeRainEnabled.key))
        {
            StartRaining();
        }
        var request = UWE.PrefabDatabase.GetPrefabAsync("505e7eff-46b3-4ad2-84e1-0fadb7be306c");
        yield return request;
        if (!request.TryGetPrefab(out _prefab))
        {
            ErrorMessage.AddMessage("FAILED TO LOAD ENZYME PREFAB!");
        }
        else
        {
            _prefabLoaded = true;
        }
    }

    public void NotifyGoalComplete(string key)
    {
        if (key == StoryUtils.EnzymeRainEnabled.key)
        {
            StartRaining();
        }
        if (key == StoryUtils.DisableDome.key)
        {
            _rainStopped = true;
        }
    }

    private void StartRaining()
    {
        _raining = true;
    }

    private void Update()
    {
        if (_raining && !_rainStopped && _prefabLoaded && Time.time > _timeSpawnAgain)
        {
            for (var i = 0; i < Random.Range(2, 5); i++)
                SpawnOne();
            _timeSpawnAgain = Time.time + Random.Range(0.2f, 0.7f);
        }
    }

    private void SpawnOne()
    {
        var angle = Random.value * 2f * Mathf.PI;
        var enzymeBall = Instantiate(_prefab, new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * Random.Range(0, 50) + MainCamera.camera.transform.position + Vector3.up * Random.Range(40, 60), Quaternion.identity);
        enzymeBall.SetActive(true);
        Destroy(enzymeBall.GetComponent<LargeWorldEntity>());
        enzymeBall.transform.parent = null;
        Destroy(enzymeBall, 60);
    }
    
    private void OnDestroy()
    {
        StoryGoalManager main = StoryGoalManager.main;
        if (main)
        {
            main.RemoveListener(this);
        }
    }
}