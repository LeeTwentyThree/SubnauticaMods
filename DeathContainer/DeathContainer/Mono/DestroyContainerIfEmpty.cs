using UnityEngine;

namespace DeathContainer.Mono;

public class DestroyContainerIfEmpty : MonoBehaviour, IScheduledUpdateBehaviour
{
    public StorageContainer container;
    
    public int scheduledUpdateIndex { get; set; }

    private bool _scheduledForDeletion;
    
    public string GetProfileTag()
    {
        return "DeathContainer:DestroyContainerIfEmpty";
    }

    public void ScheduledUpdate()
    {
        if (_scheduledForDeletion || !Plugin.Options.DestroyEmptyContainers)
            return;
        
        if (container.IsEmpty())
        {
            UpdateSchedulerUtils.Deregister(this);
            _scheduledForDeletion = true;
            Invoke(nameof(DelayedDestroy), Plugin.Options.DestroyDelaySeconds);
        }
    }

    private void Start()
    {
        UpdateSchedulerUtils.Register(this);
    }

    private void OnDestroy()
    {
        UpdateSchedulerUtils.Deregister(this);
    }

    private void DelayedDestroy()
    {
        if (container != null && !container.IsEmpty())
        {
            // DON'T DESTROY IF IT'S NOT EMPTY
            _scheduledForDeletion = false;
            return;
        } 
        Destroy(gameObject);
    }
}