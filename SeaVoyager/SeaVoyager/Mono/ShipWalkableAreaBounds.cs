using UnityEngine;

namespace SeaVoyager.Mono
{
    public class ShipWalkableAreaBounds : MonoBehaviour, IScheduledUpdateBehaviour
    {
        public SeaVoyager ship;
        private const float MaxDistance = 45f;

        public int scheduledUpdateIndex { get; set; }

        public string GetProfileTag()
        {
            return "SeaVoyager:ShipWalkableAreaBounds";
        }

        private void OnEnable()
        {
            UpdateSchedulerUtils.Register(this);
        }

        private void OnDisable()
        {
            UpdateSchedulerUtils.Deregister(this);
        }

        public void ScheduledUpdate()
        {
            if (!ship.IsOccupiedByPlayer) return;
            var distance = Vector3.Distance(Player.main.transform.position, transform.position);
            if (distance > MaxDistance)
            {
                Player.main.SetCurrentSub(null);
            }
        }
    }
}