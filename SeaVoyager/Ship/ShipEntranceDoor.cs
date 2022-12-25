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
        
        void Start()
        {
            sub = GetComponentInParent<SeaVoyager>();
            entrancePosition = transform.GetChild(0);
        }
        public void OnHandClick(GUIHand hand)
        {
            Player.main.SetCurrentSub(sub);
            Player.main.SetPosition(entrancePosition.position);
            GetComponent<AudioSource>().Play();
        }

        public void OnHandHover(GUIHand hand)
        {
            HandReticle.main.SetIcon(HandReticle.IconType.Interact);
            HandReticle.main.SetInteractText("Enter");
        }
    }
}
