using UnityEngine;

namespace SeaVoyager.Mono
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
