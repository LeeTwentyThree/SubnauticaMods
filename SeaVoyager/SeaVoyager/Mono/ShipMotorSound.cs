using UnityEngine;

namespace SeaVoyager.Mono
{
    public class ShipMotorSound : MonoBehaviour
    {
        public SeaVoyager seaVoyager;
        public FMOD_CustomLoopingEmitter emitter;

        private static readonly FMODAsset EngineLoopSlow = Helpers.GetFmodAsset("event:/sub/cyclops/cyclops_loop_slow");
        private static readonly FMODAsset EngineLoopNormal = Helpers.GetFmodAsset("event:/sub/cyclops/cyclops_loop_normal");
        private static readonly FMODAsset EngineLoopFast = Helpers.GetFmodAsset("event:/sub/cyclops/cyclops_loop_epic_fast");

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
            if (Vector3.SqrMagnitude(seaVoyager.transform.position - MainCameraControl.main.transform.position) > 100 * 100)
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
                    return EngineLoopNormal;
                case ShipSpeedSetting.Fast:
                    return EngineLoopFast;
                case ShipSpeedSetting.Slow:
                    return EngineLoopSlow;
            }
        }
    }
}
