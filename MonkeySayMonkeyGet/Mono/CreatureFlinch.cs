using UnityEngine;

namespace MonkeySayMonkeyGet.Mono;

public class CreatureFlinch : MonoBehaviour
{
    public Creature creature;

    private void Update()
    {
        if (creature)
        {
            creature.GetAnimator().SetFloat("flinch", 1f);
        }
    }
}
