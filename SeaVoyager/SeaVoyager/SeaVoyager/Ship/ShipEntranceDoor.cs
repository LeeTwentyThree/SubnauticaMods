using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using SMLHelper.V2.Utility;

namespace ShipMod.Ship
{
    public class ShipEntranceDoor : HandTarget, IHandTarget
    {
        SeaVoyager sub;
        Transform entrancePosition;

        static FMODAsset useDoorSound = Helpers.GetFmodAsset("event:/sub/cyclops/cyclops_door_open");
        
        private void Start()
        {
            sub = GetComponentInParent<SeaVoyager>();
            entrancePosition = transform.GetChild(0);
        }

        public void OnHandClick(GUIHand hand)
        {
            Player.main.SetCurrentSub(sub);
            Player.main.SetPosition(entrancePosition.position);
            Utils.PlayFMODAsset(useDoorSound, transform.position);
            if (sub.HasPower)
            {
                sub.voice.PlayVoiceLine(ShipVoice.VoiceLine.WelcomeAboard);
            }
        }

        public void OnHandHover(GUIHand hand)
        {
            HandReticle.main.SetIcon(HandReticle.IconType.Interact);
            HandReticle.main.SetInteractText("Enter");
        }
    }
}
