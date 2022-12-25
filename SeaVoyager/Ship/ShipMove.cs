using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ShipMod.Ship
{
    public class ShipMove : MonoBehaviour
    {
        public SeaVoyager ship;

        void FixedUpdate()
        {
            if(ship.currentState == ShipState.Idle)
            {
                Decelerotate();
                return;
            }
            if(GameModeUtils.IsInvisible() || ship.powerRelay.ConsumeEnergy(Time.fixedDeltaTime * 2f * QPatch.config.PowerDepletionRate, out float amountConsumed))
            {
                switch (ship.currentState)
                {
                    default:
                        break;
                    case ShipState.Moving:
                        ship.rb.AddForce(-ship.transform.forward * ship.MoveAmount * ship.rb.mass, ForceMode.Force);
                        Decelerotate();
                        break;
                    case ShipState.Rotating:
                        ship.rb.AddTorque(Vector3.up * ship.RotateAmount * ship.rb.mass, ForceMode.Force);
                        break;
                    case ShipState.MovingAndRotating:
                        ship.rb.AddForce(-ship.transform.forward * ship.MoveAmount * ship.rb.mass, ForceMode.Force);
                        ship.rb.AddTorque(Vector3.up * ship.RotateAmount * ship.rb.mass, ForceMode.Force);
                        break;
                }
            }
            else
            {
                ship.currentState = ShipState.Idle;
                ship.moveDirection = ShipMoveDirection.Idle;
                ship.rotateDirection = ShipRotateDirection.Idle;
                ship.hud.SetButtonImages();
                ship.voiceNotificationManager.PlayVoiceNotification(ship.noPowerNotification);
            }
            ship.rb.angularVelocity = new Vector3(ship.rb.angularVelocity.x, Mathf.Clamp(ship.rb.angularVelocity.y, -1f, 1f), ship.rb.angularVelocity.z);
        }
        void Decelerotate()
        {
            ship.rb.AddTorque(-ship.rb.angularVelocity, ForceMode.Acceleration);
        }
        void OnEnable()
        {
            Player.main.playerDeathEvent.AddHandler(this, OnDeath);
            Player.main.playerRespawnEvent.AddHandler(this, OnRespawn);
        }
        void OnDisable()
        {
            Player.main.playerDeathEvent.RemoveHandler(this, OnDeath);
            Player.main.playerRespawnEvent.RemoveHandler(this, OnRespawn);
        }
        void OnDeath(Player player)
        {
            ship.hud.OnStop();
            ship.rb.velocity = Vector3.zero;
            ship.rb.angularVelocity = Vector3.zero;
            ship.rb.isKinematic = true;
        }
        void OnRespawn(Player player)
        {
            if(Player.main.GetCurrentSub() == ship) ship.enginePowerDownNotification.Play();
            ship.rb.isKinematic = false;
        }
    }
}
