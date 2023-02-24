using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SeaVoyager.Ship
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
