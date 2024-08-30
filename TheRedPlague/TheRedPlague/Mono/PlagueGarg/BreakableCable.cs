using UnityEngine;

namespace TheRedPlague.Mono.PlagueGarg;

public class BreakableCable : MonoBehaviour
{
    public Animator animator;
    
    private static readonly int Property = Animator.StringToHash("break");

    public void Break()
    {
        animator.SetTrigger(Property);
    }
}