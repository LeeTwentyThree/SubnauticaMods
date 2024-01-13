using Story;
using UnityEngine;

namespace TheRedPlague.Mono;

public class UninfectSeaEmperorOnEnzymeRain : MonoBehaviour, IStoryGoalListener
{
    private void Start()
    {
        if (StoryGoalManager.main.IsGoalComplete(StoryUtils.EnzymeRainEnabled.key))
        {
            UnInfect();
            return;
        }
        StoryGoalManager.main.AddListener(this);
    }

    public void NotifyGoalComplete(string key)
    {
        if (key == StoryUtils.EnzymeRainEnabled.key)
        {
            UnInfect();
        }
    }

    private void UnInfect()
    {
        foreach (var renderer in gameObject.GetComponentsInChildren<Renderer>())
        {
            var materials = renderer.materials;
            foreach (var m in materials)
            {
                m.DisableKeyword("UWE_INFECTION");
            }
        }
        Destroy(this);
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