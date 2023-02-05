using SMLHelper.V2.Handlers;
using SMLHelper.V2.Json;
using SMLHelper.V2.Options.Attributes;
using UnityEngine;

namespace DebugHelper
{
    [Menu("DebugHelper")]
    public class DebugHelperConfig : ConfigFile
    {
        public static DebugHelperConfig Config { get; } = OptionsPanelHandler.RegisterModOptions<DebugHelperConfig>();

        [Slider("Debug Icons (Search range)", DefaultValue = 35f, Min = -1f, Max = 150f, Tooltip = "Components in this range will be debugged. Values less than 0 count as infinity. High values can be VERY slow.")]
        public float DebugRange = 35f;
        [Slider("Debug Icons (Update interval)", DefaultValue = 1f, Min = 1f, Max = 10f, Tooltip = "The number of seconds between debug renderers being automatically regenerated.\nIncrease this value if the debug systems are affecting framerate.")]
        public float DebugUpdateInterval = 1f;
        [Toggle("Debug Light component", Tooltip = "Automatically runs the 'showlights' command.")]
        public bool ShowLights = false;
        [Toggle("Debug FMOD emitters", Tooltip = "Automatically runs the 'showemitters' command.")]
        public bool ShowEmitters = false;
        [Toggle("Debug creature actions", Tooltip = "Automatically runs the 'creatureactions' command.")]
        public bool ShowCreatureActions = false;
        [Toggle("Debug LiveMixin component", Tooltip = "Automatically runs the 'showhealth' command.")]
        public bool ShowHealth = false;
        [Toggle("Debug Rigidbody component", Tooltip = "Automatically runs the 'showrigidbodies' command.")]
        public bool ShowRigidbodies = false;
        [Toggle("Show damage numbers", Tooltip = "If enabled, shows information whenever a LiveMixin takes damage.")]
        public bool ShowDamageInfo = false;
        [Toggle("Hide small damage numbers", Tooltip = "If enabled, damage numbers of less than 0.1 will be hidden.")]
        public bool HideSmallDamageNumbers = true;
        [Toggle("Show ClassIDs", Tooltip = "Automatically runs the 'showclassids' command.")]
        public bool ShowClassIDs = false;
        [Toggle("Generate SpawnInfo", Tooltip = "Automatically runs the 'showspawninfo' command.")]
        public bool ShowSpawnInfo = false;

        public KeyCode InteractWithDebugIconKey1 = KeyCode.Mouse0;
        public KeyCode InteractWithDebugIconKey2 = KeyCode.LeftShift;

        public static float DebugIconScale
        {
            get
            {
                return 50 / 100f;
            }
        }

        public static bool DebugIconsAre3D = true;
    }
}
