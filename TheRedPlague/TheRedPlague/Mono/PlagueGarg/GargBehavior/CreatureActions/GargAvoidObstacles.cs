using UnityEngine;

namespace TheRedPlague.Mono.PlagueGarg.GargBehavior.GargEssentials;

[RequireComponent(typeof(SwimBehaviour))]
class GargAvoidObstacles : CreatureAction
{
    public LastTarget lastTarget;
        
    public Vector3 positionOffset = Vector3.zero;
        
    public bool avoidTerrainOnly = true;
        
    public float avoidanceIterations = 10f;
    public float avoidanceDistance = 5f;
    public float avoidanceDuration = 2f;
    public float scanInterval = 1f;
    public float scanDistance = 2f;
    public float scanRadius;
    public float swimVelocity = 3f;
    public float swimInterval = 1f;

    protected float timeStartAvoidance;
        
    private Vector3 avoidancePosition;
    private bool swimDirectionFound;
    private float timeNextScan;
    private float timeNextSwim;

    public override float Evaluate(Creature creature, float time)
    {
        if (Time.time < timeStartAvoidance + avoidanceDuration)
        {
            return GetEvaluatePriority();
        }

        if (Time.time > timeNextScan)
        {
            timeNextScan = Time.time + scanInterval;
            var transform = creature.transform;
            var origin = transform.TransformPoint(positionOffset);
            var flag = false;
            if (scanRadius > 0f)
            {
                if (Physics.SphereCast(origin, scanRadius, transform.forward, out var raycastHit,
                        scanDistance, GetLayerMask(), QueryTriggerInteraction.Ignore))
                {
                    flag = IsObstacle(raycastHit.collider);
                }
            }
            else if (Physics.Raycast(origin, transform.forward, out var raycastHit, scanDistance,
                         GetLayerMask(), QueryTriggerInteraction.Ignore))
            {
                flag = IsObstacle(raycastHit.collider);
            }

            if (flag)
            {
                swimDirectionFound = false;
                return GetEvaluatePriority();
            }
        }

        return 0f;
    }
        
    public override void StopPerform(Creature creature, float time)
    {
        timeStartAvoidance = 0f;
    }
        
    public override void Perform(Creature creature, float time, float deltaTime)
    {
        if (Time.time > timeNextSwim)
        {
            if (!swimDirectionFound)
            {
                FindSwimDirection();
            }

            timeNextSwim = Time.time + swimInterval;
            var velocity = Mathf.Lerp(swimVelocity, 0f, this.creature.Tired.Value);
            swimBehaviour.SwimTo(avoidancePosition, velocity);
        }
    }
        
    private void FindSwimDirection()
    {
        var point = creature.transform.TransformPoint(positionOffset);
        avoidancePosition = point;
        timeStartAvoidance = Time.time;
        swimDirectionFound = false;
        var layerMask = GetLayerMask();
        var num = 0;
        while (num < avoidanceIterations)
        {
            var randomDirection = GetRandomDirection();
            RaycastHit raycastHit;
            if (!Physics.Raycast(point, randomDirection, out raycastHit, avoidanceDistance, layerMask, QueryTriggerInteraction.Ignore) || 
                !IsObstacle(raycastHit.collider))
            {
                avoidancePosition = point + randomDirection * avoidanceDistance;
                swimDirectionFound = true;
                return;
            }

            num++;
        }

        timeStartAvoidance = 0f;
    }
        
    protected virtual Vector3 GetRandomDirection()
    {
        return Random.onUnitSphere;
    }
        
    protected int GetLayerMask()
    {
        if (!avoidTerrainOnly)
        {
            return -5;
        }

        return Voxeland.GetTerrainLayerMask();
    }
        
    protected virtual bool IsObstacle(Collider collider)
    {
        var targetObj = lastTarget ? lastTarget.target : null;
        if (!avoidTerrainOnly && targetObj != null)
        {
            var attachedRigidbody = collider.attachedRigidbody;
            if ((attachedRigidbody ? attachedRigidbody.gameObject : collider.gameObject) == targetObj)
            {
                return false;
            }
        }

        return true;
    }
}