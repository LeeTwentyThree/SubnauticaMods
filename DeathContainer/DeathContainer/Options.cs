using Nautilus.Json;
using Nautilus.Options.Attributes;

namespace DeathContainer;

[Menu("Death Container")]
internal class Options : ConfigFile
{
    [Toggle("Enable HUD Signal", Tooltip = "If disabled, the death container will not display the location of your death on your HUD.")]
    public bool EnableSignal = true;
}
