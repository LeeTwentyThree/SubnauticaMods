using UnityEngine;

namespace WeatherMod;

internal class FogManagerUpdater : MonoBehaviour
{
    private static FogManagerUpdater main;

    public static void Ensure()
    {
        var updater = new GameObject("FogManagerUpdater");
        updater.AddComponent<FogManagerUpdater>();
    }

    private void Awake()
    {
        main = this;
    }

    private void Update()
    {
        FogManager.Update();
    }
}