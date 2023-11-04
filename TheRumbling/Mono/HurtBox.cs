using UnityEngine;

namespace TheRumbling.Mono;

public class HurtBox : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        var lm = other.gameObject.GetComponentInParent<LiveMixin>();
        if (lm != null)
        {
            lm.TakeDamage(10000);
        }
    }
    
    private void OnCollisionEnter(Collision c)
    {
        var lm = c.gameObject.GetComponentInParent<LiveMixin>();
        if (lm != null)
        {
            lm.TakeDamage(10000);
        }
    }
}