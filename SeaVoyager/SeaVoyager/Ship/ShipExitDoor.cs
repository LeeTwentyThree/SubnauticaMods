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
        
        void Start()
        {
            sub = GetComponentInParent<SeaVoyager>();
            entrancePosition = transform.GetChild(0);
        }
        public void OnHandClick(GUIHand hand)
        {
            Player.main.SetCurrentSub(null);
            Player.main.SetPosition(entrancePosition.position);
            GetComponent<AudioSource>().Play();
            if(Random.value > 0.5f)
            {
                sub.skyraySpawner.SpawnSkyrays(Random.Range(3, 6));
            }
        }

        public void OnHandHover(GUIHand hand)
        {
            HandReticle.main.SetIcon(HandReticle.IconType.Interact);
            HandReticle.main.SetInteractText("Exit");
        }
    }
}
