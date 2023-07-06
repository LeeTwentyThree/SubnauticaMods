using UnityEngine;

namespace PrawnSuitSpeedModule.Mono;

internal class PrawnSuitSpeedHandler : MonoBehaviour
{
    private Exosuit exosuit;

    private float defaultForwardForce = 8.2f;
    private float defaultBackwardForce = 3f;
    private float defaultSidewardForce = 4.2f;

    private void Awake()
    {
        exosuit = GetComponent<Exosuit>();
    }

    public void UpdateSpeed()
    {
        if (exosuit == null)
            return;

        float multiplier = GetForceMultiplier(exosuit);

        exosuit.forwardForce = defaultForwardForce * multiplier;
        exosuit.backwardForce = defaultBackwardForce * multiplier;
        exosuit.sidewardForce = defaultSidewardForce * multiplier;
    }
    
    private static float GetForceMultiplier(Exosuit exosuit)
    {
        return Mathf.Clamp(exosuit.modules.GetCount(Items.Equipment.PrawnSuitSpeedUpgrade.Info.TechType) + 1, 1, 10);
    }
}
