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

    public WeatherEvent CurrentEvent { get; private set; }

    public static readonly List<WeatherEvent> WeatherEvents = new() {
        new ClearSkies(),
        new LightRain(),
        new Thunderstorm(),
        new Foggy()
    };
    
    private void Awake()
    {
        Main = this;
        
        _weatherSeed = Random.value * 9999;
        SetWeather(GetRandomWeatherEvent());
    }

    public void SetWeather(WeatherEvent newEvent)
    {
        if (CurrentEvent == newEvent)
        {
            return;
        }
        
        CurrentEvent?.EndEvent();

        newEvent.BeginEvent();

        CurrentEvent = newEvent;
        
        _timeNextWeatherChange = Time.time + Random.Range(newEvent.MinDuration, newEvent.MaxDuration);
    }
    
    private WeatherEvent GetRandomWeatherEvent()
    {
        return WeatherEvents[Random.Range(0, WeatherEvents.Count)];
    }

    private void Update()
    {
        if (Time.time > _timeNextWeatherChange)
        {
            SetWeather(GetRandomWeatherEvent());
        }
    }
}