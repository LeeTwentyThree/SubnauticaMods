using UnityEngine;

namespace DeExtinction.Mono;

public class AcidDamageTrigger : MonoBehaviour
{
    public float acidDamageDuration = 2;
    
    private void OnTriggerEnter(Collider other)
    {
        var liveMixin = other.GetComponentInParent<LiveMixin>();
        if (liveMixin == null) return;
        if (liveMixin.GetComponent<Vehicle>() != null || liveMixin.GetComponent<SubRoot>() != null)
        {
            return;
        }
#if BELOWZERO
        if (liveMixin.GetComponent<IInteriorSpace>() != null)
        {
            return;
        }
#endif
        other.gameObject.EnsureComponent<AcidDamageOverTime>().ResetTimer(acidDamageDuration);
    }
}