using Nautilus.Utility.ModMessages;

namespace WeatherMod.MessageReaders;

public class SetWeatherReader : ModMessageReader
{
    protected override void OnReceiveMessage(ModMessage message)
    {
        if (message.Subject != "SetWeather") return;
        
        var weatherName = (string)message.Contents[0];
        
        WeatherAPI.SetWeatherEvent(weatherName);
    }
}