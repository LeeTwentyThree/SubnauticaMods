using UnityEngine;
using System.Reflection;
using UnityEngine.Serialization;

namespace SeaVoyager.Mono
{
    public class SeaVoyager : SubRoot
    {
        public SkyraySpawner skyraySpawner;
        public ShipHUD hud;
        public ShipPropeller propeller;
        public ShipMotor shipMotor;
        public ShipVoice voice;
        
        private float _lastHealthPercent;

        public ShipState currentState;
        public ShipMoveDirection moveDirection;
        public ShipRotateDirection rotateDirection;
        public ShipSpeedSetting speedSetting = ShipSpeedSetting.Medium;
        
        public float MoveAmount
        {
            get
            {
                if(moveDirection == ShipMoveDirection.Forward)
                {
                    return 25f * SpeedMultiplier;
                }
                if(moveDirection == ShipMoveDirection.Reverse)
                {
                    return -20f * SpeedMultiplier;
                }
                return 0f;
            }
        }

        public bool IsOccupiedByPlayer
        {
            get
            {
                return Player.main.GetCurrentSub() == this;
            }
        }

        public bool Idle
        {
            get
            {
                return currentState == ShipState.Idle;
            }
        }

        public float RotateAmount
        {
            get
            {
                if (rotateDirection == ShipRotateDirection.Left)
                {
                    return -20f;
                }
                if (rotateDirection == ShipRotateDirection.Right)
                {
                    return 20f;
                }
                return 0f;
            }
        }

        public float SpeedMultiplier
        {
            get
            {
                switch (speedSetting)
                {
                    default:
                        return 1f;
                    case ShipSpeedSetting.Slow:
                        return 0.2f;
                    case ShipSpeedSetting.Medium:
                        return 0.5f;
                    case ShipSpeedSetting.Fast:
                        return 1f;

                }
            }
        }

        public bool HasPower
        {
            get
            {
                return powerRelay.GetPower() >= 1f;
            }
        }

        public override void Awake()
        {
            PDAEncyclopedia.Add("SeaVoyager", true);
        }

        public override void OnTakeDamage(DamageInfo damageInfo)
        {
            var live = gameObject.GetComponent<LiveMixin>();
            if (live.GetHealthFraction() < 0.5f && this._lastHealthPercent >= 0.5f)
            {
                voiceNotificationManager.PlayVoiceNotification(this.hullLowNotification, true, false);
            }
            else if (live.GetHealthFraction() < 0.25f && this._lastHealthPercent >= 0.25f)
            {
                voiceNotificationManager.PlayVoiceNotification(this.hullCriticalNotification, true, false);
            }
            _lastHealthPercent = live.GetHealthFraction();
        }


        private new void OnKill()
        {
            var worldForces = GetComponent<WorldForces>();
            worldForces.underwaterGravity = 10f;
            Destroy(this);
        }
    }

    public enum ShipState
    {
        Idle,
        Autopiloting,
        Moving,
        Rotating,
        MovingAndRotating
    }
    public enum ShipMoveDirection
    {
        Idle,
        Forward,
        Reverse
    }
    public enum ShipRotateDirection
    {
        Idle,
        Left,
        Right
    }
    public enum ShipSpeedSetting
    {
        Slow,
        Medium,
        Fast
    }
}
