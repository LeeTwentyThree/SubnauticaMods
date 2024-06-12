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
    [Keybind("Tools: select")]
    public KeyCode SelectBind = KeyCode.Q;
    [Keybind("Tools: translate")]
    public KeyCode TranslateBind = KeyCode.E;
    [Keybind("Tools: rotate")]
    public KeyCode RotateBind = KeyCode.R;
    [Keybind("Tools: scale")]
    public KeyCode ScaleBind = KeyCode.T;
    [Keybind("Tools: open entity editor")]
    public KeyCode EntityEditorBind = KeyCode.Tab;
    [Keybind("Tools: use brush")]
    public KeyCode PaintBrushBind = KeyCode.B;
    [Keybind("Tools: toggle global space")]
    public KeyCode ToggleGlobalSpaceBind = KeyCode.G;
    [Keybind("Tools: toggle snapping bind")]
    public KeyCode ToggleSnappingBind = KeyCode.P;
    [Keybind("Tools: Delete selected object(s) bind")]
    public KeyCode DeleteBind = KeyCode.Delete;
}