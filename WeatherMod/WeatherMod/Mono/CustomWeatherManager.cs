using UnityEngine;
using WeatherMod.WeatherEvents;
using Random = UnityEngine.Random;

namespace WeatherMod.Mono;

public class CustomWeatherManager : MonoBehaviour
{
    public static CustomWeatherManager main;
    
    private float _weatherSeed;
    private float _timeNextWeatherChange;

    private WeatherEvent _currentEvent;

    public static readonly WeatherEvent[] WeatherEvents = {
        new LightRain()
    };
    
    private void Awake()
    {
        main = this;
        
        _weatherSeed = Random.value * 9999;
        SetWeather(GetRandomWeatherEvent());
    }

    public void SetWeather(WeatherEvent newEvent)
    {
        if (_currentEvent == newEvent)
        {
            return;
        }
        
        _currentEvent?.EndEvent();

        newEvent.BeginEvent();
        
        _timeNextWeatherChange = Time.time + Random.Range(newEvent.MinDuration, newEvent.MaxDuration);
    }

    private WeatherEvent GetRandomWeatherEvent()
    {
        return WeatherEvents[Random.Range(0, WeatherEvents.Length)];
    }

    private void Update()
    {
        if (Time.time > _timeNextWeatherChange)
        {
            SetWeather(GetRandomWeatherEvent());
        }
    }
}