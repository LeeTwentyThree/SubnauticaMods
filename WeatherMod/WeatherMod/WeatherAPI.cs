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
}