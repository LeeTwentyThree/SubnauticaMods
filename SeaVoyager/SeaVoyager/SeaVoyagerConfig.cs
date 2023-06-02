using Nautilus.Json;
using Nautilus.Options.Attributes;

namespace SeaVoyager;

[Menu("Sea Voyager Configuration")]
public class SeaVoyagerConfig : ConfigFile
{
    [Toggle("Enable lower window", Tooltip = "Uncheck this setting if the window in the Pilot's Cabin is lowering your framerate.")]
    public bool EnableCabinWindow = true;
}
