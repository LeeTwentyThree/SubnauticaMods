using UnityEngine;

namespace WeatherMod.Mono;

public class AlignWithCamera : MonoBehaviour
{
    public float heightOffset = 10;
    public float minHeightOverWater;
    public bool stickToWaterSurface;
    
    private void Update()
    {
        var camPos = MainCamera.camera.transform.position;
        if (stickToWaterSurface)
            transform.position = new Vector3(camPos.x, Ocean.GetOceanLevel(), camPos.z);
        else
            transform.position = new Vector3(camPos.x, Mathf.Max(minHeightOverWater + Ocean.GetOceanLevel(), camPos.y + heightOffset), camPos.z);
    }
}