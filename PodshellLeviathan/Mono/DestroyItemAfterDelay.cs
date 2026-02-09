using UnityEngine;

namespace PodshellLeviathan.Mono;

public class DestroyItemAfterDelay : MonoBehaviour, IScheduledUpdateBehaviour
{
    public float delay = 60f;
    public float minDespawnDistanceSqr = 45 * 45;

    private float _destroyTime;
    private bool _timedOut;

    public int scheduledUpdateIndex { get; set; }

    private Pickupable _pickupable;

    public string GetProfileTag()
    {
        return "TRP:DestroyItemAfterDelay";
    }

    private void Start()
    {
        _destroyTime = Time.time + delay;
        UpdateSchedulerUtils.Register(this);
        _pickupable = GetComponent<Pickupable>();
        if (_pickupable != null)
        {
            _pickupable.droppedEvent.AddHandler(gameObject, OnPickup);
        }
    }

    private void OnDestroy()
    {
        UpdateSchedulerUtils.Deregister(this);
    }

    public void ScheduledUpdate()
    {
        if (_timedOut || Time.time < _destroyTime)
        {
            return;
        }

        _timedOut = true;

        if (ShouldDestroy())
        {
            Destroy(gameObject);
        }
    }
    
    private void OnPickup(Pickupable p)
    {
        _timedOut = true;
    }

    private bool ShouldDestroy()
    {
        if (!isActiveAndEnabled)
            return false;
        var distanceSqr = Vector3.SqrMagnitude(transform.position - MainCamera.camera.transform.position);
        if (distanceSqr < minDespawnDistanceSqr)
            return false;
        var pickupable = GetComponent<Pickupable>();
        if (pickupable != null && Inventory.main.Contains(pickupable))
            return false;
        return true;
    }
}