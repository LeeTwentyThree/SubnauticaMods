using UnityEngine;

namespace SeaVoyager.Mono
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
            Player.main.SetCurrentSub(sub, true);
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
            HandReticle.main.SetText(HandReticle.TextType.Hand,"SeaVoyager_Enter", true);
        }
    }
}
