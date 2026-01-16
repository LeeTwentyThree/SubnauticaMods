using UnityEngine;

namespace SeaVoyager.Mono
{
    public class ShipAutoStop : MonoBehaviour
    {
        public SeaVoyager ship;
        private readonly float _maxDistance = 30f;

        private void Start()
        {
            InvokeRepeating(nameof(Check), Random.value, 1f);
        }

        private void Check()
        {
            if (Player.main.GetVehicle() != null)
                return;
            
            if (Vector3.Distance(Player.main.transform.position, transform.position) > _maxDistance)
            {
                ship.currentState = ShipState.Idle;
                ship.moveDirection = ShipMoveDirection.Idle;
                ship.rotateDirection = ShipRotateDirection.Idle;
            }
        }
    }
}
