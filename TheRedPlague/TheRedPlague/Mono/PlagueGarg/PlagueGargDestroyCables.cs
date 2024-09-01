using UnityEngine;

namespace TheRedPlague.Mono.PlagueGarg;

public class PlagueGargDestroyCables : CreatureAction
{
    public float swimVelocity = 30;

    private BreakableCable _currentTarget;
    private bool _targetValid;

    public override float Evaluate(Creature creature, float time)
    {
        return _targetValid ? evaluatePriority : 0f;
    }

    public override void Perform(Creature creature, float time, float deltaTime)
    {
        if (_targetValid && _currentTarget != null)
            swimBehaviour.SwimTo(_currentTarget.transform.position, swimVelocity);
    }

    private void Update()
    {
        _targetValid = BreakableCable.TryGetClosestCableToPoint(transform.position, out _currentTarget);

        if (_targetValid && Vector3.Distance(_currentTarget.transform.position, transform.position) < 35)
        {
            _currentTarget.Break();
            creature.GetAnimator().SetTrigger("bite");
        }
    }
}