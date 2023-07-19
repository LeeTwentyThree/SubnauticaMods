using Nautilus.Json;
using Nautilus.Options.Attributes;

namespace LiveMinimap;

[Menu("Live Minimap")]
internal class Config : ConfigFile
{
    [Slider("Minimap Height", DefaultValue = 50f, Min = 10f, Max = 150f, Tooltip = "The distance in meters above the player at which the minimap is rendered.")]
    public float Height = 50f;
    [Slider("Minimap FOV", DefaultValue = 100f, Min = 15f, Max = 120f, Tooltip = "FOV of the minimap camera.")]
    public float FOV = 100f;
}