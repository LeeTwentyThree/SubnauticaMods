using UnityEngine;

namespace ShipMod.Ship
{
    public class ShipWalkableAreaBounds : MonoBehaviour
    {
        public SeaVoyager ship;
        private float _timeCheckAgain;
        private float _checkInterval = 0.8f;
        private float _maxDistance = 45f;

        private void Update()
        {
            if (Time.time > _timeCheckAgain)
            {
                if (ship.IsOccupiedByPlayer)
                {
                    var distance = Vector3.Distance(Player.main.transform.position, transform.position);
                    if (distance > _maxDistance)
                    {
                        Player.main.SetCurrentSub(null);
                    }
                }
                _timeCheckAgain = Time.time + _checkInterval;
            }
        }
    }
}
