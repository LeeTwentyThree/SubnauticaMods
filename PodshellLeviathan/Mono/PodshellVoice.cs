using UnityEngine;

namespace PodshellLeviathan.Mono;

public class PodshellVoice : MonoBehaviour
{
    public FMOD_CustomEmitter emitter;
    public LiveMixin liveMixin;
    
    private float _timeNextSound;
    
    private float _timeRoarAgain;
    private float _minRoarDelay = 20;
    private float _maxRoarDelay = 35;
    
    private float _timeIdleSoundAgain;
    private float _minIdleSoundDelay = 15;
    private float _maxIdleSoundDelay = 35;

    private void Awake()
    {
        ResetRoarDelay();
        ResetIdleSoundDelay();
    }

    private void ResetRoarDelay() => _timeRoarAgain = Time.time + Random.Range(_minRoarDelay, _maxRoarDelay);
    
    private void ResetIdleSoundDelay() => _timeIdleSoundAgain = Time.time + Random.Range(_minIdleSoundDelay, _maxIdleSoundDelay);

    private bool CanPlaySound() => Time.time > _timeNextSound;
    
    private bool PlaySound(FMODAsset asset, float cooldown, bool allowOverride = false)
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
        if (!liveMixin.IsAlive())
            return;
        
        if (Time.time > _timeRoarAgain)
        {
            var distToCamera = Vector3.Distance(transform.position, MainCamera.camera.transform.position);
            var close = distToCamera < 60;
            var longRoar = Random.value <= 0.5f;
            if (longRoar)
                PlaySound(close ? ModAudio.LongRoarClose : ModAudio.LongRoarFar, 8);
            else
                PlaySound(close ? ModAudio.ShortRoarClose : ModAudio.ShortRoarFar, 7);
            ResetRoarDelay();
            return;
        }

        if (Time.time > _timeIdleSoundAgain)
        {
            PlaySound(ModAudio.Idle, 6);
            ResetIdleSoundDelay();
        }
    }
}