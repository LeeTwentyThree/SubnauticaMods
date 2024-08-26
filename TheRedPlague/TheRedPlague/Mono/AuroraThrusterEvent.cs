using System;
using System.Collections;
using Nautilus.Utility;
using UnityEngine;
using UnityEngine.PostProcessing;

namespace TheRedPlague.Mono;

public class AuroraThrusterEvent : MonoBehaviour
{
    private readonly Vector3[] _firePositions = {
        new(469.71f, 131.95f, -362.83f),
        new(795.65f, 56.73f, -712.86f),
        new(778.29f, 208.04f, -687.50f)
    };

    private readonly Vector3 _fireDirection = new Vector3(-0.7f, 0.1f, -0.7f);

    private ParticleSystem[] _fireParticles;

    private static readonly FMODAsset Sound = AudioUtils.GetFmodAsset("AuroraThrusterEvent");

    private float _startTime;

    private readonly ScreenShakeMoment[] _screenShakeMoments = new[]
    {
        new ScreenShakeMoment(0, 6, 0.2f, MainCameraControl.ShakeMode.Quadratic, 0.2f),
        new ScreenShakeMoment(6, 7, 1, MainCameraControl.ShakeMode.BuildUp, 0.7f),
        new ScreenShakeMoment(11.5f, 10, 1.9f, MainCameraControl.ShakeMode.BuildUp, 1.1f),
        new ScreenShakeMoment(20, 10, 2f, MainCameraControl.ShakeMode.Cos, 1.3f),
        new ScreenShakeMoment(36, 7, 0.3f, MainCameraControl.ShakeMode.Sqrt, 0.8f),
    };
    
    public static void PlayCinematic()
    {
        var cinematic = new GameObject("AuroraFireCinematic").AddComponent<AuroraThrusterEvent>();
    }

    private IEnumerator Start()
    {
        StoryUtils.AuroraThrusterEvent.Trigger();
        
        _startTime = Time.time;
        var firePrefab = Plugin.AssetBundle.LoadAsset<GameObject>("AuroraEngineParticle");

        Utils.PlayFMODAsset(Sound, new Vector3(970.37f, 85.66f, -249.60f));
        
        _fireParticles = new ParticleSystem[_firePositions.Length];
        for (var i = 0; i < _firePositions.Length; i++)
        {
            var obj = Instantiate(firePrefab, _firePositions[i] - _fireDirection * 150, Quaternion.LookRotation(_fireDirection));
            _fireParticles[i] = obj.GetComponent<ParticleSystem>();
        }
        
        var auroraAnimator = CrashedShipExploder.main.transform.parent.gameObject.AddComponent<Animator>();
        auroraAnimator.runtimeAnimatorController = Plugin.AssetBundle.LoadAsset<RuntimeAnimatorController>("Aurora-MainPrefab");
        Destroy(auroraAnimator, 60);

        var explosionScreenFX = MainCamera.camera.GetComponent<ExplosionScreenFX>();
        yield return new WaitForSeconds(6f);
        explosionScreenFX.strength = 0.3f;
        explosionScreenFX.enabled = true;
        yield return new WaitForSeconds(4);
        AuroraTentacleBehavior.GrabAll();
        yield return new WaitForSeconds(10);
        explosionScreenFX.strength = 0.2f;
        yield return new WaitForSeconds(6);
        explosionScreenFX.enabled = false;

        yield break;
    }

    private void Update()
    {
        var relativeTime = Time.time - _startTime;
        foreach (var screenShakeMoment in _screenShakeMoments)
        {
            screenShakeMoment.OnUpdate(relativeTime);
        }
    }

    private class ScreenShakeMoment
    {
        private readonly  float _time;
        private readonly float _duration;
        private readonly float _intensity;
        private readonly float _frequency;
        private readonly MainCameraControl.ShakeMode _shakeMode;
        
        private bool _complete;

        public ScreenShakeMoment(float time, float duration, float intensity, MainCameraControl.ShakeMode shakeMode, float frequency)
        {
            _time = time;
            _duration = duration;
            _intensity = intensity;
            _shakeMode = shakeMode;
            _frequency = frequency;
        }

        public void OnUpdate(float relativeTime)
        {
            if (_complete) return;
            if (relativeTime < _time) return;
            MainCameraControl.main.ShakeCamera(_intensity, _duration, _shakeMode, _frequency);
            _complete = true;
        }
    }
}