using Nautilus.Utility;
using PdaUpgradeChips.MonoBehaviours.UI;
using UnityEngine;
using Random = UnityEngine.Random;

namespace PdaUpgradeChips.MonoBehaviours.Upgrades;

public class LeviathanDetectorUpgrade : UpgradeChipBase, IScheduledUpdateBehaviour
{
    private const float CooldownWhileLeviathansAreActive = 150;
    private const float CooldownAfterLeviathanDisappears = 15;
    private const float DetectionRadius = 140;
    private const string VoiceLinePrefix = "PdaDetectingLeviathan";
    private const int VoiceLineVariations = 3;

    private float _timeDetectionCooldownEnds;
    private float _timeDespawnCooldownEnds;

    private int _knownLeviathanCount;

    private FMODAsset[] _sounds;

    private void OnEnable()
    {
        if (_sounds == null)
        {
            _sounds = new FMODAsset[VoiceLineVariations];
            for (var i = 0; i < VoiceLineVariations; i++)
            {
                _sounds[i] = AudioUtils.GetFmodAsset(VoiceLinePrefix + (i + 1));
            }
        }
        UpdateSchedulerUtils.Register(this);
    }

    private void OnDisable()
    {
        UpdateSchedulerUtils.Deregister(this);
        if (LeviathanDetectorUI.main)
        {
            LeviathanDetectorUI.main.gameObject.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        foreach (var sound in _sounds)
        {
            Destroy(sound);
        }
    }

    public string GetProfileTag()
    {
        return "LeviathanDetectorUpgrade";
    }

    public void ScheduledUpdate()
    {
        var newLeviathanCount =
            AggressiveLeviathanTracker.GetNumberOfTrackersInRange(Player.main.transform.position, DetectionRadius);

        if (LeviathanDetectorUI.main)
        {
            LeviathanDetectorUI.main.gameObject.SetActive(newLeviathanCount > 0);
        }

        // Detecting first leviathan
        if (_knownLeviathanCount == 0 && newLeviathanCount >= 1)
        {
            if (Time.time > _timeDespawnCooldownEnds)
            {
                WarnPlayer();
                _timeDetectionCooldownEnds = Time.time + CooldownWhileLeviathansAreActive;
            }
        }
        // Detecting another leviathan
        else if (newLeviathanCount > 0)
        {
            if (Time.time > _timeDespawnCooldownEnds && Time.time > _timeDetectionCooldownEnds)
            {
                WarnPlayer();
                _timeDetectionCooldownEnds = Time.time + CooldownWhileLeviathansAreActive;
            }
        }
        else if (_knownLeviathanCount > 0 && newLeviathanCount == 0)
        {
            _timeDespawnCooldownEnds = Time.time + CooldownAfterLeviathanDisappears;
        }
        
        _knownLeviathanCount = newLeviathanCount;
    }

    private void WarnPlayer()
    {
        var voiceLineIndex = Random.Range(0, VoiceLineVariations);
        Subtitles.Add(VoiceLinePrefix + (voiceLineIndex + 1));
        Utils.PlayFMODAsset(_sounds[voiceLineIndex], Player.main.transform.position);
    }

    public int scheduledUpdateIndex { get; set; }
}