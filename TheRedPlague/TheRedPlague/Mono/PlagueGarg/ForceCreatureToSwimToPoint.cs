using UnityEngine;

namespace TheRedPlague.Mono.PlagueGarg;

public class ForceCreatureToSwimToPoint : CreatureAction
{
    public Vector3 position;
    public float swimVelocity;
    
    private void Start()
    {
        evaluatePriority = 100f;
        creature.ScanCreatureActions();
    }

    public override float Evaluate(Creature creature, float time)
    {
        return 100f;
    }

    public override void Perform(Creature creature, float time, float deltaTime)
    {
        swimBehaviour.SwimTo(position, swimVelocity);
    }
}