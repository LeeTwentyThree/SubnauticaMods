using System;
using UnityEngine;

namespace WeatherMod.Mono;

public class LoopingAudioPlayer : MonoBehaviour
{
    private FMOD_CustomEmitter _emitter;

    private float _secondsUntilOver;

    private ModSound _currentSound;
    
    private void Awake()
    {
        _emitter = gameObject.EnsureComponent<FMOD_CustomEmitter>();
    }

    public void BeginPlayLoopingSound(ModSound sound)
    {
        if (_emitter.playing) _emitter.Stop();
        if (sound == null)
        {
            _currentSound = null;
            return;
        }

        if (sound == _currentSound)
        {
            return;
        }
        
        _secondsUntilOver = sound.Duration;
        _emitter.SetAsset(sound.Asset);
        _emitter.Play();
        _currentSound = sound;
    }

    private void Update()
    {
        if (_currentSound == null)
        {
            return;
        }
        
        if (Time.timeScale > 0f)
        {
            _secondsUntilOver -= Time.unscaledDeltaTime;
        }

        if (_secondsUntilOver <= 0f)
        {
            if (_emitter.playing) _emitter.Stop();
            _emitter.Play();
            _secondsUntilOver = _currentSound.Duration;
        }
    }
}