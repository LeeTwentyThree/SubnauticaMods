using UnityEngine;
using UnityEngine.Serialization;

namespace DeExtinction.Mono;

public class ClownPincherNibble : MonoBehaviour
{
    public ClownPincherCreature clownPincher;
    public FMOD_CustomLoopingEmitter eatingEmitter;
    public SwimBehaviour swimBehaviour;
    
    public GameObject currentlyEating;

    private bool _frozen;
    private Rigidbody _rb;
    private float _timeNextNibble = 0f;
    private bool _nibbling;

    private const float NibbleInterval = 1f;
    private const float NibbleHungerDecrement = 0.12f;

    private void Awake()
    {
        _rb = GetComponentInParent<Rigidbody>();
    }

    private bool TouchingFood => currentlyEating != null;
    
    public bool CurrentlyEating => currentlyEating != null && _nibbling;
    
    private void OnTriggerEnter(Collider collider)
    {
        if (clownPincher.Hunger.Value < 0.2f) return;
        
        if (!clownPincher.liveMixin.IsAlive() || _frozen) return;
        
        var nibbleGameObject = collider.gameObject;
        var ecoTarget = nibbleGameObject.GetComponentInParent<EcoTarget>();
        var lm = nibbleGameObject.GetComponentInParent<LiveMixin>();
        var isDead = (lm != null && !lm.IsAlive()) ||
                     (ecoTarget != null && ecoTarget.type == EcoTargetType.DeadMeat);

        var isSpecialConsumable = ecoTarget != null && ecoTarget.type == Plugin.ClownPincherFoodEcoTargetType;

        if (isDead || isSpecialConsumable)
        {
            currentlyEating = collider.gameObject;
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject == currentlyEating)
        {
            currentlyEating = null;
        }
    }

    void Update()
    {
        if (TouchingFood && clownPincher.Hunger.Value > 0.9f)
        {
            StartNibbling();
        }

        if (!_frozen && _nibbling && currentlyEating != null && Time.time > _timeNextNibble)
        {
            _timeNextNibble = Time.time + NibbleInterval;
            EatWaterParticles();
        }

        if (_nibbling && clownPincher.Hunger.Value < 0.1f || !TouchingFood)
        {
            StopNibbling();
        }
    }

    public void EatWaterParticles()
    {
        if (!clownPincher.liveMixin.IsAlive()) return;
        
        clownPincher.PlayEatAnimation();
        clownPincher.Hunger.Add(-NibbleHungerDecrement);
        _rb.AddForce(-_rb.velocity * 0.75f, ForceMode.VelocityChange);
        _rb.AddTorque(-_rb.angularVelocity * 0.75f, ForceMode.VelocityChange);
    }

    public void StartNibbling()
    {
        _nibbling = true;
        eatingEmitter.Play();
        swimBehaviour.LookAt(currentlyEating.transform);
        /*
        var otherRigidbody = _currentlyEating.GetComponentInParent<Rigidbody>();
        if (otherRigidbody != null)
        {
            otherRigidbody.isKinematic = true;
        }
        */
    }

    public void StopNibbling()
    {
        _nibbling = false;
        currentlyEating = null;
        swimBehaviour.LookAt(null);
        eatingEmitter.Stop();
    }

    public void OnFreeze()
    {
        _frozen = true;
    }

    public void OnUnfreeze()
    {
        _frozen = false;
    }
}