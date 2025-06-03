using UnityEngine;

namespace PdaUpgradeCards.MonoBehaviours;

public class PdaUpgradeButton : MonoBehaviour
{
    public static PdaUpgradeButton Main { get; private set; }

    private void Awake()
    {
        Main = this;
    }
}