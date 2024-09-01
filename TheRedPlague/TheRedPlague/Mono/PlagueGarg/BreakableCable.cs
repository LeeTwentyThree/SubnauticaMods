using System.Collections.Generic;
using UnityEngine;

namespace TheRedPlague.Mono.PlagueGarg;

public class BreakableCable : MonoBehaviour
{
    public Animator animator;
    
    private static readonly int Property = Animator.StringToHash("break");

    private static readonly List<BreakableCable> Cables = new();
    
    public void Break()
    {
        animator.SetTrigger(Property);
        enabled = false;

        gameObject.AddComponent<InfectAnything>();
        
        if (Cables.Count == 0)
        {
            StoryUtils.InfectCables.Trigger();
        }

        CollapsibleCubeBehaviour.OnCableBroken(gameObject.GetComponent<PrefabIdentifier>().Id);
        transform.Find("BreakableCable/Armature.001/Bone.024").gameObject.AddComponent<FallIntoVoid>();
    }

    private void OnEnable()
    {
        Cables.Add(this);
    }
    
    private void OnDisable()
    {
        Cables.Remove(this);
    }

    public static bool TryGetClosestCableToPoint(Vector3 point, out BreakableCable result)
    {
        result = null;
        var closest = float.MaxValue;
        foreach (var cable in Cables)
        {
            var sqrDist = Vector3.SqrMagnitude(cable.transform.position);
            if (sqrDist < closest)
            {
                closest = sqrDist;
                result = cable;
            }
        }

        return result != null;
    }
}