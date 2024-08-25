using UnityEngine;

namespace TheRedPlague.Mono.CreatureBehaviour.Drifter;

public class DrifterMistInstance : MonoBehaviour
{
    public float infectionRange = 20f;
    public float fallStartSpeed = 4f;
    public float fallMaxSpeed = 10f;
    public float fallAccel = 1f;

    private float _fallSpeed;
    
    private float _timeInfectAgain;

    private Vector3 _velocity;

    private void Start()
    {
        _timeInfectAgain = Time.time + Random.value;
    }

    public void SetStartVelocity(Vector3 velocity)
    {
        _velocity = velocity;
    }

    private void Update()
    {
        if (Time.time > _timeInfectAgain)
        {
            InfectInRange();
            _timeInfectAgain = Time.time + 0.5f;
        }

        _fallSpeed = Mathf.Clamp(_fallSpeed + fallAccel * Time.deltaTime, fallStartSpeed, fallMaxSpeed);

        transform.position += new Vector3(0, -_fallSpeed * Time.deltaTime, 0) + _velocity * Time.deltaTime;
    }

    private void InfectInRange()
    {
        InfectionTarget.InfectInRange(transform.position, infectionRange);
    }
}