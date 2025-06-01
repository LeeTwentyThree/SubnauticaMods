using UnityEngine;

namespace PdaUpgradeChips.MonoBehaviours;

public class PdaUpgradeButton : MonoBehaviour
{
    public static PdaUpgradeButton Main { get; private set; }

    private void Awake()
    {
        Main = this;
    }
}