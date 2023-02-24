using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SeaVoyager.Ship
{
    public class ShipVoice : MonoBehaviour
    {
        public AudioSource source;

        private Dictionary<VoiceLine, float> timeLinesCanPlayAgain = new Dictionary<VoiceLine, float>();

        private Dictionary<VoiceLine, float> voiceLineMinDelays = new Dictionary<VoiceLine, float>()
        {
            {VoiceLine.AheadFlank, 3f},
            {VoiceLine.AheadSlow, 3f},
            {VoiceLine.AheadStandard, 3f},
            {VoiceLine.ApproachingShallowWater, 30f},
            {VoiceLine.EnginePoweringDown, 8f},
            {VoiceLine.EnginePoweringUp, 8f},
            {VoiceLine.FirstUse, 99999f},
            {VoiceLine.PowerDepleted, 20f},
            {VoiceLine.SonarMap, 10f},
            {VoiceLine.RegionMap, 10f},
            {VoiceLine.VehicleAttached, 3f},
            {VoiceLine.VehicleDock, 10f},
            {VoiceLine.VehicleReleased, 3f},
            {VoiceLine.WelcomeAboard, 12f},
        };

        private Dictionary<VoiceLine, string> voiceLineSubtitleKey = new Dictionary<VoiceLine, string>()
        {
            {VoiceLine.AheadFlank, "SeaVoyagerAheadFlank"},
            {VoiceLine.AheadSlow, "SeaVoyagerAheadSlow"},
            {VoiceLine.AheadStandard, "SeaVoyagerAheadStandard"},
            {VoiceLine.ApproachingShallowWater, "SeaVoyagerShallowWater"},
            {VoiceLine.EnginePoweringDown, "SeaVoyagerPoweringDown"},
            {VoiceLine.EnginePoweringUp, "SeaVoyagerPoweringUp"},
            {VoiceLine.FirstUse, "SeaVoyagerFirstUse"},
            {VoiceLine.PowerDepleted, "SeaVoyagerNoPower"},
            {VoiceLine.SonarMap, "SeaVoyagerSonarMap"},
            {VoiceLine.RegionMap, "SeaVoyagerRegionMap"},
            {VoiceLine.VehicleAttached, "SeaVoyagerVehicleAttached"},
            {VoiceLine.VehicleDock, "SeaVoyagerDockVehicle"},
            {VoiceLine.VehicleReleased, "SeaVoyagerVehicleReleased"},
            {VoiceLine.WelcomeAboard, "SeaVoyagerWelcomeAboard"},
        };

        public bool VoiceLinePlaying
        {
            get
            {
                return source.isPlaying;
            }
        }

        private void Start()
        {
            LoadAudioClips();
        }

        private Dictionary<VoiceLine, AudioClip> voiceLineClips;

        private void LoadAudioClips()
        {
            voiceLineClips = new Dictionary<VoiceLine, AudioClip>();
            voiceLineClips.Add(VoiceLine.AheadFlank, Plugin.bundle.LoadAsset<AudioClip>("ahead_flank"));
            voiceLineClips.Add(VoiceLine.AheadSlow, Plugin.bundle.LoadAsset<AudioClip>("ahead_slow"));
            voiceLineClips.Add(VoiceLine.AheadStandard, Plugin.bundle.LoadAsset<AudioClip>("ahead_standard"));
            voiceLineClips.Add(VoiceLine.ApproachingShallowWater, Plugin.bundle.LoadAsset<AudioClip>("approaching_shallow_water"));
            voiceLineClips.Add(VoiceLine.EnginePoweringDown, Plugin.bundle.LoadAsset<AudioClip>("engine_powering_down"));
            voiceLineClips.Add(VoiceLine.EnginePoweringUp, Plugin.bundle.LoadAsset<AudioClip>("engine_powering_up"));
            voiceLineClips.Add(VoiceLine.FirstUse, Plugin.bundle.LoadAsset<AudioClip>("first_use"));
            voiceLineClips.Add(VoiceLine.PowerDepleted, Plugin.bundle.LoadAsset<AudioClip>("reserve_power_empty"));
            voiceLineClips.Add(VoiceLine.SonarMap, Plugin.bundle.LoadAsset<AudioClip>("sonar_map_activated"));
            voiceLineClips.Add(VoiceLine.RegionMap, Plugin.bundle.LoadAsset<AudioClip>("updating_regional_map"));
            voiceLineClips.Add(VoiceLine.VehicleAttached, Plugin.bundle.LoadAsset<AudioClip>("vehicle_attached"));
            voiceLineClips.Add(VoiceLine.VehicleDock, Plugin.bundle.LoadAsset<AudioClip>("vehicle_docked_successfully"));
            voiceLineClips.Add(VoiceLine.VehicleReleased, Plugin.bundle.LoadAsset<AudioClip>("vehicle_released"));
            voiceLineClips.Add(VoiceLine.WelcomeAboard, Plugin.bundle.LoadAsset<AudioClip>("welcome_aboard_captain"));
        }

        private void SetTimeCanPlayAgain(VoiceLine line)
        {
            if (voiceLineMinDelays.TryGetValue(line, out float minDelay))
            {
                if (timeLinesCanPlayAgain.ContainsKey(line))
                {
                    timeLinesCanPlayAgain[line] = Time.time + minDelay;
                }
                else
                {
                    timeLinesCanPlayAgain.Add(line, Time.time + minDelay);
                }
            }
        }

        public bool PlayVoiceLine(VoiceLine line, bool canInterruptSelf = false)
        {
            if (!canInterruptSelf && VoiceLinePlaying)
            {
                return false;
            }
            if (timeLinesCanPlayAgain.TryGetValue(line, out float timeCanPlayAgain))
            {
                if (Time.time < timeCanPlayAgain)
                {
                    return false;
                }
            }
            source.clip = voiceLineClips[line];
            source.Play();
            SetTimeCanPlayAgain(line);
            if (voiceLineSubtitleKey.TryGetValue(line, out var subtitleKey))
            {
                Subtitles.Add(subtitleKey);
            }
            return true;
        }

        public enum VoiceLine
        {
            AheadFlank,
            AheadSlow,
            AheadStandard,
            ApproachingShallowWater,
            EnginePoweringDown,
            EnginePoweringUp,
            FirstUse,
            PowerDepleted,
            SonarMap,
            RegionMap,
            VehicleAttached,
            VehicleDock,
            VehicleReleased,
            WelcomeAboard
        }
    }
}
