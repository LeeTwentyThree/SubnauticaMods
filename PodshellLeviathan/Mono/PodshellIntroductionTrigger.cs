using System;
using Story;
using UnityEngine;

namespace PodshellLeviathan.Mono;

public class PodshellIntroductionTrigger : MonoBehaviour, IScheduledUpdateBehaviour, IStoryGoalListener
{
    public StoryGoal goal;
    public float maxDistance = 100;
    
    private void Start()
    {
        if (!StoryGoalManager.main.IsGoalComplete(goal.key))
        {
            StoryGoalManager.main.AddListener(this);
            UpdateSchedulerUtils.Register(this);
        }
    }

    private void OnDestroy()
    {
        StoryGoalManager.main.RemoveListener(this);
        UpdateSchedulerUtils.Deregister(this);
    }

    public string GetProfileTag()
    {
        return "Podshell:PlayPodshellMusic";
    }

    public void ScheduledUpdate()
    {
        if (Vector3.SqrMagnitude(transform.position - Player.main.transform.position) < maxDistance * maxDistance)
        {
            if (!StoryGoalManager.main.IsGoalComplete(goal.key))
            {
                FMODUWE.PlayOneShot(ModAudio.PodshellMusic, Player.main.transform.position);
                goal.Trigger();
            }
            UpdateSchedulerUtils.Deregister(this);
        }
    }

    public int scheduledUpdateIndex { get; set; }
    
    public void NotifyGoalComplete(string key)
    {
        if (key.Equals(goal.key, StringComparison.OrdinalIgnoreCase))
        {
            UpdateSchedulerUtils.Deregister(this);
        }
    }
}