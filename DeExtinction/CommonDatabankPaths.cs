namespace DeExtinction;

public static class CommonDatabankPaths
{
#if SUBNAUTICA
    public const string SmallHerbivores = "Lifeforms/Fauna/SmallHerbivores";
#elif BELOWZERO
    public const string SmallHerbivores = "EncyPath_Research/Lifeforms/Fauna/SmallHerbivores";
#endif
    
#if SUBNAUTICA
    public const string LargeHerbivores = "Lifeforms/Fauna/LargeHerbivores";
#elif BELOWZERO
    public const string LargeHerbivores = "EncyPath_Research/Lifeforms/Fauna/LargeHerbivores";
#endif
    
#if SUBNAUTICA
    public const string Scavengers = "Lifeforms/Fauna/Scavengers";
#elif BELOWZERO
    public const string Scavengers = "EncyPath_Research/Lifeforms/Fauna/Scavengers";
#endif
    
#if SUBNAUTICA
    public const string Leviathans = "Lifeforms/Fauna/Leviathans";
#elif BELOWZERO
    public const string Leviathans = "EncyPath_Research/Lifeforms/Fauna/Leviathans";
#endif
    
#if SUBNAUTICA
    public const string Carnivores = "Lifeforms/Fauna/Carnivores";
#elif BELOWZERO
    public const string Carnivores = "EncyPath_Research/Lifeforms/Fauna/Carnivores";
#endif
}