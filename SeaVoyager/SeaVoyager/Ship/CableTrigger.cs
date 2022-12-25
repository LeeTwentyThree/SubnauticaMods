using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ShipMod.Ship
{
    public class CableTrigger : MonoBehaviour 
    {
        public SuspendedDock dock;

        void OnTriggerEnter(Collider collider)
        {
            var exosuit = collider.gameObject.GetComponentInParent<Vehicle>();
            if (exosuit != null && dock.GetCanDock())
            {
                dock.AttachVehicle(exosuit);
            }
        }
    }
}
