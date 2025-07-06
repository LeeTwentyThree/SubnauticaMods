using Nautilus.Json;
using Nautilus.Options.Attributes;

namespace SeaVoyager;

[Menu("Sea Voyager Configuration")]
public class SeaVoyagerConfig : ConfigFile
{
    [Toggle("Enable lower window")]
    public bool EnableCabinWindow = false;
}
