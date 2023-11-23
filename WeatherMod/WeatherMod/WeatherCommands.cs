using System;
using Nautilus.Commands;
using UnityEngine;
using WeatherMod.Mono;

namespace WeatherMod;

public static class WeatherCommands
{
    [ConsoleCommand("setfog")]
    public static void SetFog(float r, float g, float b, float density = 0.002f, float waterBrightness = 1f, float sunlightBrightnessAboveWater = 1f, float sunlightBrightnessBelowWater = 1f)
    {
        var fogSettings = new FogSettings(density, new Color(r, g, b), waterBrightness, sunlightBrightnessAboveWater, sunlightBrightnessBelowWater);
        FogManager.ChangeCurrentFog(fogSettings);
        ErrorMessage.AddMessage("Updated the fog");
    }
    
    [ConsoleCommand("pauseweather")]
    public static void PauseWeather()
    {
        ErrorMessage.AddMessage(!CustomWeatherManager.Main.enabled ? "The weather was already paused" : "Paused the weather");
        CustomWeatherManager.Main.enabled = false;
    }
    
    [ConsoleCommand("unpauseweather")]
    public static void UnpauseWeather()
    {
        ErrorMessage.AddMessage(CustomWeatherManager.Main.enabled ? "The weather was already unpaused" : "Unpaused the weather");
        CustomWeatherManager.Main.enabled = true;
    }
    
    [ConsoleCommand("setweather")]
    public static void SetWeather(string eventName = "")
    {
        var weatherManager = CustomWeatherManager.Main;

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
            ErrorMessage.AddMessage($"Setting weather to: {targetEvent.GetType().Name}");
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

    [ConsoleCommand("printweather")]
    public static void PrintWeather()
    {
        var weatherManager = CustomWeatherManager.Main;

        if (weatherManager == null)
        {
            ErrorMessage.AddMessage("No weather manager found in scene!");
            return;
        }
        
        ErrorMessage.AddMessage($"Current weather: {weatherManager.CurrentEvent.GetType()}");
    }
    
    [ConsoleCommand("setwavelengths")]
    public static void SetWavelengths(float r, float g, float b)
    {
        var skyManager = uSkyManager.main;

        if (skyManager == null)
        {
            ErrorMessage.AddMessage("No uSkyManager found in scene!");
            return;
        }

        uSkyManager.main.Wavelengths = new Vector3(r, g, b);
    }
    
    [ConsoleCommand("setplanetdistance")]
    public static void SetPlanetDistance(float distance)
    {
        var skyManager = uSkyManager.main;

        if (skyManager == null)
        {
            ErrorMessage.AddMessage("No uSkyManager found in scene!");
            return;
        }

        ErrorMessage.AddMessage($"Changing planet distance from {skyManager.planetDistance} to {distance}!");

        uSkyManager.main.planetDistance = distance;
    }
}