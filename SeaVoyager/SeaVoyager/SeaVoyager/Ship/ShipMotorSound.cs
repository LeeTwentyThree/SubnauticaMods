using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ShipMod.Ship
{
    public class ShipMotorSound : MonoBehaviour
    {
        public SeaVoyager seaVoyager;
        public FMOD_CustomLoopingEmitter emitter;

        private static FMODAsset engineLoopSlow = Helpers.GetFmodAsset("event:/sub/cyclops/cyclops_loop_slow");
        private static FMODAsset engineLoopNormal = Helpers.GetFmodAsset("event:/sub/cyclops/cyclops_loop_normal");
        private static FMODAsset engineLoopFast = Helpers.GetFmodAsset("event:/sub/cyclops/cyclops_loop_epic_fast");

        private void Update()
        {
            var newAsset = AssetToPlay();
            if (emitter.asset != newAsset)
            {
                emitter.SetAsset(newAsset);
            }
            if (ShouldPlaySound())
            {
                emitter.Play();
            }
            else
            {
                emitter.Stop();
            }
        }

        private bool ShouldPlaySound()
        {
            if (seaVoyager.Idle)
            {
                return false;
            }
            if (Vector3.Distance(seaVoyager.transform.position, MainCameraControl.main.transform.position) > 100f)
            {
                return false;
            }
            return true;
        }

        private FMODAsset AssetToPlay()
        {
            switch (seaVoyager.speedSetting)
            {
                default:
                    return engineLoopNormal;
                case ShipSpeedSetting.Fast:
                    return engineLoopFast;
                case ShipSpeedSetting.Slow:
                    return engineLoopSlow;
            }
        }
    }
}
