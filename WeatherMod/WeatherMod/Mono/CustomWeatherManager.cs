using System.Collections.Generic;
using UnityEngine;
using WeatherMod.WeatherEvents;
using Random = UnityEngine.Random;

namespace WeatherMod.Mono;

public class CustomWeatherManager : MonoBehaviour
{
    public static CustomWeatherManager Main;
    
    private float _weatherSeed;
    private float _timeNextWeatherChange;

    private WeatherSoundUpdater _soundUpdater;

    public WeatherEvent CurrentEvent { get; private set; }

    internal static readonly List<WeatherEvent> ActiveWeatherEvents = new() {
        new ClearSkies(),
        new LightRain(),
        new Thunderstorm(),
        new Foggy(),
        new Windy()
    };
    
    internal static readonly List<WeatherEvent> WeatherEvents = new() {
        new ClearSkies(),
        new LightRain(),
        new Thunderstorm(),
        new GoldenThunderstorm(),
        new Foggy(),
        new Windy()
    };
    
    private void Awake()
    {
        Main = this;
        
        _weatherSeed = Random.value * 9999;

        _soundUpdater = gameObject.EnsureComponent<WeatherSoundUpdater>();
        
        SetWeather(GetRandomWeatherEvent());
    }

    public void SetWeather(WeatherEvent newEvent)
    {
        if (CurrentEvent != null && CurrentEvent.GetType() == newEvent.GetType())
        {
            return;
        }
        
        CurrentEvent?.EndEvent();

        newEvent.BeginEvent();

        CurrentEvent = newEvent;
        
        _soundUpdater.SetActiveAudio(newEvent.AmbientSound);
        
        _timeNextWeatherChange = Time.time + Random.Range(newEvent.MinDuration, newEvent.MaxDuration);
    }
    
    private WeatherEvent GetRandomWeatherEvent()
    {
        return ActiveWeatherEvents[Random.Range(0, ActiveWeatherEvents.Count)];
    }

    private void Update()
    {
        if (Time.time > _timeNextWeatherChange)
        {
            SetWeather(GetRandomWeatherEvent());
        }
    }
}