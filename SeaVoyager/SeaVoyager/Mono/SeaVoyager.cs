using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.Reflection;
using UnityEngine.EventSystems;

namespace ShipMod.Ship
{
    public class SeaVoyager : SubRoot
    {
        private ShipLadder embarkLadder;
        private ShipLadder exitLadder;
        private ShipLadder descendLadder;
        private ShipLadder loftLadder;
        private ShipLadder disembarkLadder;
        private ShipLadder engineRoomLadderUp;
        private ShipLadder engineRoomLadderDown;
        private ShipExitDoor exitHatch;
        private ShipSlidingDoor slidingDoor1;
        private ShipSlidingDoor slidingDoor2;
        private SuspendedDock dock;
        private SuspendedDock dock2;

        public SkyraySpawner skyraySpawner;
        public ShipSolarPanel solarPanel;
        public ShipHUD hud;
        public ShipPropeller propeller;
        public ShipMove shipMove;
        public ShipVoice voice;
        private float oldHPPercent;

        public ShipState currentState;
        public ShipMoveDirection moveDirection;
        public ShipRotateDirection rotateDirection;
        public ShipSpeedSetting speedSetting = ShipSpeedSetting.Medium;

        private static FMODAsset climbUpLongSound = Helpers.GetFmodAsset("event:/sub/cyclops/climb_front_up");
        private static FMODAsset climbUpShortSound = Helpers.GetFmodAsset("event:/sub/cyclops/climb_back_up");
        private static FMODAsset slideDownSound = Helpers.GetFmodAsset("event:/sub/rocket/ladders/innerRocketShip_ladder_down");

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
            this.LOD = GetComponent<BehaviourLOD>();
            this.rb = GetComponent<Rigidbody>();
            PropertyInfo prop = GetType().GetProperty("oxygenMgr", BindingFlags.Public | BindingFlags.Instance);
            if (null != prop && prop.CanWrite)
            {
                prop.SetValue(this, GetComponent<OxygenManager>() as object, null);
            }

            powerRelay = GetComponent<PowerRelay>();
            isBase = true;
            modulesRoot = this.transform;

            entranceHatch = Helpers.FindChild(gameObject, "MainDoorEnter");
            entranceHatch.AddComponent<ShipEntranceDoor>();

            exitHatch = Helpers.FindChild(gameObject, "MainDoorExit").AddComponent<ShipExitDoor>();

            lightControl = GetComponentInChildren<LightingController>();

            // embark ladder
            embarkLadder = Helpers.FindChild(gameObject, "EmbarkLadder").AddComponent<ShipLadder>();
            embarkLadder.interactText = "Embark Sea Voyager";
            embarkLadder.SetAsMainEmbarkLadder(this);

            var embarkCinematic = Helpers.FindChild(gameObject, "EmbarkCinematic").AddComponent<ShipCinematic>();
            embarkCinematic.Initialize("cyclops_ladder_long_up", "cinematic", 1.9f, climbUpLongSound, embarkLadder.transform.GetChild(0));

            embarkLadder.cinematic = embarkCinematic;

            // disembark ladder
            disembarkLadder = Helpers.FindChild(gameObject, "DisembarkLadder").AddComponent<ShipLadder>();
            disembarkLadder.interactText = "Disembark";

            // exit lower area ladder
            exitLadder = Helpers.FindChild(gameObject, "ExitLadder").AddComponent<ShipLadder>();
            exitLadder.interactText = "Ascend";

            var exitCinematic = Helpers.FindChild(gameObject, "ExitCinematic").AddComponent<ShipCinematic>();
            exitCinematic.Initialize("cyclops_ladder_long_up", "cinematic", 1.9f, climbUpLongSound, exitLadder.transform.GetChild(0));

            exitLadder.cinematic = exitCinematic;

            // access cockpit loft ladder
            loftLadder = Helpers.FindChild(gameObject, "LoftLadder").AddComponent<ShipLadder>();
            loftLadder.interactText = "Ascend";

            var loftCinematic = Helpers.FindChild(gameObject, "LoftLadderCinematic").AddComponent<ShipCinematic>();
            loftCinematic.Initialize("cyclops_ladder_short_up", "cinematic", 1f, climbUpShortSound, loftLadder.transform.GetChild(0));

            loftLadder.cinematic = loftCinematic;

            // access lower area ladder
            descendLadder = Helpers.FindChild(gameObject, "EntranceLadder").AddComponent<ShipLadder>();
            descendLadder.interactText = "Descend";

            var slideCinematic = Helpers.FindChild(gameObject, "SlideDownCinematic").AddComponent<ShipCinematic>();
            slideCinematic.Initialize("rockethsip_cockpitLadderDown", "cinematic", 5f, slideDownSound, descendLadder.transform.GetChild(0));

            descendLadder.cinematic = slideCinematic;

            // engine room ladder (up)
            engineRoomLadderUp = Helpers.FindChild(gameObject, "EngineRoomLadderUp").AddComponent<ShipLadder>();
            engineRoomLadderUp.interactText = "Ascend";
            
            var engineRoomUpCinematic = Helpers.FindChild(gameObject, "EngineRoomUpCinematic").AddComponent<ShipCinematic>();
            engineRoomUpCinematic.Initialize("cyclops_ladder_short_up", "cinematic", 0.9f, climbUpShortSound, engineRoomLadderUp.transform.GetChild(0));

            engineRoomLadderUp.cinematic = engineRoomUpCinematic;

            // engine room ladder (down)
            engineRoomLadderDown = Helpers.FindChild(gameObject, "EngineRoomLadderDown").AddComponent<ShipLadder>();
            engineRoomLadderDown.interactText = "Descend";

            hud = Helpers.FindChild(gameObject, "PilotCanvas").AddComponent<ShipHUD>();

            shipMove = gameObject.AddComponent<ShipMove>();
            shipMove.ship = this;

            propeller = Helpers.FindChild(gameObject, "Propeller").AddComponent<ShipPropeller>();
            propeller.rotationDirection = new Vector3(0f, 0f, 1f);
            propeller.ship = this;

            voiceNotificationManager = Helpers.FindChild(gameObject, "VoiceSource").AddComponent<VoiceNotificationManager>();
            voiceNotificationManager.subRoot = this;

            slidingDoor1 = Helpers.FindChild(gameObject, "KeyPadDoor1").AddComponent<ShipSlidingDoor>();
            slidingDoor2 = Helpers.FindChild(gameObject, "KeyPadDoor2").AddComponent<ShipSlidingDoor>();

            dock = gameObject.SearchChild("ExosuitDock").AddComponent<SuspendedDock>();
            dock.ship = this;
            dock.Initialize();

            dock2 = gameObject.SearchChild("ExosuitDock2").AddComponent<SuspendedDock>();
            dock2.ship = this;
            dock2.Initialize();

            skyraySpawner = gameObject.SearchChild("SkyraySpawns").AddComponent<SkyraySpawner>();

            PDAEncyclopedia.Add("SeaVoyager", true);
        }

        public override void OnTakeDamage(DamageInfo damageInfo)
        {
            var live = gameObject.GetComponent<LiveMixin>();
            if (live.GetHealthFraction() < 0.5f && this.oldHPPercent >= 0.5f)
            {
                voiceNotificationManager.PlayVoiceNotification(this.hullLowNotification, true, false);
            }
            else if (live.GetHealthFraction() < 0.25f && this.oldHPPercent >= 0.25f)
            {
                voiceNotificationManager.PlayVoiceNotification(this.hullCriticalNotification, true, false);
            }
            oldHPPercent = live.GetHealthFraction();
        }


        void OnKill()
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
