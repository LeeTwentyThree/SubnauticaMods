using SMLHelper.V2.Utility;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ShipMod.Ship
{
    public class ShipExitDoor : HandTarget, IHandTarget
    {
        SeaVoyager sub;
        Transform entrancePosition;

        static FMODAsset useDoorSound = Helpers.GetFmodAsset("event:/sub/cyclops/cyclops_door_close");

        void Start()
        {
            sub = GetComponentInParent<SeaVoyager>();
            entrancePosition = transform.GetChild(0);
        }
        public void OnHandClick(GUIHand hand)
        {
            Player.main.SetCurrentSub(null);
            Player.main.SetPosition(entrancePosition.position);
            if(Random.value > 0.5f)
            {
                sub.skyraySpawner.SpawnSkyrays(Random.Range(3, 6));
            }
            Utils.PlayFMODAsset(useDoorSound, transform.position);
        }

        public void OnHandHover(GUIHand hand)
        {
            HandReticle.main.SetIcon(HandReticle.IconType.Interact);
            HandReticle.main.SetInteractText("Exit");
        }
    }
}
