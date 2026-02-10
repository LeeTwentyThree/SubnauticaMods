using UnityEngine;

namespace SeaVoyager.Mono
{
    public class ShipMotor : MonoBehaviour
    {
        public SeaVoyager ship;
        public bool reverseForward = true;
        private float _timeCanMoveAgain;

        private void FixedUpdate()
        {
            if(ship.currentState == ShipState.Idle)
            {
                Decelerotate();
                return;
            }
            if (!CanMove())
            {
                return;
            }
            if(GameModeUtils.IsInvisible() || ship.powerRelay.ConsumeEnergy(Time.fixedDeltaTime * 0.3f * ship.SpeedMultiplier, out float amountConsumed))
            {
                switch (ship.currentState)
                {
                    case ShipState.Moving:
                        if (reverseForward)
                        {
                            ship.rb.AddForce(-ship.transform.forward * (ship.MoveAmount * ship.rb.mass), ForceMode.Force);
                        }
                        else
                        {
                            ship.rb.AddForce(ship.transform.forward * (ship.MoveAmount * ship.rb.mass), ForceMode.Force);
                        }
                        Decelerotate();
                        break;
                    case ShipState.Rotating:
                        ship.rb.AddTorque(Vector3.up * (ship.RotateAmount * ship.rb.mass), ForceMode.Force);
                        break;
                    case ShipState.MovingAndRotating:
                        if (reverseForward)
                        {
                            ship.rb.AddForce(-ship.transform.forward * (ship.MoveAmount * ship.rb.mass), ForceMode.Force);
                        }
                        else
                        {
                            ship.rb.AddForce(ship.transform.forward * (ship.MoveAmount * ship.rb.mass), ForceMode.Force);
                        }
                        ship.rb.AddTorque(Vector3.up * (ship.RotateAmount * ship.rb.mass), ForceMode.Force);
                        break;
                }
            }
            else
            {
                ship.currentState = ShipState.Idle;
                ship.moveDirection = ShipMoveDirection.Idle;
                ship.rotateDirection = ShipRotateDirection.Idle;
                ship.hud.UpdateButtonImages();
                ship.voiceNotificationManager.PlayVoiceNotification(ship.noPowerNotification);
            }
            ship.rb.angularVelocity = new Vector3(ship.rb.angularVelocity.x, Mathf.Clamp(ship.rb.angularVelocity.y, -1f, 1f), ship.rb.angularVelocity.z);
        }
        public void StopMovementForSeconds(float seconds)
        {
            _timeCanMoveAgain = Time.time + seconds;
        }
        private bool CanMove()
        {
            return Time.time > _timeCanMoveAgain;
        }

        private void Decelerotate()
        {
            ship.rb.AddTorque(-ship.rb.angularVelocity, ForceMode.Acceleration);
        }

        private void OnEnable()
        {
            Player.main.playerDeathEvent.AddHandler(this, OnDeath);
            Player.main.playerRespawnEvent.AddHandler(this, OnRespawn);
        }

        private void OnDisable()
        {
            Player.main.playerDeathEvent.RemoveHandler(this, OnDeath);
            Player.main.playerRespawnEvent.RemoveHandler(this, OnRespawn);
        }

        private void OnDeath(Player player)
        {
            if (!ship.IsOccupiedByPlayer) return;
            
            ship.currentState = ShipState.Idle;
            ship.moveDirection = ShipMoveDirection.Idle;
            ship.rotateDirection = ShipRotateDirection.Idle;
            ship.rb.velocity = Vector3.zero;
            ship.rb.angularVelocity = Vector3.zero;
            ship.rb.isKinematic = true;
        }

        private void OnRespawn(Player player)
        {
            ship.rb.isKinematic = false;
        }
    }
}
