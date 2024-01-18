using UnityEngine;

namespace DeExtinction.Mono;

internal class FiltorbSwimBehaviour : CreatureAction
{
    public float force = 50f;
    public float rotationalForce = 0.5f;

    private Vector3 _torqueDirection = new Vector3(1, 1, 1).normalized;
    private Rigidbody _rigibody;

    void Start()
    {
        _rigibody = GetComponent<Rigidbody>();
    }

    public override float Evaluate(Creature creature, float time)
    {
        return evaluatePriority;
    }

    void FixedUpdate()
    {
        _rigibody.AddForce(FiltorbCurrentGenerator.Main.CurrentForceDirection * (force * Time.deltaTime));
        if (_torqueDirection != Vector3.zero)
            _rigibody.AddTorque(_torqueDirection * (rotationalForce * Time.deltaTime));
    }
}