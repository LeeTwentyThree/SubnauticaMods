using UnityEngine;

namespace SeaVoyager.Mono
{
    public class ShipAutoStop : MonoBehaviour
    {
        public SeaVoyager ship;
        private float _maxDistance = 300f;

        private void Start()
        {
            InvokeRepeating(nameof(Check), Random.value, 1f);
        }

        private void Check()
        {
            if (Vector3.Distance(Player.main.transform.position, transform.position) > _maxDistance)
            {
                ship.currentState = ShipState.Idle;
                ship.moveDirection = ShipMoveDirection.Idle;
                ship.rotateDirection = ShipRotateDirection.Idle;
            }
        }
    }
}
