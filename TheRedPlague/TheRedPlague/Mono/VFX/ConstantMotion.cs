using UnityEngine;

namespace TheRedPlague.Mono.VFX;

public class ConstantMotion : MonoBehaviour
{
    public Vector3 motionPerSecond;

    private void Update()
    {
        transform.position += motionPerSecond * Time.deltaTime;
    }
}