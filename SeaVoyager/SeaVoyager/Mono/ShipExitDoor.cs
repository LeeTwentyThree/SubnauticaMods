using UnityEngine;

namespace SeaVoyager.Mono
{
    public class ShipExitDoor : HandTarget, IHandTarget
    {
        SeaVoyager sub;
        Transform entrancePosition;

        static FMODAsset useDoorSound = Helpers.GetFmodAsset("event:/sub/cyclops/cyclops_door_close");

        void Start()
        {
            sub = GetComponentInParent<SeaVoyager>();
            Plugin.Logger.LogDebug($"{sub}");
            entrancePosition = transform.GetChild(0);
            Plugin.Logger.LogDebug($"{entrancePosition}");
        }
        public void OnHandClick(GUIHand hand)
        {
            Player.main.SetCurrentSub(null, true);
            if (entrancePosition == null) return;
            Player.main.SetPosition(entrancePosition.position);
            if (sub == null) return;
            if(Random.value > 0.5f)
            {
                sub.skyraySpawner.SpawnSkyrays(Random.Range(3, 6));
            }
            Utils.PlayFMODAsset(useDoorSound, transform.position);
        }

        public void OnHandHover(GUIHand hand)
        {
            HandReticle.main.SetIcon(HandReticle.IconType.Interact);
            HandReticle.main.SetText(HandReticle.TextType.Hand,"SeaVoyager_Exit", true);
        }
    }
}
