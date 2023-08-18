using UnityEngine;

namespace CustomWaterLevel;

internal class VFXConstructionHelper : MonoBehaviour
{
    public VFXConstructing constructing;
    public float defaultOffset;

    private void Start()
    {
        defaultOffset = constructing.heightOffset;
    }

    private void Update()
    {
        constructing.heightOffset = defaultOffset + Plugin.WaterLevel;
    }
}
