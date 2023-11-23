using Nautilus.Utility;

namespace WeatherMod;

public static class WeatherAudio
{
    public static readonly ModSound[] ThunderSoundsNear = new[]
    {
        new ModSound("ThunderNear_1"),
        new ModSound("ThunderNear_2"),
        new ModSound("ThunderNear_3")
    };
    
    public static readonly ModSound[] ThunderSoundsFar = new[]
    {
        new ModSound("ThunderFar_1"),
        new ModSound("ThunderFar_2"),
        new ModSound("ThunderFar_3")
    };

    public static ModSound ThunderstormLoop { get; } = new ModSound("ThunderstormLoop2", "ThunderstormLoop");
    public static ModSound WindyLoop { get; } = new ModSound("WindLoop_Heavy");
    public static ModSound LightRainLoop { get; } = new ModSound("LightRainLoop");
    public static ModSound GoldenThunderstormLoop { get; } = new ModSound("GoldenThunderstormLoop");
    
    public static void RegisterAll()
    {
        ThunderSoundsNear.ForEach(s => s.Register(AudioUtils.BusPaths.SurfaceAmbient, 10, 400));
        ThunderSoundsFar.ForEach(s => s.Register(AudioUtils.BusPaths.SurfaceAmbient, 40, 1000));
        
        ThunderstormLoop.Register(AudioUtils.BusPaths.SurfaceAmbient, -1f, -1f);
        WindyLoop.Register(AudioUtils.BusPaths.SurfaceAmbient, -1f, -1f);
        LightRainLoop.Register(AudioUtils.BusPaths.SurfaceAmbient, -1f, -1f);
        GoldenThunderstormLoop.Register(AudioUtils.BusPaths.PlayerSFXs, -1f, -1f);
    }
}