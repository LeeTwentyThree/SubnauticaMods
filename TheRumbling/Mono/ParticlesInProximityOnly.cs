using UnityEngine;

namespace TheRumbling.Mono;

public class ParticlesInProximityOnly : MonoBehaviour
{
    private ParticleSystem system;
    //private Light light;

    private void Start()
    {
        system = gameObject.GetComponent<ParticleSystem>();
        //light = gameObject.GetComponent<Light>();
        InvokeRepeating(nameof(Check), Random.value, 1);
    }

    private void Check()
    {
        var e = Vector3.SqrMagnitude(transform.position - MainCamera.camera.transform.position) < (250 * 250);
        system.EnableEmission(e);
        //light.enabled = e;
    }
}