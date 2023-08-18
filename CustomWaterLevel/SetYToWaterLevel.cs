using UnityEngine;

namespace CustomWaterLevel;

internal class SetYToWaterLevel : MonoBehaviour
{
    public void Update()
    {
        transform.position = new Vector3(0f, Plugin.WaterLevel, 0f);
    }
}
