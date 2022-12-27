using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ShipMod.Ship
{
    public class ShipPropeller : MonoBehaviour
    {
        public SeaVoyager ship;
        float angularVelocity = 0f;
        readonly float maxAngularVelocity = 2400;
        readonly float angularVelocityLossRate = 600f;
        public Vector3 rotationDirection;
        public float spinSpeed = 750f;
        
        public void Update()
        {
            angularVelocity = Mathf.Clamp(angularVelocity + (GetSpinSpeed() * Time.deltaTime), 0f, maxAngularVelocity * ship.SpeedMultiplier);
            transform.Rotate(rotationDirection * angularVelocity * Time.deltaTime);
        }

        private float GetSpinSpeed()
        {
            if (ship.currentState == ShipState.Idle)
            {
                return -angularVelocityLossRate;
            }
            return spinSpeed * ship.SpeedMultiplier;
        }
    }
}
