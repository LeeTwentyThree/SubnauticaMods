using Nautilus.Handlers;
using Story;
using UnityEngine;

namespace TheRedPlague.Mono;

public class PlayerInfectDamage : MonoBehaviour, IStoryGoalListener
{
    private void Start()
    {
        InvokeRepeating(nameof(Damage), 1, 1);
        StoryGoalManager.main.AddListener(this);
    }

    private void Damage()
    {
        if (!isActiveAndEnabled) return;
        Player.main.liveMixin.TakeDamage(1);
    }

    public void NotifyGoalComplete(string key)
    {
        if (key == StoryUtils.ForceFieldLaserDisabled.key)
        {
            enabled = false;
        }
    }
    
    private void OnDestroy()
    {
        StoryGoalManager.main.RemoveListener(this);
    }
}