using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ShipMod.Ship
{
    public class ShipCinematic : MonoBehaviour
    {
        public PlayerCinematicController controller;
        private float length;
        private string playerAnim;
        private string animatorParam;
        private Action finishCinematicAction;
        private FMODAsset sound;
        private Transform endTransform;

        public void Initialize(string playerAnim, string animatorParam, float length, FMODAsset sound = null, Transform endTransform = null)
        {
            this.length = length;
            this.playerAnim = playerAnim;
            this.animatorParam = animatorParam;
            this.sound = sound;
            this.endTransform = endTransform;
        }

        private void Start()
        {
            controller = gameObject.EnsureComponent<PlayerCinematicController>();
            controller.animator = GetComponent<Animator>();
            controller.animParamReceivers = new GameObject[0];
            controller.playerViewAnimationName = playerAnim;
            controller.animParam = animatorParam;
            controller.animatedTransform = transform.GetChild(0);
            controller.endTransform = endTransform;
        }

        public bool PlayCinematic(Action onFinished)
        {
            if (PlayerCinematicController.cinematicModeCount > 0)
            {
                return false;
            }
            if (!Inventory.main.ReturnHeld(true))
            {
                return false;
            }
            finishCinematicAction = onFinished;
            Invoke(nameof(FinishCinematic), length);
            if (sound != null)
            {
                Utils.PlayFMODAsset(sound, Player.main.transform.position);
            }
            controller.StartCinematicMode(Player.main);
            return true;
        }

        private void FinishCinematic()
        {
            controller.OnPlayerCinematicModeEnd();
            if (finishCinematicAction != null)
            {
                finishCinematicAction.Invoke();
            }
        }
    }
}
