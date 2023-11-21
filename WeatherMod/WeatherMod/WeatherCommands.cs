using System;
using Nautilus.Commands;
using UnityEngine;
using WeatherMod.Mono;

namespace WeatherMod;

public static class WeatherCommands
{
    [ConsoleCommand("setfog")]
    public static void SetFog(float r, float g, float b, float density = 0.002f, float waterBrightness = 1f)
    {
        var fogSettings = new FogSettings(density, new Color(r, g, b));
        fogSettings.WaterBrightness = waterBrightness;
        FogManager.ChangeCurrentFog(fogSettings);
        ErrorMessage.AddMessage("Updated the fog");
    }
    
    [ConsoleCommand("pauseweather")]
    public static void PauseWeather()
    {
        ErrorMessage.AddMessage(!CustomWeatherManager.main.enabled ? "The weather was already paused" : "Paused the weather");
        CustomWeatherManager.main.enabled = false;
    }
    
    [ConsoleCommand("unpauseweather")]
    public static void UnpauseWeather()
    {
        ErrorMessage.AddMessage(CustomWeatherManager.main.enabled ? "The weather was already unpaused" : "Unpaused the weather");
        CustomWeatherManager.main.enabled = true;
    }
    
    [ConsoleCommand("setweather")]
    public static void SetWeather(string eventName = "")
    {
        var weatherManager = CustomWeatherManager.main;

        if (weatherManager == null)
        {
            ErrorMessage.AddMessage("No weather manager found in scene!");
            return;
        }
        
        WeatherEvent targetEvent = null;
        if (!string.IsNullOrEmpty(eventName))
        {
            foreach (var evt in CustomWeatherManager.WeatherEvents)
            {
                if (string.Equals(evt.GetType().Name, eventName, StringComparison.CurrentCultureIgnoreCase))
                {
                    targetEvent = evt;
                }
            }
        }

        if (targetEvent != null)
        {
            weatherManager.SetWeather(targetEvent);
        }
        else
        {
            ErrorMessage.AddMessage("Valid weather events:");
            foreach (var evt in CustomWeatherManager.WeatherEvents)
            {
                ErrorMessage.AddMessage(evt.GetType().Name.ToLower());
            }
        }
    }
}