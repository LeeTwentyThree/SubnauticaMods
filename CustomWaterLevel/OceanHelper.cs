using UnityEngine;

namespace CustomWaterLevel;

internal class OceanHelper : MonoBehaviour
{
    public Ocean ocean;

    private void Update()
    {
        ocean.transform.position = new Vector3(0f, Plugin.WaterLevel, 0f);
        ocean.defaultOceanLevel = Plugin.WaterLevel;
    }
}
