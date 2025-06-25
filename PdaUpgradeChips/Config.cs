using Nautilus.Json;
using Nautilus.Options.Attributes;
using PdaUpgradeChips.MonoBehaviours.Upgrades;

namespace PdaUpgradeChips;

[Menu("PDA Upgrade Chips")]
internal class Config : ConfigFile
{
    [Slider("Minimap field of view", 60, 100, DefaultValue = 70)]
    public float MinimapFOV { get; set; } = 70;

    [Slider("Minimap height offset", 70, 100, DefaultValue = 80)]
    public float MinimapHeightOffset { get; set; } = 80;

    [Toggle("Minimap always points north")]
    public bool MinimapPointsNorth { get; set; } = true;

    [Choice("Minimap location", "Top left", "Top", "Top right",
        "Right", "Bottom right", "Bottom", "Bottom left", "Left")]
    public MinimapUpgrade.Location Location { get; set; } = MinimapUpgrade.Location.TopLeft;

    [Slider("Minimap size", 30, 500, DefaultValue = 200f)]
    public float MinimapScale { get; set; } = 200f;
    
    [Slider("Minimap offset (X)", 0, 400, DefaultValue = 50f, Step = 5)]
    public float MinimapOffsetX { get; set; } = 50f;
    
    [Slider("Minimap offset (Y)", 0, 400, DefaultValue = 50f, Step = 5)]
    public float MinimapOffsetY { get; set; } = 50f;
    
    [Slider("Minimap sonar effect modifier", 0, 1f, DefaultValue = 0.4f, Step = 0.05f, Format = "{0:F2}")]
    public float MinimapSonarEffectModifier { get; set; } = 0.4f;
}