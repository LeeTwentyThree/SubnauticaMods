using UnityEngine;

namespace SeaVoyager.Mono;

public class ShipStabilizer : Stabilizer
{
    private Rigidbody _rigidbody;
    public float baseforcemultiplier = 10f;
    public float depthforcemultiplier = 5f;

    private void Awake()
    {
        _rigidbody = gameObject.GetComponent<Rigidbody>();
    }

    public void LateUpdate()
    {
        uprightAccelerationStiffness = 
            baseforcemultiplier + (Ocean.GetOceanLevel() - gameObject.transform.position.y) * 
            depthforcemultiplier - _rigidbody.velocity.y * depthforcemultiplier;
    }
}