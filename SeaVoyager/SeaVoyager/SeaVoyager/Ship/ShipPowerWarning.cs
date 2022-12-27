using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ShipMod.Ship
{
    public class ShipPowerWarning : MonoBehaviour
    {
        public SeaVoyager ship;

        private void Update()
        {
            if (ship.IsOccupiedByPlayer && ship.powerRelay.GetPower() <= 1f)
            {
                ship.voice.PlayVoiceLine(ShipVoice.VoiceLine.PowerDepleted);
            }
        }
    }
}
