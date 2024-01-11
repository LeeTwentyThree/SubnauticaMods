using Nautilus.Json;
using Nautilus.Options.Attributes;

namespace TheRedPlague;

[Menu(PluginInfo.PLUGIN_NAME)]
public class ModConfig : ConfigFile
{
    [Slider("Darkness Percent", 0f, 100f, DefaultValue = 90, Format = "{0:F0}%")]
    public float DarknessPercent = 90f;
}