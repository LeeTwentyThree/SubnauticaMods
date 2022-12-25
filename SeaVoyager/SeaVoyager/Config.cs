using SMLHelper.V2.Json;
using SMLHelper.V2.Options.Attributes;
using UnityEngine;
namespace ShipMod
{
    [Menu("Sea Voyager Configuration")]
    public class Config : ConfigFile
    {
        [Slider("Sound volume", 0f, 100f, Step = 1f, DefaultValue = 100f, Tooltip = "Not linked to the Master sound setting.\nRestart required.")]
        public float AudioVolumeV2 = 100f;

        public float NormalizedAudioVolume
        {
            get
            {
                return Mathf.Clamp01(AudioVolumeV2 / 100f);
            }
        }

        [Toggle("Enable lower window", Tooltip = "Uncheck this setting if the window in the Pilot's Cabin is lowering your framerate.")]
        public bool EnableCabinWindow = true;
    }
}
