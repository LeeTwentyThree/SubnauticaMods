using Nautilus.Utility.ModMessages;

namespace WeatherMod.MessageReaders;

public class SetWeatherPausedReader : ModMessageReader
{
    protected override void OnReceiveMessage(ModMessage message)
    {
        if (message.Subject != "SetWeatherPaused") return;
        
        var paused = (bool)message.Contents[1];
        
        WeatherAPI.SetWeatherPaused(paused);
    }
}