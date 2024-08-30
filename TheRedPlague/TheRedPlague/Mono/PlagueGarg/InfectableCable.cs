using Story;
using UnityEngine;

namespace TheRedPlague.Mono.PlagueGarg;

public class InfectableCable : MonoBehaviour, IStoryGoalListener
{
    private bool _infected;
    
    private void Start()
    {
        var goalManager = StoryGoalManager.main;
        if (goalManager.IsGoalComplete(StoryUtils.InfectCables.key))
        {
            Infect();
            return;
        }
        if (goalManager != null)
        {
            goalManager.AddListener(this);
        }
    }

    public void NotifyGoalComplete(string key)
    {
        if (_infected) return;
        if (key == StoryUtils.InfectCables.key)
        {
            Infect();
        }
    }

    private void Infect()
    {
        _infected = true;
        gameObject.AddComponent<InfectAnything>();
    }

    private void OnDestroy()
    {
        var storyGoalManager = StoryGoalManager.main;
        if (storyGoalManager)
        {
            storyGoalManager.RemoveListener(this);
        }
    }
}