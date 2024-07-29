using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TheRedPlague.Mono.CreatureBehaviour.Sucker;

public class SuckerGrabVehicles : MonoBehaviour, IOnTakeDamage
{
    public float grabCooldown = 1.6f;
    public float changeDirectionMinDelay = 0.8f;
    public float changeDirectionMaxDelay = 3f;
    public float vehicleAcceleration = 5f;
    public float vehicleRotationSpeed = 15f;
    public float chanceToMovePerSecond = 0.7f;
    
    public Collider mainCollider;
    public Rigidbody rigidbody;
    
    private Vehicle _currentVehicle;
    
    private float _timeCanGrabAgain;
    private float _timeChangeDirectionAgain;
    private float _timeDecideToMoveAgain;
    private Quaternion _targetRotation;
    private bool _acceleratingVehicle;

    private bool Grabbing => _currentVehicle != null;
    
    private void OnCollisionEnter(Collision other)
    {
        if (Time.time < _timeCanGrabAgain) return;
        if (Grabbing) return;
        
        var vehicle = other.gameObject.GetComponent<Vehicle>();
        if (vehicle != null)
        {
            GrabOntoVehicle(vehicle);
        }
    }

    private void GrabOntoVehicle(Vehicle vehicle)
    {
        transform.parent = vehicle.transform;
        _currentVehicle = vehicle;
        SetCollisionsIgnore(_currentVehicle, false);
        rigidbody.isKinematic = true;
        LargeWorldStreamer.main.cellManager.UnregisterEntity(gameObject);
        transform.LookAt(_currentVehicle.transform);
        if (Physics.Raycast(transform.position, transform.forward, out var hit, 3, -1, QueryTriggerInteraction.Ignore))
            transform.position = hit.point;
        else
            transform.position += transform.forward * 0.5f;
    }

    private void ReleaseFromVehicle()
    {
        if (!Grabbing) return;
        SetCollisionsIgnore(_currentVehicle, false);
        rigidbody.isKinematic = false;
        transform.parent = null;
        LargeWorldStreamer.main.cellManager.RegisterEntity(gameObject);
        _timeCanGrabAgain = Time.time + grabCooldown;
        _currentVehicle = null;
    }

    private void SetCollisionsIgnore(Vehicle vehicle, bool ignore)
    {
        foreach (var collider in vehicle.collisionModel.GetComponentsInChildren<Collider>())
        {
            Physics.IgnoreCollision(mainCollider, collider, ignore);
        }
    }

    public void OnTakeDamage(DamageInfo damageInfo)
    {
        if (damageInfo.damage > 10)
        {
            if (damageInfo.dealer != null && damageInfo.dealer.GetComponent<Vehicle>()) return;
            ReleaseFromVehicle();
        }
    }

    private void FixedUpdate()
    {
        if (!Grabbing) return;

        var vehicleRb = _currentVehicle.useRigidbody;
        
        if (_acceleratingVehicle && Ocean.GetDepthOf(gameObject) > 0)
            vehicleRb.AddRelativeForce(Vector3.forward * vehicleAcceleration, ForceMode.Acceleration);

        if (Time.time > _timeChangeDirectionAgain)
        {
            _timeChangeDirectionAgain = Time.time + Random.Range(changeDirectionMinDelay, changeDirectionMaxDelay);
            _targetRotation = Quaternion.LookRotation(Random.onUnitSphere);
        }

        if (Time.time > _timeDecideToMoveAgain)
        {
            _acceleratingVehicle = Random.value <= chanceToMovePerSecond;
            _timeDecideToMoveAgain = Time.time + 1;
        }
    }

    private void Update()
    {
        if (Grabbing) _currentVehicle.useRigidbody.MoveRotation(Quaternion.RotateTowards(_currentVehicle.useRigidbody.rotation, _targetRotation, vehicleRotationSpeed * Time.deltaTime));
    }
}