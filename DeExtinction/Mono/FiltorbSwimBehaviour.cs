using UnityEngine;

namespace DeExtinction.Mono;

// HAS TO BE A CREATURE ACTION, otherwise the Filtorb will constantly hide because it has no alternative action!
internal class FiltorbSwimBehaviour : CreatureAction
{
    public float force = 16;
    public float rotationalForce = 0.5f;

    private Vector3 _torqueDirection = new Vector3(1, 1, 1).normalized;
    private Rigidbody _rigibody;

    void Start()
    {
        _rigibody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        _rigibody.AddForce(FiltorbCurrentGenerator.Main.CurrentForceDirection * (force * Time.deltaTime));
        _rigibody.AddTorque(_torqueDirection * (rotationalForce * Time.deltaTime));
    }
}