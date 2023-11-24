using UnityEngine;

namespace WeatherMod.Mono;

public class WaterSplashVfxController : MonoBehaviour
{
    public Transform affectedTransform;
    
    public float heightOffset = 0.1f;
    
    private void LateUpdate()
    {
        var camPos = MainCamera.camera.transform.position;
        affectedTransform.position = new Vector3(camPos.x, Ocean.GetOceanLevel() + heightOffset, camPos.z);
    }
}