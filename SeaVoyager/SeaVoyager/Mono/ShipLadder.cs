using UnityEngine;
using Story;

namespace ShipMod.Ship
{
    public class ShipLadder : HandTarget, IHandTarget
    {
        public string interactText;
        public ShipCinematic cinematic;

        private Transform _entrancePosition;
        private SeaVoyager _ship;
        private bool _isMainEmbarkLadder;

        private static string _firstUseStoryGoal = "SeaVoyagerFirstUse";

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
            HandReticle.main.SetInteractText(interactText);
        }

        public void SetAsMainEmbarkLadder(SeaVoyager ship)
        {
            _isMainEmbarkLadder = true;
            _ship = ship;
        }

        private void SetPlayerPosition()
        {
            Player.main.SetPosition(_entrancePosition.position);
            if (_isMainEmbarkLadder)
            {
                if (StoryGoalManager.main.OnGoalComplete(_firstUseStoryGoal))
                {
                    _ship.voice.PlayVoiceLine(ShipVoice.VoiceLine.FirstUse);
                }
            }
        }
    }
}
