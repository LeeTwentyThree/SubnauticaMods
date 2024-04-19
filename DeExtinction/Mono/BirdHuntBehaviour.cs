using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DeExtinction.Mono;

public class BirdHuntBehaviour : CreatureAction
{
    public float minAttackDelay = 20f;
    public float maxHorizontalDistance = 60f;
    public float maxFishDepth = 5f;
    public float diveVelocity = 14;
    public float minHungerForAttacks = 0.8f;
    public float maxAttackDuration = 6;
    public EcoTargetType preyTargetType = EcoTargetType.SmallFish;

    private float _timeCanAttackAgain;
    private bool _attacking;
    private float _giveUpTime;
    private GameObject _target;
    private WorldForces _worldForces;

    private EcoRegion.TargetFilter _isTargetValidFilter;

    private void Start()
    {
        InvokeRepeating(nameof(ScanForPrey), Random.value, 2f);
        _isTargetValidFilter = IsTargetValid;
        _worldForces = GetComponent<WorldForces>();
    }

#if SUBNAUTICA
    public override float Evaluate(Creature creature, float time)
#elif BELOWZERO
    public override float Evaluate(float time)
#endif
    {
        if ((_attacking && Time.time > _giveUpTime) || _target == null)
        {
            return 0;
        }

        return evaluatePriority;
    }

    private bool IsTargetValid(IEcoTarget target)
    {
        var targetObject = target.GetGameObject();
        
        if (targetObject == null)
        {
            return false;
        }

        return Ocean.GetDepthOf(targetObject) < maxFishDepth ||
               Vector3.Distance(GetPositionOfSeaBelow(), targetObject.transform.position) < maxHorizontalDistance;
    }

#if SUBNAUTICA
    public override void StartPerform(Creature creature, float time)
#elif BELOWZERO
    public override void StartPerform(float time)
#endif
    {
        _attacking = true;
        if (_target)
        {
            _target.EnsureComponent<ForceFishToSurface>().evaluatePriority = 1;
            creature.ScanCreatureActions();
        }

        _giveUpTime = Time.time + maxAttackDuration;
        _worldForces.aboveWaterGravity = 5f;
    }
    
#if SUBNAUTICA
    public override void StopPerform(Creature creature, float time)
#elif BELOWZERO
    public override void StopPerform(float time)
#endif
    {
        _attacking = false;
        if (_target != null)
        {
            var forceFishToSurface = _target.GetComponent<ForceFishToSurface>();
            if (forceFishToSurface) forceFishToSurface.evaluatePriority = 0;
        }
        _target = null;
        _timeCanAttackAgain = Time.time + minAttackDelay;
        _worldForces.aboveWaterGravity = 0f;
    }

#if SUBNAUTICA
    public override void Perform(Creature creature, float time, float deltaTime)
#elif BELOWZERO
    public override void Perform(float time, float deltaTime)
#endif
    {
        if (_target == null) return;
        swimBehaviour.SwimTo(_target.transform.position, diveVelocity);
    }

    private void ScanForPrey()
    {
        if (!isActiveAndEnabled || Time.time < _timeCanAttackAgain || creature.Hunger.Value < minHungerForAttacks || EcoRegionManager.main == null || _attacking || _target != null)
        {
            return;
        }

        var nearestTarget = EcoRegionManager.main.FindNearestTarget(preyTargetType, GetPositionOfSeaBelow(), _isTargetValidFilter, 2);

        if (nearestTarget == null || nearestTarget.GetGameObject() == null)
        {
            return;
        }
        
        _target = nearestTarget.GetGameObject();
    }

    private Vector3 GetPositionOfSeaBelow()
    {
        return new Vector3(transform.position.x, Ocean.GetOceanLevel(), transform.position.z);
    }
}