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
        readonly float maxAngularVelocity = 15120f;
        readonly float angularVelocityLossRate = 1512f;
        public Vector3 rotationDirection;
        public float spinSpeed = 3024f;
        
        public void Update()
        {
            if (ship.currentState != ShipState.Idle)
            {
                angularVelocity = Mathf.Clamp(angularVelocity + (spinSpeed * Time.deltaTime), 0f, maxAngularVelocity);
            }
            else
            {
                angularVelocity = Mathf.Clamp(angularVelocity - (angularVelocityLossRate * Time.deltaTime), 0f, maxAngularVelocity);
            }
            transform.Rotate(rotationDirection * angularVelocity * Time.deltaTime);
        }
    }
}
