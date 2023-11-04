using Nautilus.Options.Attributes;
using Nautilus.Handlers;
using Nautilus.Json;

namespace TheRumbling;

[Menu("<color=#FF0000>The Rumbling</color>")]
public class Config : ConfigFile
{
    [Slider("Titan Walk Speed (m/s)", Min = 0, Max = 50, DefaultValue = 7)]
    public float TitanWalkSpeed = 7f;
}