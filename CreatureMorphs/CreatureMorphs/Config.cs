using Nautilus.Json;
using Nautilus.Options.Attributes;

namespace CreatureMorphs;

internal class Config : ConfigFile
{
    [Keybind(label: "Morph key")]
    public KeyCode MorphKey = KeyCode.M;
}
