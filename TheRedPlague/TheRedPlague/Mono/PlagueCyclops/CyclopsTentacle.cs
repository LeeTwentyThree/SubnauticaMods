using System.Collections.Generic;
using UnityEngine;

namespace TheRedPlague.Mono.PlagueCyclops;

public class CyclopsTentacle : MonoBehaviour
{
    private static readonly List<CyclopsTentacle> _tentacles = new();
    
    public Animator animator;
    public UniqueIdentifier identifier;
    public Collider[] colliders;

    private bool _released;

    private string StoryGoalKey => identifier.Id + "TentacleReleased";

    public void Release()
    {
        if (_released) return;
        animator.SetTrigger("release");
        _released = true;
        Story.StoryGoalManager.main.OnGoalComplete(StoryGoalKey);
        colliders.ForEach(c => c.enabled = false);
    }

    private void Start()
    {
        if (Story.StoryGoalManager.main.IsGoalComplete(StoryGoalKey))
        {
            Release();
        }
    }

    private void OnEnable()
    {
        _tentacles.Add(this);
    }
    
    private void OnDisable()
    {
        _tentacles.Remove(this);
    }

    public static int GetReleasedCyclopsTentaclesCount()
    {
        var count = 0;
        foreach (var tentacle in _tentacles)
        {
            if (tentacle == null) continue;
            if (tentacle._released) count++;
        }

        return count;
    }

    public void OnKill()
    {
        Release();
    }
}