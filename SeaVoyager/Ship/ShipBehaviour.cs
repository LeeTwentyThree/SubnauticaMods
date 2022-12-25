using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.Reflection;

namespace ShipMod.Ship
{
    public class ShipBehaviour : SubRoot
    {
        ShipLadder embarkLadder;
        ShipLadder exitLadder;
        ShipLadder descendLadder;
        ShipLadder loftLadder;
        ShipExitDoor exitHatch;
        ShipSlidingDoor slidingDoor1;
        ShipSlidingDoor slidingDoor2;
        SuspendedDock dock;

        public ShipSolarPanel solarPanel;
        public ShipHUD hud;
        public ShipPropeller propeller;
        public ShipMove shipMove;
        float oldHPPercent;

        public ShipState currentState;
        public float rotationAmount;
        public float moveAmount;

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

            embarkLadder = Helpers.FindChild(gameObject, "EmbarkLadder").AddComponent<ShipLadder>();
            embarkLadder.interactText = "Embark";

            exitLadder = Helpers.FindChild(gameObject, "ExitLadder").AddComponent<ShipLadder>();
            exitLadder.interactText = "Ascend";

            loftLadder = Helpers.FindChild(gameObject, "LoftLadder").AddComponent<ShipLadder>();
            loftLadder.interactText = "Ascend";

            descendLadder = Helpers.FindChild(gameObject, "EntranceLadder").AddComponent<ShipLadder>();
            descendLadder.interactText = "Descend";

            hud = Helpers.FindChild(gameObject, "PilotCanvas").AddComponent<ShipHUD>();

            shipMove = gameObject.AddComponent<ShipMove>();
            shipMove.ship = this;

            propeller = Helpers.FindChild(gameObject, "Propeller").AddComponent<ShipPropeller>();
            propeller.rotationDirection = new Vector3(0f, 0f, 1f);
            propeller.ship = this;

            voiceNotificationManager = Helpers.FindChild(gameObject, "VoiceSource").AddComponent<VoiceNotificationManager>();
            voiceNotificationManager.subRoot = this;

            welcomeNotification = AddNotification(QPatch.welcomeSoundAsset, "Welcome aboard captain. All systems online.");
            welcomeNotification.minInterval = 7f;
            noPowerNotification = AddNotification(QPatch.noPowerSoundAsset, "No power. Wait for solar panels to active in the daytime.");
            noPowerNotification.minInterval = 7f;
            enginePowerDownNotification = AddNotification(QPatch.powerDownSoundAsset, "Engine powering down due to emergency.");
            enginePowerDownNotification.minInterval = 5f;

            slidingDoor1 = Helpers.FindChild(gameObject, "KeyPadDoor1").AddComponent<ShipSlidingDoor>();
            slidingDoor2 = Helpers.FindChild(gameObject, "KeyPadDoor2").AddComponent<ShipSlidingDoor>();

            dock = gameObject.SearchChild("ExosuitDock").AddComponent<SuspendedDock>();
            dock.ship = this;
            dock.Initialize();

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

        VoiceNotification AddNotification(FMODAsset asset, string text)
        {
            var component = voiceNotificationManager.gameObject.AddComponent<VoiceNotification>();
            component.sound = asset;
            component.text = text;
            return component;
        }
    }

    public enum ShipState
    {
        Idle,
        Autopiloting,
        Manual,
        Rotating
    }
}
