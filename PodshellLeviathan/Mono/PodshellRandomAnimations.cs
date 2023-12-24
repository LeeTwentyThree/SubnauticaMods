using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace PodshellLeviathan.Mono;

public class PodshellRandomAnimations : MonoBehaviour
{
    private PodshellLeviathanBehavior _behavior;

    private float _timeNextAction;
    private float _minActionDelay = 20f;
    private float _maxActionDelay = 30f;

    private PodshellAnimation[] _animations = new[]
    {
        // Roar animations are specially handled because they vary by distance
        new PodshellAnimation("big_roar", -1, null, AnimationType.LongRoar),
        new PodshellAnimation("small_roar", -1, null, AnimationType.Roar),
        new PodshellAnimation("cleaning", 11, null, AnimationType.Cleaning),
        new PodshellAnimation("teeth_grinding", 3f, ModAudio.TeethGrinding, AnimationType.Cleaning),
    };

    private PodshellAnimation _lastAnimation;

    private void Awake()
    {
        _behavior = GetComponent<PodshellLeviathanBehavior>();
    }

    private void Start()
    {
        ResetTimer();
    }

    private void ResetTimer() => _timeNextAction = Time.time + Random.Range(_minActionDelay, _maxActionDelay);

    private void Update()
    {
        if (!_behavior.liveMixin.IsAlive())
            return;
        
        if (Time.time > _timeNextAction)
        {
            PlayNextAnimation();
            ResetTimer();
        }
    }

    private void PlayNextAnimation()
    {
        var animationToPlay = AttemptToGetUniqueAnimation();
        PlayAnimation(animationToPlay);
        _lastAnimation = animationToPlay;
    }

    private void PlayAnimation(PodshellAnimation animation)
    {
        _behavior.GetAnimator().SetTrigger(animation.Trigger);
        if (animation.AnimationType == AnimationType.Roar || animation.AnimationType == AnimationType.LongRoar)
        {
            _behavior.voice.PlayRoarSound(animation.AnimationType == AnimationType.LongRoar);
        }
        else
        {
            if (animation.SoundAsset != null) _behavior.voice.PlaySound(animation.SoundAsset, animation.Duration);
        }
    }

    private PodshellAnimation AttemptToGetUniqueAnimation()
    {
        for (var i = 0; i < 10; i++)
        {
            var randomAnimation = _animations[Random.Range(0, _animations.Length)];
            if (randomAnimation != _lastAnimation)
                return randomAnimation;
        }

        return _animations[Random.Range(0, _animations.Length)];
    }

    private class PodshellAnimation
    {
        public string Trigger { get; }
        public float Duration { get; }
        public FMODAsset SoundAsset { get; }
        public AnimationType AnimationType { get; }

        public PodshellAnimation(string trigger, float duration, FMODAsset soundAsset, AnimationType animationType)
        {
            Trigger = trigger;
            Duration = duration;
            SoundAsset = soundAsset;
            AnimationType = animationType;
        }
    }

    private enum AnimationType
    {
        Roar,
        LongRoar,
        GrindTeeth,
        Cleaning
    }
}