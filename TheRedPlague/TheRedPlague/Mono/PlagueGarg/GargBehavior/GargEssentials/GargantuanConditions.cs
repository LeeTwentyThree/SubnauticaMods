using System;
using System.Collections.Generic;
using UnityEngine;

namespace TheRedPlague.Mono.PlagueGarg.GargBehavior.GargEssentials;

// General Condition-based static class
internal static class GargantuanConditions
{
    public static bool CanSwallowWhole(GameObject gameObject, LiveMixin liveMixin)
    {
        if (liveMixin.health - DamageSystem.CalculateDamage(600f, DamageType.Normal, gameObject) <= 0)
        {
            return false;
        }
        if (gameObject.GetComponentInChildren<Player>()) // you dont want to destroy the player object EVER
        {
            return false;
        }
        if (gameObject.GetComponentInParent<Player>()) // further checking
        {
            return false;
        }
        if (gameObject.GetComponentInParent<Vehicle>()) // vehicles should be bitten or grabbed, not swallowed
        {
            return false;
        }
        if (gameObject.GetComponentInParent<SubRoot>()) // subs should not be swallowed either
        {
            return false;
        }
        if (liveMixin.maxHealth > 600f)
        {
            return false;
        }
        if (liveMixin.invincible)
        {
            return false;
        }
        return true;
    }
        
    public static bool IsVehicleOrSub(GameObject gameObject)
    {
        if (!gameObject)
        {
            return false;
        }
        if (gameObject.GetComponentInParent<Vehicle>())
        {
            return true;
        }
        if (gameObject.GetComponentInParent<SubRoot>())
        {
            return true;
        }
            
        return false;
    }
        
    public static bool PlayerIsKillable()
    {
        if (Player.main.GetCurrentSub() != null)
        {
            return false;
        }
        if (PlayerInPrecursorBase())
        {
            return false;
        }
        return true;

    }
        
    public static bool PlayerInPrecursorBase()
    {
        string biome = Player.main.GetBiomeString();
        if (biome.StartsWith("precursor", StringComparison.OrdinalIgnoreCase) || biome.StartsWith("prison", StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }
        return false;
    }

    public static bool PlayerInVoidBase()
    {
        string biome = Player.main.GetBiomeString();
        if (biome.StartsWith("precursorvoidbase", StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }
        return false;
    }

    public static bool PlayerInOCFRoom()
    {
        string biome = Player.main.GetBiomeString();
        if (biome.StartsWith("precursorvoidbaseocf", StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }
        return false;
    }

    public static bool IgnoreCyclops()
    {
        return false;
    }

    private static readonly List<Type> _adultGargGrabbable = new() { typeof(SeaDragon), typeof(ReaperLeviathan), typeof(GhostLeviathan), typeof(GhostLeviatanVoid), typeof(SeaTreader), typeof(Reefback) };
    private static readonly List<Type> _juvenileGargGrabbable = new() { typeof(ReaperLeviathan), typeof(SeaTreader), typeof(Shocker) };

    public static bool AdultCanGrab(GameObject go)
    {
        var creature = go.GetComponent<Creature>();
        if (creature != null)
        {
            if (_adultGargGrabbable.Contains(creature.GetType())) 
                return true;
        }

        return false;
    }
        
    public static bool JuvenileCanGrab(GameObject go)
    {
        var creature = go.GetComponent<Creature>();
        if (creature != null)
        {
            if (_juvenileGargGrabbable.Contains(creature.GetType())) 
                return true;
        }

        return false;
    }
}