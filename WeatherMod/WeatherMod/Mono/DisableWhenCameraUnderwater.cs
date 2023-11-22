using UnityEngine;

namespace WeatherMod.Mono;

public class DisableWhenCameraUnderwater : MonoBehaviour
{
    private Renderer[] _renderers;

    private void Awake()
    {
        _renderers = GetComponentsInChildren<Renderer>(true);
    }
    
    private void Update()
    {
    }
}