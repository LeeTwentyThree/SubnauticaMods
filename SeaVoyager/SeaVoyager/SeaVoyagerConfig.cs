using Nautilus.Json;
using Nautilus.Options.Attributes;

namespace SeaVoyager;

[Menu("Sea Voyager Configuration")]
public class SeaVoyagerConfig : ConfigFile
{
    [Toggle("Enable lower window")]
    public bool EnableCabinWindow = false;
    [Toggle("Override player movement to fix physics issues", Label = "This setting overrides the player's current movement mode, possibly breaking other parts of the game, but improves physics when on the Sea Voyager deck. Restart required.")]
    public bool OverrideGameMovement = true;
}
