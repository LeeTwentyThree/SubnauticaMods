using UnityEngine;

namespace SeaVoyager.Mono
{
    public class HeldByCable : MonoBehaviour
    {
        public SuspendedDock dock;
        public bool Docked
        {
            get
            {
                return dock != null;
            }
        }
    }
}
