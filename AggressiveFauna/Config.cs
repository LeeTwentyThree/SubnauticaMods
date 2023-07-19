using Nautilus.Options.Attributes;
using Nautilus.Json;

namespace AggressiveFauna;

[Menu("AggressiveFauna")]
internal class Config : ConfigFile
{
    internal static Config Instance { get { return Plugin.config; } }

    [Toggle("Changes apply during day", Tooltip = "If disabled, the settings below will not have an effect during daytime.")]
    public bool AffectDaytime = true;
    [Toggle("Changes apply during night", Tooltip = "If disabled, the settings below will not have an effect during nighttime.")]
    public bool AffectNighttime = true;
    [Toggle("Aggression warnings", Tooltip = "If enabled, you will get a warning when creatures begin to become more aggressive than usual.")]
    public bool ShowAggressionWarnings = true;
    [Toggle("Aggression music", Tooltip = "If enabled, a music track will play when creatures begin to become more aggressive than usual.")]
    public bool PlayAggressionMusic = false;
    [Slider("Detection radius multiplier", DefaultValue = 1f, Min = 1f, Max = 5f, Step = 0.1f, Tooltip = "The scale factor of the detection range of aggressive creatures.\nUnmodded value: 1x")]
    public float DetectionRadiusMultiplier = 5f;
    [Toggle("Detection through terrain", Tooltip = "If enabled, creatures will be able to detect you through walls.\nUnmodded value: False")]
    public bool CanSeeThroughTerrain = true;
    [Toggle("Detection through bases", Tooltip = "If enabled, creatures will be able to detect you while you're inside a base. They still can not attack the base.\nUnmodded value: False")]
    public bool CanSeeThroughBases = true;
    [Slider("Player prioritization", DefaultValue = 1f, Min = 1f, Max = 5f, Step = 0.1f, Tooltip = "The higher this value, the more creatures will be more likely to attack you over other targets in range.\nUnmodded value: 1x")]
    public float PlayerPrioritization = 5f;
    [Slider("Aggression multiplier", DefaultValue = 1f, Min = 1f, Max = 10f, Step = 0.1f, Tooltip = "General aggression multiplier. Scales the minimum aggression required for attacking.\nUnmodded value: 1x")]
    public float AggressionMultiplier = 10f;
    [Slider("Attack duration scale", DefaultValue = 1f, Min = 1f, Max = 20f, Step = 0.1f, Tooltip = "Multiplier for how long a creature will pursue you for.\nUnmodded value: 1x")]
    public float AttackDurationScale = 3f;
    [Slider("Attack charge velocity scale", DefaultValue = 1f, Min = 1f, Max = 3f, Step = 0.1f, Tooltip = "Multiplier for how fast a creature will charge at you.\nUnmodded value: 1x", Format = "{0:F1}")]
    public float AttackChargeVelocityScale = 2f;
    [Slider("Remember target time scale", DefaultValue = 1f, Min = 1f, Max = 20f, Step = 0.1f, Tooltip = "Multiplier for how long a creature will remember you for.\nUnmodded value: 1x")]
    public float RememberTargetTimeScale = 3f;
    [Slider("Attack cooldown percentage", DefaultValue = 100f, Min = 0f, Max = 100f, Step = 1f, Format = "{0:F0}%", Tooltip = "Multiplier for how long of a break a creature will take after attacking.\nUnmodded value: 100%")]
    public float AttackCooldownPercentage = 30f;
    public float AttackCooldownPercentageNormalized { get { return AttackCooldownPercentage / 100f; } }
    [Slider("Bite cooldown percentage", DefaultValue = 100f, Min = 0f, Max = 100f, Step = 1f, Format = "{0:F0}%", Tooltip = "Multiplier for the amount of time between each melee attack.\nUnmodded value: 100%")]
    public float BiteCooldownPercentage = 30f;
    public float BiteCooldownPercentageNormalized { get { return BiteCooldownPercentage / 100f; } }
    [Toggle("No fleeing", Tooltip = "If enabled, creatures will not flee when taking damage.\nUnmodded value: False")]
    public bool DisableFleeing = true;
    [Toggle("No fear of electricity", Tooltip = "If enabled, creatures will not flee when taking damage from electrical sources, such as perimeter defense modules.\nUnmodded value: False")]
    public bool DisableFleeingFromElectricity = true;
    [Toggle("Ignore befriending", Tooltip = "If enabled, creatures will ignore attempts to feed & befriend them.\nUnmodded value: False")]
    public bool DisableFeeding = true;
    [Toggle("Attack unoccupied vehicles", Tooltip = "If enabled, creatures will be able to target vehicles that are not being piloted.\nUnmodded value: False")]
    public bool AttackUnoccupiedVehicles = true;
    [Toggle("Everything bites vehicles", Tooltip = "If enabled, ALL creatures will be able to deal damage to small vehicles. In the base game, Stalkers and Sandsharks do not have this ability.\nUnmodded value: False")]
    public bool AlwaysBiteVehicles = true;
    [Toggle("Everything bites subs", Tooltip = "If enabled, ALL creatures will be able to deal damage to the Cyclops and other large submarines.\nUnmodded value: False")]
    public bool AlwaysBiteCyclops = true;
    [Slider("FOV multiplier", DefaultValue = 1f, Min = 1f, Max = 4f, Step = 0.1f, Tooltip = "Higher values allow creatures to see you more easily.\nUnmodded value: 1x")]
    public float FOVScale = 3f;
}
