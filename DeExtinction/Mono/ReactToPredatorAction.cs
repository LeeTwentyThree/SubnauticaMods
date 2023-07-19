using UnityEngine;

namespace DeExtinction.Mono;

internal class ReactToPredatorAction : CreatureAction
{
    private void Start()
    {
        fear = gameObject.GetComponent<CreatureFear>();
    }

    public float maxReactDistance;
    public EcoTargetType fearedTargetType = EcoTargetType.Shark;
    public float actionLength = 1f;

    protected CreatureFear fear;
    protected bool performingAction;
    protected float timeStopAction;

    private bool _frozen;

    public override float Evaluate(Creature creature, float time)
    {
        if (_frozen)
        {
            return 0f;
        }
        IEcoTarget closestTarget = EcoRegionManager.main.FindNearestTarget(fearedTargetType, transform.position, null, 1);
        if (closestTarget != null && closestTarget.GetGameObject() != null)
        {
            if (Vector3.Distance(closestTarget.GetPosition(), transform.position) < maxReactDistance)
            {
                performingAction = true;
                timeStopAction = Time.time + actionLength;
                if (fear) fear.SetScarePosition(closestTarget.GetPosition());
            }
        }
        if (performingAction)
        {
            if (Time.time > timeStopAction)
            {
                performingAction = false;
            }
        }

        if (performingAction)
        {
            return evaluatePriority;
        }
        else
        {
            return 0f;
        }
    }

    public void OnFreeze()
    {
        _frozen = true;
    }

    public void OnUnfreeze()
    {
        _frozen = false;
    }
}