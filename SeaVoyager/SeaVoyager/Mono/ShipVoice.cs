using System.Collections.Generic;
using Nautilus.Utility;
using UnityEngine;

namespace SeaVoyager.Mono
{
    public class ShipVoice : MonoBehaviour
    {
        public FMOD_CustomEmitter emitter;

        private readonly Dictionary<VoiceLine, float> _timeLinesCanPlayAgain = new();

        private readonly Dictionary<VoiceLine, float> _voiceLineDurations = new()
        {
            {VoiceLine.AheadFlank, 3.3f},
            {VoiceLine.AheadSlow, 1.5f},
            {VoiceLine.AheadStandard, 1.5f},
            {VoiceLine.ApproachingShallowWater, 5.5f},
            {VoiceLine.EnginePoweringDown, 5.5f},
            {VoiceLine.EnginePoweringUp, 8},
            {VoiceLine.FirstUse, 4.5f},
            {VoiceLine.PowerDepleted, 3},
            {VoiceLine.SonarMap, 2},
            {VoiceLine.RegionMap, 1.5f},
            {VoiceLine.VehicleAttached, 1.5f},
            {VoiceLine.VehicleDock, 5f},
            {VoiceLine.VehicleReleased, 1f},
            {VoiceLine.WelcomeAboard, 4.3f},
        };
        
        private readonly Dictionary<VoiceLine, float> _voiceLineMinDelays = new()
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

        private readonly Dictionary<VoiceLine, string> _voiceLineSubtitleKey = new()
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

        private float _currentLineEndTime;
        private VoiceLine _current = VoiceLine.None;
        
        private bool IsAnyVoiceLinePlaying()
        {
            if (_current == VoiceLine.None)
            {
                return false;
            }

            return Time.time < _currentLineEndTime; 
        }

        private void Start()
        {
            LoadAudioClips();
        }

        private Dictionary<VoiceLine, FMODAsset> voiceLineClips;

        private void LoadAudioClips()
        {
            voiceLineClips = new Dictionary<VoiceLine, FMODAsset>();
            voiceLineClips.Add(VoiceLine.AheadFlank, AudioUtils.GetFmodAsset("seavoyager_ahead_flank"));
            voiceLineClips.Add(VoiceLine.AheadSlow, AudioUtils.GetFmodAsset("seavoyager_ahead_slow"));
            voiceLineClips.Add(VoiceLine.AheadStandard, AudioUtils.GetFmodAsset("seavoyager_ahead_standard"));
            voiceLineClips.Add(VoiceLine.ApproachingShallowWater, AudioUtils.GetFmodAsset("seavoyager_approaching_shallow_water"));
            voiceLineClips.Add(VoiceLine.EnginePoweringDown, AudioUtils.GetFmodAsset("seavoyager_engine_powering_down"));
            voiceLineClips.Add(VoiceLine.EnginePoweringUp, AudioUtils.GetFmodAsset("seavoyager_engine_powering_up"));
            voiceLineClips.Add(VoiceLine.FirstUse, AudioUtils.GetFmodAsset("seavoyager_first_use"));
            voiceLineClips.Add(VoiceLine.PowerDepleted, AudioUtils.GetFmodAsset("seavoyager_reserve_power_empty"));
            voiceLineClips.Add(VoiceLine.SonarMap, AudioUtils.GetFmodAsset("seavoyager_sonar_map_activated"));
            voiceLineClips.Add(VoiceLine.RegionMap, AudioUtils.GetFmodAsset("seavoyager_updating_regional_map"));
            voiceLineClips.Add(VoiceLine.VehicleAttached, AudioUtils.GetFmodAsset("seavoyager_vehicle_attached"));
            voiceLineClips.Add(VoiceLine.VehicleDock, AudioUtils.GetFmodAsset("seavoyager_vehicle_docked_successfully"));
            voiceLineClips.Add(VoiceLine.VehicleReleased, AudioUtils.GetFmodAsset("seavoyager_vehicle_released"));
            voiceLineClips.Add(VoiceLine.WelcomeAboard, AudioUtils.GetFmodAsset("seavoyager_welcome_aboard_captain"));
        }

        private void SetTimeCanPlayAgain(VoiceLine line)
        {
            if (_voiceLineMinDelays.TryGetValue(line, out float minDelay))
            {
                if (_timeLinesCanPlayAgain.ContainsKey(line))
                {
                    _timeLinesCanPlayAgain[line] = Time.time + minDelay;
                }
                else
                {
                    _timeLinesCanPlayAgain.Add(line, Time.time + minDelay);
                }
            }
        }

        public bool PlayVoiceLine(VoiceLine line, bool canInterruptSelf = false)
        {
            if (!canInterruptSelf && IsAnyVoiceLinePlaying())
            {
                return false;
            }

            if (_timeLinesCanPlayAgain.TryGetValue(line, out float timeCanPlayAgain))
            {
                if (Time.time < timeCanPlayAgain)
                {
                    return false;
                }
            }
            
            _current = line;
            if (_voiceLineDurations.TryGetValue(line, out var duration))
            {
                _currentLineEndTime = Time.time + duration;
            }
            else
            {
                _currentLineEndTime = Time.time + 0.5f;
            }

            emitter.SetAsset(voiceLineClips[line]);
            emitter.Play();
            SetTimeCanPlayAgain(line);
            if (_voiceLineSubtitleKey.TryGetValue(line, out var subtitleKey))
            {
                Subtitles.Add(subtitleKey);
            }
            return true;
        }

        public enum VoiceLine
        {
            None = -1,
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
