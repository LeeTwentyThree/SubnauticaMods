using UnityEngine;
using Story;

namespace SeaVoyager.Mono
{
    public class ShipLadder : HandTarget, IHandTarget
    {
        public string interactText;
        public ShipCinematic cinematic;

        private Transform _entrancePosition;
        private SeaVoyager _ship;
        
        public bool isMainEmbarkLadder;

        private const string FirstUseStoryGoal = "SeaVoyagerFirstUse";

        private void Start()
        {
            _entrancePosition = transform.GetChild(0);
        }

        public void OnHandClick(GUIHand hand)
        {
            if (cinematic == null)
            {
                SetPlayerPosition();
            }
            else
            {
                if (!cinematic.PlayCinematic(SetPlayerPosition))
                {
                    SetPlayerPosition();
                }
            }
        }

        public void OnHandHover(GUIHand hand)
        {
            HandReticle.main.SetIcon(HandReticle.IconType.Interact);
            HandReticle.main.SetText(HandReticle.TextType.Hand, interactText, true);
        }

        public void SetAsMainEmbarkLadder(SeaVoyager ship)
        {
            isMainEmbarkLadder = true;
            _ship = ship;
        }

        private void SetPlayerPosition()
        {
            Player.main.SetPosition(_entrancePosition.position);
            
            if (isMainEmbarkLadder)
            {
                if (StoryGoalManager.main.OnGoalComplete(FirstUseStoryGoal))
                {
                    _ship.voice.PlayVoiceLine(ShipVoice.VoiceLine.FirstUse);
                }
            }
        }
    }
}
