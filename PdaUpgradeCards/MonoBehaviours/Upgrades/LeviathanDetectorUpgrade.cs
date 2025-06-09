using UnityEngine;

namespace PdaUpgradeCards.MonoBehaviours.Upgrades;

public class LeviathanDetectorUpgrade : UpgradeChipBase, IScheduledUpdateBehaviour
{
    private const float CooldownWhileLeviathansAreActive = 150;
    private const float CooldownAfterLeviathanDisappears = 15;
    private const float DetectionRadius = 120;

    private float _timeDetectionCooldownEnds;
    private float _timeDespawnCooldownEnds;

    private int _knownLeviathanCount;

    private void OnEnable()
    {
        UpdateSchedulerUtils.Register(this);
    }

    private void OnDisable()
    {
        UpdateSchedulerUtils.Deregister(this);
    }

    public string GetProfileTag()
    {
        return "LeviathanDetectorUpgrade";
    }

    public void ScheduledUpdate()
    {
        var newLeviathanCount =
            AggressiveLeviathanTracker.GetNumberOfTrackersInRange(Player.main.transform.position, DetectionRadius);
        
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
        ErrorMessage.AddMessage("Detecting leviathans!!");
    }

    public int scheduledUpdateIndex { get; set; }
}