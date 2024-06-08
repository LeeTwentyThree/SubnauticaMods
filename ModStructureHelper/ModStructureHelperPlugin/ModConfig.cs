using Nautilus.Json;
using Nautilus.Options.Attributes;
using UnityEngine;

namespace ModStructureHelperPlugin;

[Menu("Mod Structure Helper")]
public class ModConfig : ConfigFile
{
    [Keybind("Toggle structure helper bind")]
    public KeyCode ToggleStructureHelperKeyBind = KeyCode.F4;
    [Keybind("Save bind (paired with control)")]
    public KeyCode SaveKeyBind = KeyCode.S;
}