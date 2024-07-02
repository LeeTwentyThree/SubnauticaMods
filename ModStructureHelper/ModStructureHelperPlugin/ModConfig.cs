using Nautilus.Json;
using Nautilus.Options.Attributes;
using UnityEngine;

namespace ModStructureHelperPlugin;

[Menu("Mod Structure Helper")]
public class ModConfig : ConfigFile
{
    [Keybind("Toggle structure helper bind")]
    public KeyCode ToggleStructureHelperKeyBind = KeyCode.F4;
    [Keybind("Save bind (+control)")]
    public KeyCode SaveKeyBind = KeyCode.S;
    [Keybind("Tools: select")]
    public KeyCode SelectBind = KeyCode.Q;
    [Keybind("Tools: translate")]
    public KeyCode TranslateBind = KeyCode.E;
    [Keybind("Tools: rotate")]
    public KeyCode RotateBind = KeyCode.R;
    [Keybind("Tools: scale")]
    public KeyCode ScaleBind = KeyCode.T;
    [Keybind("Tools: drag entities")]
    public KeyCode DragBind = KeyCode.F;
    [Keybind("Tools: open entity editor")]
    public KeyCode EntityEditorBind = KeyCode.Tab;
    [Keybind("Tools: use brush")]
    public KeyCode PaintBrushBind = KeyCode.B;
    [Keybind("Tools: toggle global space")]
    public KeyCode ToggleGlobalSpaceBind = KeyCode.G;
    [Keybind("Tools: toggle snapping")]
    public KeyCode ToggleSnappingBind = KeyCode.P;
    [Keybind("Tools: pick object for brush")]
    public KeyCode PickObjectBind = KeyCode.K;
    [Keybind("Tools: open cable editor")]
    public KeyCode CableEditorBind = KeyCode.M;
    [Keybind("Tools: duplicate (+control)")]
    public KeyCode DuplicateBind = KeyCode.D;
    [Keybind("Tools: select all (+control)")]
    public KeyCode SelectAllBind = KeyCode.A;
    [Keybind("Tools: undo (+control)")]
    public KeyCode UndoBind = KeyCode.Z;
    [Keybind("Tools: delete selected object(s)")]
    public KeyCode DeleteBind = KeyCode.Delete;
    [Keybind("Brush tool: rotate left")]
    public KeyCode BrushRotateLeft = KeyCode.LeftBracket;
    [Keybind("Brush tool: rotate right")]
    public KeyCode BrushRotateRight = KeyCode.RightBracket;
    [Keybind("Select tool: select trigger colliders")]
    public KeyCode PrioritizeTriggers = KeyCode.LeftAlt;
    [Keybind("Scale tool: use uniform scale")]
    public KeyCode ScaleUniform = KeyCode.LeftAlt;
    [Toggle("Use consistent handle scale")]
    public bool ConsistentHandleScales = true;
    [Slider("Handle scale multiplier", 0.1f, 2f, DefaultValue = 1f, Step = 0.1f, Format = "{0:F1}x")]
    public float HandleScaleMultiplier = 1f;
}