using Nautilus.Utility;
using UnityEngine;

namespace TheRedPlague.Mono.CreatureBehaviour.MrTeeth;

public class MrTeethBuryBehaviour : CreatureAction
{
    public float buryDelay = 3f;
    public float swimVelocity = 8f;
    public float buryDistance = 6.5f;
    private float _spawnTime;

    private Vector3 _targetPos;

    private bool _buried;
    
    private static readonly FMODAsset BurySound = AudioUtils.GetFmodAsset("MrTeethBury");

    private void Start()
    {
        _spawnTime = Time.time;
    }

    public override float Evaluate(Creature creature, float time)
    {
        if (Time.time < _spawnTime + buryDelay) return 0;
        if (Player.main.transform.position.y < 0) return 0;
        return base.Evaluate(creature, time);
    }

    public override void StartPerform(Creature creature, float time)
    {
        var spawnPoint = MrTeethSpawnPoint.GetRandom();
        if (spawnPoint == null)
        {
            Destroy(gameObject);
            return;
        }

        _targetPos = spawnPoint.transform.position;
    }

    public override void Perform(Creature creature, float time, float deltaTime)
    {
        if (_buried) return;
        swimBehaviour.SwimTo(_targetPos, swimVelocity);
        if (Vector3.Distance(transform.position, _targetPos) > buryDistance) return;
        swimBehaviour.splineFollowing.useRigidbody.isKinematic = true;
        _buried = true;
        creature.GetAnimator().SetTrigger("bury");
        Utils.PlayFMODAsset(BurySound, transform.position);
        Destroy(gameObject, 3f);
    }
}