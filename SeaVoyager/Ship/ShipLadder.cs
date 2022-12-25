using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ShipMod.Ship
{
    public class ShipLadder : HandTarget, IHandTarget
    {
        public string interactText;
        Transform entrancePosition;

        void Start()
        {
            entrancePosition = transform.GetChild(0);
        }

        public void OnHandClick(GUIHand hand)
        {
            Player.main.SetPosition(entrancePosition.position);
        }

        public void OnHandHover(GUIHand hand)
        {
            HandReticle.main.SetIcon(HandReticle.IconType.Interact);
            HandReticle.main.SetInteractText(interactText);
        }
    }
}
