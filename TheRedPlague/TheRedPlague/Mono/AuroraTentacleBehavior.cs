using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheRedPlague.Mono;

public class AuroraTentacleBehavior : MonoBehaviour
{
    public Renderer renderer;
    public Animator animator;
    
    private static List<AuroraTentacleBehavior> _auroraTentacles = new List<AuroraTentacleBehavior>();

    private void OnEnable()
    {
        _auroraTentacles.Add(this);
    }

    private void OnDisable()
    {
        _auroraTentacles.Remove(this);
    }

    private IEnumerator GrabAuroraCoroutine()
    {
        yield return new WaitForSeconds(Random.Range(0, 3));
        animator.SetTrigger("grab");
        yield return null;
        renderer.enabled = true;
    }

    public static void GrabAll()
    {
        foreach (var tentacle in _auroraTentacles)
        {
            if (tentacle) tentacle.StartCoroutine(tentacle.GrabAuroraCoroutine());
        }
    }
}