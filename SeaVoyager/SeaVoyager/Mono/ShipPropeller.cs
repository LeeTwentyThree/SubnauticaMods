using UnityEngine;

namespace SeaVoyager.Mono
{
    public class ShipPropeller : MonoBehaviour
    {
        public SeaVoyager ship;
        
        private float _angularVelocity = 0f;
        private const float MaxAngularVelocity = 2400;
        private const float AngularVelocityLossRate = 600f;
        
        public Vector3 rotationDirection;
        public float spinSpeed = 750f;
        
        public void Update()
        {
            _angularVelocity = Mathf.Clamp(_angularVelocity + (GetSpinSpeed() * Time.deltaTime), 0f, MaxAngularVelocity * ship.SpeedMultiplier);
            transform.Rotate(rotationDirection * (_angularVelocity * Time.deltaTime));
        }

        private float GetSpinSpeed()
        {
            if (ship.currentState == ShipState.Idle)
            {
                return -AngularVelocityLossRate;
            }
            return spinSpeed * ship.SpeedMultiplier;
        }
    }
}
