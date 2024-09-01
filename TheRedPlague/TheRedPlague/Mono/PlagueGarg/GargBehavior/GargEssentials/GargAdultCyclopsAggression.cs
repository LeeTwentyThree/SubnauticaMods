using Nautilus.Utility;

namespace TheRedPlague.Mono.PlagueGarg.GargBehavior.GargEssentials;

using UnityEngine;

class GargAdultCyclopsAggression : CreatureAction
{
    SwimBehaviour _swimBehaviour;
    Creature _creature;
    GargantuanBehaviour _garg;

    float _aggressionLevel;
    const float kAggressionMinThreshold = 65f;

    SubRoot _cyclopsTargeting;
    bool _chasingCyclops;

    float _timeCooldownStarted;
    bool _onCooldown;
    bool _waitingForGrandRoar;
    const float kCooldownLength = 90f;

    const float kEvalPriority = 1f;

    public float chargeSpeed;

    private FMOD_CustomLoopingEmitter _cyclopsSirenEmitter;

    private const float kSilentRunningAggressionPercent = 0.4f;

    private static float _timeCyclopsVOCanPlayAgain;

    void Start()
    {
        _swimBehaviour = gameObject.GetComponent<SwimBehaviour>();
        _creature = gameObject.GetComponent<Creature>();
        _garg = gameObject.GetComponent<GargantuanBehaviour>();

        var sirenEmitterObj = new GameObject("Siren Loop");
        sirenEmitterObj.transform.SetParent(transform, false);
        _cyclopsSirenEmitter = sirenEmitterObj.AddComponent<FMOD_CustomLoopingEmitter>();
        _cyclopsSirenEmitter.SetAsset(AudioUtils.GetFmodAsset("event:/sub/cyclops/siren"));
    }

    private void PlayCyclopsVO()
    {
        if (Time.time < _timeCyclopsVOCanPlayAgain)
        {
            return;
        }
        _timeCyclopsVOCanPlayAgain = Time.time + 10f;
    }

    public override float Evaluate(Creature creature, float time)
    {
        if (_waitingForGrandRoar)
        {
            return 0f;
        }
        if (_chasingCyclops)
        {
            return kEvalPriority;
        }
        return 0f;
    }

    public override void Perform(Creature creature, float time, float deltaTime)
    {
        _swimBehaviour.SwimTo(_cyclopsTargeting.transform.position, chargeSpeed); // swim to the cyclops
        _creature.Aggression.Add(0.25f * deltaTime); // add aggression
    }

    private void EndChase()
    {
        // stop chasing the cyclops
        _chasingCyclops = false;
        _waitingForGrandRoar = false;
        // start the cooldown
        _onCooldown = true;
        _timeCooldownStarted = Time.time;
        _aggressionLevel = 0f;
        _cyclopsSirenEmitter.Stop();
    }

    void Update()
    {
        if (_onCooldown)
        {
            if (Time.time > _timeCooldownStarted + kCooldownLength)
            {
                _onCooldown = false;
            }
            return;
        }
        if (_cyclopsTargeting == null) // if not already targeting a cyclops
        {
            _cyclopsTargeting = Player.main.GetCurrentSub(); // try to find a cyclops to target
            if (_cyclopsTargeting != null && _cyclopsTargeting.isCyclops)
            {
                // if a cyclops was found, reset aggressionLevel to 0.
                _aggressionLevel = 0f;
            }
        }
        else if (_cyclopsTargeting.noiseManager != null && _cyclopsTargeting.noiseManager.noiseScalar > 0.1f)
        {
            float aggressionToAdd = _cyclopsTargeting.silentRunning ? kSilentRunningAggressionPercent : 1f;
            aggressionToAdd *= Time.deltaTime;

            _aggressionLevel += aggressionToAdd;

            if (_aggressionLevel >= kAggressionMinThreshold)
            {
                // if the aggression has reached the threshold, start chasing
                _chasingCyclops = true;
            }

        }

        // if currently chasing cyclops and initiating the grab attack, exit chase mode and set the cooldown
        if (_chasingCyclops)
        {
            if (GargantuanConditions.IgnoreCyclops() || _garg.grab.IsHoldingLargeSub())
            {
                EndChase();
                _chasingCyclops = false;
            }
        }
    }
}