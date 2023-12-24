using UnityEngine;

namespace PodshellLeviathan.Mono;

public class PodshellVoice : MonoBehaviour
{
    private PodshellLeviathanBehavior _behavior;
    
    public FMOD_CustomEmitter emitter;
    
    private float _timeNextSound;
    
    private float _timeIdleSoundAgain;
    private float _minIdleSoundDelay = 15;
    private float _maxIdleSoundDelay = 35;

    private void Awake()
    {
        _behavior = GetComponent<PodshellLeviathanBehavior>();
        ResetIdleSoundDelay();
    }
    
    private void ResetIdleSoundDelay() => _timeIdleSoundAgain = Time.time + Random.Range(_minIdleSoundDelay, _maxIdleSoundDelay);

    private bool CanPlaySound() => Time.time > _timeNextSound;
    
    public bool PlaySound(FMODAsset asset, float cooldown, bool allowOverride = false)
    {
        if (!CanPlaySound() && !allowOverride)
            return false;
        emitter.SetAsset(asset);
        emitter.Play();
        _timeNextSound = Time.time + cooldown;
        return true;
    }

    private void OnKill()
    {
        PlaySound(ModAudio.Death, 16.96348f, true);
    }
    
    private void Update()
    {
        if (!_behavior.liveMixin.IsAlive())
            return;

        if (Time.time > _timeIdleSoundAgain)
        {
            PlaySound(ModAudio.Idle, 6);
            ResetIdleSoundDelay();
        }
    }

    public bool PlayRoarSound(bool longRoar)
    {
        var distToCamera = Vector3.Distance(transform.position, MainCamera.camera.transform.position);
        var close = distToCamera < 60;
        if (longRoar)
            return PlaySound(close ? ModAudio.LongRoarClose : ModAudio.LongRoarFar, 8);
        else
            return PlaySound(close ? ModAudio.ShortRoarClose : ModAudio.ShortRoarFar, 7);
    }
}