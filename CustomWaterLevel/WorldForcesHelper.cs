using UnityEngine;

namespace CustomWaterLevel;

internal class WorldForcesHelper : MonoBehaviour
{
    public WorldForces worldForces;
    public float defaultWaterDepth;

    private void Start()
    {
        defaultWaterDepth = worldForces.waterDepth;
    }

    private void Update()
    {
        worldForces.waterDepth = defaultWaterDepth + Plugin.WaterLevel;
    }
}
