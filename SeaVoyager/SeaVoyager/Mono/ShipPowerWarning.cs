using UnityEngine;

namespace SeaVoyager.Mono
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
