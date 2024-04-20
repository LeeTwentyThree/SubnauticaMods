using UnityEngine;
using Random = UnityEngine.Random;

namespace DeExtinction.Mono;

public class BirdHuntBehaviour : CreatureAction
{
    public float minAttackDelay = 20f;
    public float maxHorizontalDistance = 10f;
    public float maxFishDepth = 10f;
    public float diveVelocity = 12f;
    public float swimVelocity = 20;
    public float resurfaceVelocity = 8;
    public float minHungerForAttacks = 0.8f;
    public float maxAttackDuration = 20;
    public float maxUnderwaterTime = 4f;
    public EcoTargetType preyTargetType = EcoTargetType.SmallFish;

    private float _timeCanAttackAgain;
    private bool _attacking;
    private float _giveUpTime;
    private GameObject _target;
    private LiveMixin _targetLiveMixin;
    
    private WorldForces _worldForces;
    private Locomotion _locomotion;
    private float _defaultForwardRotationSpeed;
    private float _defaultMaxAccel;

    private EcoRegion.TargetFilter _isTargetValidFilter;
    private bool _submergedDuringAttack;
    private bool _leavingWaterDuringAttack;
    private float _timeSubmergeStart;
    
    private static readonly int Flapping = Animator.StringToHash("flapping");
    private static readonly int Swimming = Animator.StringToHash("swimming");

    private void Start()
    {
        InvokeRepeating(nameof(ScanForPrey), Random.value, 0.3f);
        _isTargetValidFilter = IsTargetValid;
        _worldForces = GetComponent<WorldForces>();
        _locomotion = GetComponent<Locomotion>();
        _defaultForwardRotationSpeed = _locomotion.forwardRotationSpeed;
        _defaultMaxAccel = _locomotion.maxAcceleration;
    }

#if SUBNAUTICA
    public override float Evaluate(Creature creature, float time)
#elif BELOWZERO
    public override float Evaluate(float time)
#endif
    {
        if ((_attacking && Time.time > _giveUpTime) || _target == null || creature.Hunger.Value < minHungerForAttacks)
        {
            return 0;
        }

        if (_attacking && transform.position.y > Ocean.GetOceanLevel() && _submergedDuringAttack)
        {
            return 0;
        }

        if (!_attacking && _target != null && Vector3.Distance(_target.transform.position, GetPositionOfSeaBelow()) >
            maxHorizontalDistance)
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

        if (targetObject.GetComponent<Creature>() == null)
        {
            return false;
        }

        return Ocean.GetDepthOf(targetObject) < maxFishDepth &&
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
            _target.EnsureComponent<ForceFishToSurface>().evaluatePriority = 5;
            var otherCreature = _target.GetComponent<Creature>();
            otherCreature.ScanCreatureActions();
            otherCreature.UpdateBehaviour(Time.time, Time.time - otherCreature.lastUpdateTime);
        }

        _giveUpTime = Time.time + maxAttackDuration;
        _worldForces.aboveWaterGravity = 0.5f;
        _submergedDuringAttack = false;
        _leavingWaterDuringAttack = false;
        _locomotion.forwardRotationSpeed = 0.4f;
        _locomotion.maxAcceleration = 5;
        creature.GetAnimator().SetBool(Flapping, true);
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
        _targetLiveMixin = null;
        _timeCanAttackAgain = Time.time + minAttackDelay;
        _worldForces.aboveWaterGravity = 0f;
        _locomotion.forwardRotationSpeed = _defaultForwardRotationSpeed;
        _locomotion.maxAcceleration = _defaultMaxAccel;
        creature.GetAnimator().SetBool(Flapping, false);
        creature.GetAnimator().SetBool(Swimming, false);
    }

#if SUBNAUTICA
    public override void Perform(Creature creature, float time, float deltaTime)
#elif BELOWZERO
    public override void Perform(float time, float deltaTime)
#endif
    {
        if (Ocean.GetDepthOf(gameObject) > 0)
        {
            if (!_submergedDuringAttack)
            {
                _timeSubmergeStart = Time.time;
                creature.GetAnimator().SetBool(Swimming, true);
            }

            _submergedDuringAttack = true;
            if (Time.time > _timeSubmergeStart + maxUnderwaterTime || _target == null ||
                (_targetLiveMixin != null && !_targetLiveMixin.IsAlive()))
            {
                swimBehaviour.SwimTo(GetPositionOfSeaBelow() + Vector3.up * resurfaceVelocity, resurfaceVelocity);
                _leavingWaterDuringAttack = true;
            }
        }
    }

    private void Update()
    {
        if (_attacking && _target != null && !_leavingWaterDuringAttack)
        {
            swimBehaviour.SwimTo(_target.transform.position, _submergedDuringAttack ? swimVelocity : diveVelocity);
        }
    }

    private void ScanForPrey()
    {
        if (!isActiveAndEnabled || Time.time < _timeCanAttackAgain || creature.Hunger.Value < minHungerForAttacks ||
            EcoRegionManager.main == null || _attacking || _target != null)
        {
            return;
        }

        var nearestTarget =
            EcoRegionManager.main.FindNearestTarget(preyTargetType, GetPositionOfSeaBelow(), _isTargetValidFilter, 2);

        if (nearestTarget == null || nearestTarget.GetGameObject() == null)
        {
            return;
        }

        _target = nearestTarget.GetGameObject();
        _targetLiveMixin = _target.GetComponent<LiveMixin>();
    }

    private Vector3 GetPositionOfSeaBelow()
    {
        return new Vector3(transform.position.x, Ocean.GetOceanLevel(), transform.position.z);
    }
}