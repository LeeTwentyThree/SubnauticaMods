using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace PodshellLeviathan.Mono;

public class PodshellVoice : MonoBehaviour, IManagedUpdateBehaviour
{
    private PodshellLeviathanBehavior _behavior;
    
    public FMOD_CustomEmitter emitter;
    public bool useScreenShake;
    public float maxScreenShakeDistance = 120;
    
    private float _timeNextSound;
    
    private float _timeIdleSoundAgain;
    private float _minIdleSoundDelay = 45;
    private float _maxIdleSoundDelay = 90;

    private void Awake()
    {
        _behavior = GetComponent<PodshellLeviathanBehavior>();
        ResetIdleSoundDelay();
    }
    
    private void ResetIdleSoundDelay() => _timeIdleSoundAgain = Time.time + Random.Range(_minIdleSoundDelay, _maxIdleSoundDelay);

    private bool CanPlaySound() => Time.time > _timeNextSound;
    
    public bool PlaySound(FMODAsset asset, float cooldown, float screenShakeDuration = 0f, bool allowOverride = false)
    {
        if (!CanPlaySound() && !allowOverride)
            return false;
        if (useScreenShake && screenShakeDuration > 0)
        {
            var intensityScale = Mathf.Lerp(0, 1, Mathf.InverseLerp(maxScreenShakeDistance, 0,
                Vector3.Distance(transform.position, MainCamera.camera.transform.position)));
            ShakeScreen(screenShakeDuration, intensityScale);
        }
        emitter.SetAsset(asset);
        emitter.Play();
        _timeNextSound = Time.time + cooldown;
        return true;
    }

    public void OnKill()
    {
        FMODUWE.PlayOneShot(ModAudio.Death, transform.position);
    }
    
    public void ManagedUpdate()
    {
        if (!_behavior.liveMixin.IsAlive())
            return;

        if (Time.time > _timeIdleSoundAgain)
        {
            PlaySound(ModAudio.Idle, 6);
            _behavior.GetAnimator().SetTrigger("small_roar");
            ResetIdleSoundDelay();
        }
    }

    private void OnEnable()
    {
        BehaviourUpdateUtils.Register(this);
    }

    private void OnDisable()
    {
        BehaviourUpdateUtils.Deregister(this);
    }

    private static void ShakeScreen(float duration, float intensityScale)
    {
        MainCameraControl.main.ShakeCamera(4 * intensityScale, duration, shakeFrequency: 0.5f,
            mode: MainCameraControl.ShakeMode.Quadratic);
    }

    public bool PlayRoarSound(bool longRoar)
    {
        var distToCamera = Vector3.Distance(transform.position, MainCamera.camera.transform.position);
        var close = distToCamera < 80 && Player.main.GetCurrentSub() == null;
        if (longRoar)
            return PlaySound(close ? ModAudio.LongRoarClose : ModAudio.LongRoarFar, 8, 7);
        else
            return PlaySound(close ? ModAudio.ShortRoarClose : ModAudio.ShortRoarFar, 7, 6);
    }

    public string GetProfileTag()
    {
        return "Podshell:PodshellVoice";
    }

    public int managedUpdateIndex { get; set; }
}