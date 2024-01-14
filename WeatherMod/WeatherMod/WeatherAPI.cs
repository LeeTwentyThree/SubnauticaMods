using System;
using WeatherMod.Mono;

namespace WeatherMod;

public static class WeatherAPI
{
    public static void RegisterWeatherEvent(WeatherEvent newWeatherEvent, bool canOccurNaturally)
    {
        CustomWeatherManager.WeatherEvents.Add(newWeatherEvent);
        if (canOccurNaturally)
            CustomWeatherManager.ActiveWeatherEvents.Add(newWeatherEvent);
    }
    
    public static void SetWeatherEvent(string eventName)
    {
        var weatherManager = CustomWeatherManager.Main;
        
        if (weatherManager == null)
            return;
        
        WeatherEvent weatherEvent = null;
        foreach (var evt in CustomWeatherManager.WeatherEvents)
        {
            if (string.Equals(evt.GetType().Name, eventName, StringComparison.CurrentCultureIgnoreCase))
            {
                weatherEvent = evt;
            }
        }

        if (weatherEvent != null)
        {
            weatherManager.SetWeather(weatherEvent);
        }
    }

    public static void SetWeatherPaused(bool paused)
    {
        var weatherManager = CustomWeatherManager.Main;

        if (weatherManager != null)
            weatherManager.enabled = !paused;
    }
}