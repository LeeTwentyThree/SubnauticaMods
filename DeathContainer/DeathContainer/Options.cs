using Nautilus.Json;
using Nautilus.Options.Attributes;

namespace DeathContainer;

[Menu("Death Container")]
internal class Options : ConfigFile
{
    [Toggle("Enable HUD signal", Tooltip = "If disabled, the death container will not display the location of your death on your HUD.")]
    public bool EnableSignal = true;
    
    [Toggle("Destroy empty containers", Tooltip = "Automatically destroy empty containers after some duration?")]
    public bool DestroyEmptyContainers = true;
    
    [Slider("Container destroy delay", Tooltip = "How many seconds it takes the containers to be destroyed once emptied.", DefaultValue = 180, Min = 2, Max = 600, Format = "{0} seconds")]
    public int DestroyDelaySeconds = 180;
}
