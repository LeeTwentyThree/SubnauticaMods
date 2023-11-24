namespace WeatherMod;

public class WeatherEventAudio
{
    public ModSound NormalAmbience { get; }
    public ModSound InsideOnlyAmbience { get; }
    public ModSound JustBelowAmbience { get; }
    

    public WeatherEventAudio(ModSound normalAmbience, ModSound insideOnlyAmbience, ModSound justBelowAmbience)
    {
        NormalAmbience = normalAmbience;
        InsideOnlyAmbience = insideOnlyAmbience;
        JustBelowAmbience = justBelowAmbience;
    }
}