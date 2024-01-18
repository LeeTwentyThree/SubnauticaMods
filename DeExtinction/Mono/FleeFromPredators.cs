using UnityEngine;

namespace DeExtinction.Mono;

internal class FleeFromPredators : ReactToPredatorAction
{
    public float swimVelocity;

    public override void Perform(Creature creature, float time, float deltaTime)
    {
        var targetDirection = (transform.position - Fear.lastScarePosition).normalized;
        swimBehaviour.SwimTo(transform.position + targetDirection * (swimVelocity * 5f), swimVelocity);
    }
}