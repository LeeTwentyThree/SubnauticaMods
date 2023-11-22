using UnityEngine;

namespace WeatherMod.Mono;

public class AlignWithCamera : MonoBehaviour
{
    public float heightOffset = 6;
    public float minHeightOverWater;
    
    private void Update()
    {
        var camPos = MainCamera.camera.transform.position;
        transform.position = new Vector3(camPos.x, Mathf.Max(minHeightOverWater + Ocean.GetOceanLevel(), camPos.y + heightOffset), camPos.z);
    }
}