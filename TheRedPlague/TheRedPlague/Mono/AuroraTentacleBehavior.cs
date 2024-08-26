using System;
using System.Collections;
using System.Collections.Generic;
using Story;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TheRedPlague.Mono;

public class AuroraTentacleBehavior : MonoBehaviour
{
    public Renderer renderer;
    public Animator animator;
    
    private static List<AuroraTentacleBehavior> _auroraTentacles = new List<AuroraTentacleBehavior>();

    private bool _alreadyGrabbed;

    private void OnEnable()
    {
        _auroraTentacles.Add(this);
    }

    private void Start()
    {
        var story = StoryGoalManager.main;
        if (story == null || _alreadyGrabbed) return;
        if (story.IsGoalComplete(StoryUtils.AuroraThrusterEvent.key))
        {
            renderer.enabled = true;
            animator.SetTrigger("grab_instant");
            _alreadyGrabbed = true;
        }
    }

    private void OnDisable()
    {
        _auroraTentacles.Remove(this);
    }

    private IEnumerator GrabAuroraCoroutine()
    {
        _alreadyGrabbed = true;
        yield return new WaitForSeconds(Random.Range(0, 3));
        animator.SetTrigger("grab");
        yield return null;
        renderer.enabled = true;
    }

    public static void GrabAll()
    {
        foreach (var tentacle in _auroraTentacles)
        {
            if (tentacle && !tentacle._alreadyGrabbed) tentacle.StartCoroutine(tentacle.GrabAuroraCoroutine());
        }
    }
}