using UnityEngine;

namespace PrawnSuitSpeedModule.Mono;

internal class PrawnSuitSpeedHandler : MonoBehaviour
{
    private Exosuit exosuit;

    private float defaultGroundForceMultiplier = 4f;

    private void Awake()
    {
        exosuit = GetComponent<Exosuit>();
    }

    public void UpdateSpeed()
    {
        if (exosuit == null)
            return;

        exosuit.onGroundForceMultiplier = defaultGroundForceMultiplier * GetForceMultiplier(exosuit);
    }
    
    private static float GetForceMultiplier(Exosuit exosuit)
    {
        return Mathf.Clamp(exosuit.modules.GetCount(Items.Equipment.PrawnSuitSpeedUpgrade.Info.TechType) + 1, 1, 10);
    }
}
