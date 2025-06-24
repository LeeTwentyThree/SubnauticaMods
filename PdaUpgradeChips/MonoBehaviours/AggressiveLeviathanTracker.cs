using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PdaUpgradeChips.MonoBehaviours;

public class AggressiveLeviathanTracker : MonoBehaviour
{
    private static readonly List<AggressiveLeviathanTracker> Trackers = new();
    
    public static IEnumerable<AggressiveLeviathanTracker> GetTrackers() => Trackers;
    public static int TrackersCount => Trackers.Count;

    private void OnEnable()
    {
        Trackers.Add(this);
    }

    private void OnDisable()
    {
        Trackers.Remove(this);
    }

    public static IEnumerable<AggressiveLeviathanTracker> GetTrackersInRange(Vector3 center, float radius)
    {
        return Trackers.Where(tracker => Vector3.Distance(tracker.transform.position, center) <= radius);
    }
    
    public static int GetNumberOfTrackersInRange(Vector3 center, float radius)
    {
        var count = 0;
        
        foreach (var tracker in Trackers)
        {
            if (Vector3.Distance(tracker.transform.position, center) <= radius)
                count++;
        }

        return count;
    }
}