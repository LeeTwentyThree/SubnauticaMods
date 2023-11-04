namespace TheRumbling;

internal static class Balance
{
    // Heat simulation
    public const float MaxTemperature = 500f;
    public const float TemperatureFalloffDistance = 360;
    public const float DistortionEffectMaxDistance = 100;
    public const float HeatDistortionEffectStrength = 2;

    // Rumbling logistics
    public const float SpawnDistance = 1000;
    public const float HorizontalDistanceBetweenTitans = 90;
    public const int FormationRows = 10;
    public const int FormationUnitsPerRow = 30;
    public const int FormationAdditionUnitsPerEachRow = 1;
    public const float DistanceBetweenRows = 75;
    public const float DefaultTitanWalkSpeed = 7;
    public const float DefaultTitanScale = 60;
    // the y position at which the titans swim
    public const float DefaultTitanSwimYMin = -52;
    public const float DefaultTitanSwimYMax = -48;
    public const float DefaultTitanSpawnY = -60;
}