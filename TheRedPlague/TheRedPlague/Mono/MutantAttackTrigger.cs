using UnityEngine;

namespace TheRedPlague.Mono;

public class MutantAttackTrigger : MonoBehaviour
{
    public string prefabFileName;
    public bool heavilyMutated;
    
    private void OnTriggerEnter(Collider other)
    {
        if (GetTarget(other).GetComponent<Player>() != null)
        {
            DeathScare.PlayMutantDeathScare(prefabFileName, heavilyMutated);
        }
    }
    
    private GameObject GetTarget(Collider collider)
    {
        var other = collider.gameObject;
        if (other.GetComponent<LiveMixin>() == null && collider.attachedRigidbody != null)
        {
            other = collider.attachedRigidbody.gameObject;
        }
        return other;
    }
}