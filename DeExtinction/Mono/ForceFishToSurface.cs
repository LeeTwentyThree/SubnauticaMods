using UnityEngine;

namespace DeExtinction.Mono;

public class ForceFishToSurface : CreatureAction
{
    private float _swimSpeed;

    private void Start()
    {
        var swimRandom = GetComponent<SwimRandom>();
        _swimSpeed = swimRandom != null ? swimRandom.swimVelocity : 3;
    }

#if SUBNAUTICA
    public override void Perform(Creature creature, float time, float deltaTime)
#elif BELOWZERO
    public override void Perform(float time, float deltaTime)
#endif
    {
        if (swimBehaviour == null) return;
        swimBehaviour.SwimTo(new Vector3(transform.position.x, Ocean.GetOceanLevel(), transform.position.z), _swimSpeed);
    }
}