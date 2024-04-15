using UnityEngine;

namespace DeExtinction.Mono;

internal class FleeFromPredators : ReactToPredatorAction
{
    public float swimVelocity;

#if SUBNAUTICA
    public override void Perform(Creature creature, float time, float deltaTime)
#elif BELOWZERO
    public override void Perform(float time, float deltaTime)
#endif
    {
        var targetDirection = (transform.position - Fear.lastScarePosition).normalized;
        swimBehaviour.SwimTo(transform.position + targetDirection * (swimVelocity * 5f), swimVelocity);
    }
}