using UnityEngine;

namespace SeaVoyager.Mono;

public class SetSeaVoyagerKinematic : MonoBehaviour, IScheduledUpdateBehaviour
{
    public Rigidbody rb;
    public SeaVoyager voyager;
    public Transform maxInteriorYLevelMeasure;
    public int scheduledUpdateIndex { get; set; }

    public string GetProfileTag()
    {
        return "SeaVoyager:SetSeaVoyagerKinematic";
    }

    private void OnEnable()
    {
        UpdateSchedulerUtils.Register(this);
    }

    private void OnDisable()
    {
        UpdateSchedulerUtils.Deregister(this);
    }

    public void ScheduledUpdate()
    {
        var kinematic = ShouldBeKinematic();
        rb.isKinematic = kinematic;
        rb.collisionDetectionMode =
            kinematic ? CollisionDetectionMode.ContinuousSpeculative : CollisionDetectionMode.Continuous;
        rb.interpolation = kinematic ? RigidbodyInterpolation.None : RigidbodyInterpolation.Interpolate;
    }

    private bool ShouldBeKinematic()
    {
        var playerY = Player.main.transform.position.y;
        var oceanLevel = Ocean.GetOceanLevel();
        if (playerY < oceanLevel) return false;
        if (playerY > oceanLevel + 20) return false;
        if (playerY > maxInteriorYLevelMeasure.position.y) return false;
        if (rb.velocity.magnitude > 1) return false;
        if (Player.main.GetCurrentSub() != voyager) return false;
        return voyager.currentState == ShipState.Idle;
    }
}