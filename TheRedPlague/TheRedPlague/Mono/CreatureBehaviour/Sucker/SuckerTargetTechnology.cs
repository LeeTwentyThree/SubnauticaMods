using UnityEngine;

namespace TheRedPlague.Mono.CreatureBehaviour.Sucker;

public class SuckerTargetTechnology : CreatureAction
{
    public float swimVelocity;
    public float radius;
    
    private SuckerControllerTarget _target;
    
    private void Start()
    {
        InvokeRepeating(nameof(UpdateTarget), Random.value, 2f);
    }

    public override float Evaluate(Creature creature, float time)
    {
        if (_target != null) return evaluatePriority;
        return 0;
    }
    
    public override void Perform(Creature creature, float time, float deltaTime)
    {
        if (_target == null) return;
        swimBehaviour.SwimTo(_target.transform.position, swimVelocity);
    }

    private void UpdateTarget()
    {
        SuckerControllerTarget.TryGetClosest(out _target, transform.position, radius);
    }
}