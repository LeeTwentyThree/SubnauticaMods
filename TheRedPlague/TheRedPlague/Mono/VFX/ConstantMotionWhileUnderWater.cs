using UnityEngine;

namespace TheRedPlague.Mono.VFX;

public class ConstantMotionWhileUnderWater : MonoBehaviour
{
    public Vector3 motionPerSecond;
    public float maxYLevel;

    private void Update()
    {
        if (transform.position.y > maxYLevel) return;
        transform.position += motionPerSecond * Time.deltaTime;
    }
}