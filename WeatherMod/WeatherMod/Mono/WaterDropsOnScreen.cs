using System;
using UnityEngine;

namespace WeatherMod.Mono;

public class WaterDropsOnScreen : MonoBehaviour
{
    private ParticleSystem _particleSystem;
    // private Material _material;
    private float _lastRateOverTime;

    private void Awake()
    {
        _particleSystem = GetComponent<ParticleSystem>();
        _particleSystem.Play();
        // _material = GetComponent<Renderer>().material;

        _lastRateOverTime = GetRateOverTime();
        SetRateOverTime(_lastRateOverTime);
    }

    private static float GetRateOverTime()
    {
        if (Player.main.IsUnderwater() || Player.main.IsInside()) return 0;
        if (CustomWeatherManager.Main == null) return 0;
        if (CustomWeatherManager.Main.CurrentEvent == null) return 0;
        return CustomWeatherManager.Main.CurrentEvent.RainDropVfxEmission;
    }

    private void Update()
    {
        var rateOverTime = GetRateOverTime();
        if (rateOverTime != _lastRateOverTime)
        {
            _lastRateOverTime = rateOverTime;
            SetRateOverTime(rateOverTime);
        }
    }

    private void SetRateOverTime(float newRate)
    {
        var emissionModule = _particleSystem.emission;
        emissionModule.rateOverTime = newRate;
    }
}