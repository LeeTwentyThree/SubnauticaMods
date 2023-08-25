using Nautilus.Json;
using Nautilus.Options.Attributes;

namespace MonkeySayMonkeyGet;

[Menu("Monkey Say Monkey Get")]
public class Config : ConfigFile
{
    [Toggle("Enable debug mode", Tooltip = "Check this box if you want to see some debug messages and everything you say in the top left hand corner.")]
    public bool EnableDebugMode = false;
    [Toggle("Disable \"Hello\" command", Tooltip = "Check this box if you want to disable the Shrek popup which is mainly used for testing.")]
    public bool DisableShrek = false;
    [Toggle("Disable \"Beat the Game\" command", Tooltip = "Check this box if you want to disable the command and have a (more) serious playthrough.")]
    public bool DisableBeatTheGame = false;
    [Toggle("Recognize \"Everything\"", Tooltip = "Check this box if you want certain commands to affect EVERYTHING in existence at your will. May break your game.")]
    public bool CanMentionEverything = true;
    [Slider(Label = "Sound volume", Tooltip = "Scale the volume of the sound with this slider.", DefaultValue = 50f, Min = 0f, Max = 100f, Step = 1f)]
    public float SoundVolume = 50f;
}
