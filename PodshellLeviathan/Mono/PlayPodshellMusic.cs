using System;
using Story;
using UnityEngine;

namespace PodshellLeviathan.Mono;

public class PlayPodshellMusic : MonoBehaviour, IScheduledUpdateBehaviour, IStoryGoalListener
{
    public string goalKey = "PodshellIntroductionMusic";
    public float maxDistance = 40;
    
    private void Start()
    {
        if (!StoryGoalManager.main.IsGoalComplete(goalKey))
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
            if (StoryGoalManager.main.OnGoalComplete(goalKey))
            {
                FMODUWE.PlayOneShot(ModAudio.PodshellMusic, Player.main.transform.position);
            }
            UpdateSchedulerUtils.Deregister(this);
        }
    }

    public int scheduledUpdateIndex { get; set; }
    
    public void NotifyGoalComplete(string key)
    {
        if (key.Equals(goalKey, StringComparison.OrdinalIgnoreCase))
        {
            UpdateSchedulerUtils.Deregister(this);
        }
    }
}