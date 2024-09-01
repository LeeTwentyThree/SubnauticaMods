using UnityEngine;

namespace TheRedPlague.Mono.PlagueGarg;

public class FallIntoVoid : MonoBehaviour
{
    private float speed = 0f;
    private float maxVelocity = 12;
    private float accel = 0.5f;

    private void Update()
    {
        speed = Mathf.Clamp(speed + accel * Time.deltaTime, 0, maxVelocity);
        transform.position += new Vector3(0, -speed * Time.deltaTime, 0);
    }
}