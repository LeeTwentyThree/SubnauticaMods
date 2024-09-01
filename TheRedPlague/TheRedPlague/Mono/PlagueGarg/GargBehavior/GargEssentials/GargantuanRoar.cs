using Nautilus.Handlers;

namespace TheRedPlague.Mono.PlagueGarg.GargBehavior.GargEssentials;

using System.Runtime.InteropServices;
using FMOD;
using FMODUnity;
using UnityEngine;
using UnityEngine.Events;
using static GargantuanConditions;

public class GargantuanRoar : MonoBehaviour
{
    public float delayMin = 11f;
    public float delayMax = 18f;
    public float closeRoarThreshold = 150f;
    public bool producesChromaticAberration = false;
    public FMOD_CustomEmitter closeEmitter;
    public FMOD_CustomEmitter farEmitter;
    public RoarMode roarMode;
    public bool toldToShutUp;

    private FMOD_CustomEmitter _grandRoar;
    
    public GargantuanBehaviour gargantuanBehaviour;
    private Channel _channel;
    private Creature _creature;
    private DSP _fft;
    private DSP_PARAMETER_FFT _fftData;
    private float[][] _spectrumData;
    private float[] _spectrum = new float[128];
    private float _indoorsScreenShakeScale = 0.2f;

    private int _length;
    public bool screenShake;
    public bool roarDoesDamage;

    private float _timeRoarAgain;
    private float _timeUpdateShakeAgain;
    
    private float _clipLoudness;

    private const float kMaxDamageDistance = 200f;
    private const float kRoarMaxDamagePerSecond = 6f;
    private const float kScreenShakeScale = 1f;
    private float _timeStopDamaging = 0f;
    private bool _hasRoaredOnce;
    private bool _wasInside;
    private ExplosionScreenFX _chromaticAberrationFX;

    private float _chromaticAberrationMultiplier = 0.25f; // chromatic aberration = clip loudness * multiplier
    private float _chromaticAberrationMax = 0.4f;

    public bool IsRoaring => _channel.isPlaying(out var playing) == RESULT.OK && playing;

    private void Start()
    {
        RuntimeManager.CoreSystem.createDSPByType(DSP_TYPE.FFT, out _fft);
        _fft.setParameterInt((int)DSP_FFT.WINDOWTYPE, (int)FMOD.DSP_FFT_WINDOW.RECT);
        _fft.setParameterInt((int)DSP_FFT.WINDOWSIZE, 1024);
        InitializeEmitters();
        _creature = GetComponent<Creature>();
    }

    private void Update()
    {
        if (!_creature.liveMixin.IsAlive())
        {
            Destroy(this);
            return;
        }
        _wasInside = !PlayerIsKillable();
        if (toldToShutUp)
        {
            return;
        }
        if (Time.time > _timeRoarAgain)
        {
            PlayOnce(out _, roarMode);
        }
        
        if (screenShake)
        {
            if (Time.time > _timeUpdateShakeAgain && IsRoaring)
            {
                UpdateSpectrum();
                _clipLoudness = 0f;
                foreach (var sample in _spectrum)
                {
                    _clipLoudness += Mathf.Abs(sample);
                }

                if (_wasInside)
                {
                    _clipLoudness *= _indoorsScreenShakeScale;
                }

                if (_clipLoudness > 0.1f)
                {
                    MainCameraControl.main.ShakeCamera(_clipLoudness * kScreenShakeScale, 1f, MainCameraControl.ShakeMode.Linear, 1f);
                }
                _timeUpdateShakeAgain = Time.time + 0.5f;
            }
            UpdateChromaticAberration();
        }
        if (roarDoesDamage && _wasInside && Time.time < _timeStopDamaging)
        {
            DoDamage();
        }
    }

    private void UpdateChromaticAberration() // called every frame only while roaring
    {
        if (IsRoaring && producesChromaticAberration)
        {
            if (!_wasInside)
            {
                SetChromaticAberration(_clipLoudness * _chromaticAberrationMultiplier);
            }
            else
            {
                SetChromaticAberration(0f);
            }
        }
    }

    private void SetChromaticAberration(float strength)
    {
        if (_chromaticAberrationFX == null)
        {
            _chromaticAberrationFX = MainCamera.camera.GetComponent<ExplosionScreenFX>();
        }
        if (_chromaticAberrationFX == null)
        {
            return;
        }
        _chromaticAberrationFX.enabled = strength > 0.05f;
        _chromaticAberrationFX.strength = Mathf.Clamp(strength, 0f, _chromaticAberrationMax);
    }

    private void OnDestroy()
    {
        if (_fft.hasHandle())
        {
            _fft.release();
            _fft.clearHandle();
        }
    }

    public void PlayOnce(out float roarLength, RoarMode roarMode)
    {
        if (IsRoaring)
        {
            roarLength = 0f;
            return;
        }
        float distance = Vector3.Distance(MainCameraControl.main.transform.position, transform.position);
        roarLength = 0f;
        if (PlayFModSound(distance, roarMode))
        {
            _channel.addDSP(CHANNELCONTROL_DSP_INDEX.TAIL, _fft);
            if (_channel.getCurrentSound(out var sound) == RESULT.OK)
            {
                sound.getLength(out var length, TIMEUNIT.MS);
                roarLength = ToSeconds(length);
            }
        }

        _creature.GetAnimator().SetFloat("random", Random.value);
        _creature.GetAnimator().SetTrigger("roar");
        float timeToWait = roarLength + Random.Range(delayMin, delayMax);
        _timeRoarAgain = Time.time + timeToWait;
        _timeStopDamaging = Time.time + 6f;
        _hasRoaredOnce = true;
    }

    public void Stop()
    {
        closeEmitter.Stop();
        farEmitter.Stop();
        _grandRoar.Stop();
    }

    private void DoDamage()
    {
        return;
        float distance = Vector3.Distance(Player.main.transform.position, transform.position);
        if (distance < kMaxDamageDistance)
        {
            float distanceScalar = Mathf.Clamp(1f - (distance / kMaxDamageDistance), 0.01f, 1f);
            Player.main.liveMixin.TakeDamage(distanceScalar * Time.deltaTime * kRoarMaxDamagePerSecond, transform.position, DamageType.Normal, gameObject);
        }
    }

    public void DelayTimeOfNextRoar(float length)
    {
        _timeRoarAgain = Mathf.Max(Time.time + length, _timeRoarAgain);
    }

    private bool PlayFModSound(float distance, RoarMode roarMode)
    {
        switch (roarMode)
        {
            case RoarMode.CloseOnly:
                closeEmitter.Play();
                return CustomSoundHandler.TryGetCustomSoundChannel(closeEmitter.GetInstanceID(), out _channel);
            case RoarMode.FarOnly:
                farEmitter.Play();
                return CustomSoundHandler.TryGetCustomSoundChannel(farEmitter.GetInstanceID(), out _channel);
            default:
            {
                FMOD_CustomEmitter emitter;
                if (distance < closeRoarThreshold && PlayerIsKillable())
                {
                    emitter = closeEmitter != null ? closeEmitter : farEmitter;
                    emitter.Play();
                    return CustomSoundHandler.TryGetCustomSoundChannel(emitter.GetInstanceID(), out _channel);
                }

                emitter = farEmitter != null ? farEmitter : closeEmitter;
                emitter.Play();
                return CustomSoundHandler.TryGetCustomSoundChannel(emitter.GetInstanceID(), out _channel);
            }
        }
    }

    private void InitializeEmitters()
    {
        _grandRoar = gameObject.AddComponent<FMOD_CustomEmitter>();
        _grandRoar.followParent = true;
        _grandRoar.playOnAwake = false;
    }

    #region Ugly FMOD spectrum stuff
    private void UpdateSpectrum()
    {
        if (!_channel.hasHandle() || _channel.isPlaying(out var playing) != RESULT.OK || !playing)
            return;
        
        _fft.getParameterData((int)DSP_FFT.SPECTRUMDATA, out var ptr, out _);
        _fftData = Marshal.PtrToStructure<DSP_PARAMETER_FFT>(ptr);
        _spectrumData = _fftData.spectrum;
        var numChannels = _fftData.numchannels;
        _length = _fftData.length;
        ResizeIfNeeded(_length);
        if (numChannels > 0)
        {
            for (var i = 0; i < _length; i++)
            {
                float sampleData = 0;
                for (int j = 0; j < numChannels; j++)
                {
                    var level = _spectrumData[j][i];
                    sampleData += level;
                }
                
                _spectrum[i] = sampleData;
            }
        }
    }

    private float ToSeconds(uint milliseconds)
    {
        return (float)milliseconds / 1000f;
    }

    private void ResizeIfNeeded(int length)
    {
        if (length > _spectrum.Length)
        {
            _spectrum = new float[length];
        }
    }
    #endregion

    public enum RoarMode
    {
        /// <summary>
        /// Always choose a "normal" or "close" roar
        /// </summary>
        CloseOnly = 1,
        /// <summary>
        /// Always choose a "distant" roar
        /// </summary>
        FarOnly = 2,
        /// <summary>
        /// Determine roar sound based on distance
        /// </summary>
        Automatic = CloseOnly | FarOnly,
    }
}