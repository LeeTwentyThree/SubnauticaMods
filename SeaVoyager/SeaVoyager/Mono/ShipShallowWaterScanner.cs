using System;
using UnityEngine;

namespace SeaVoyager.Mono
{
    public class ShipShallowWaterScanner : MonoBehaviour, IScheduledUpdateBehaviour
    {
        public SeaVoyager seaVoyager;
        
        private static readonly LayerMask LayerMask = LayerMask.GetMask("TerrainCollider");
        
        private float _timeScanAgain;

        private const float ScanInterval = 0.25f;
        private const float MaxScanDistance = 200f;
        private const float MinDepth = -26f;
        private const float TooCloseThreshold = 20f;

        private static float MinYLevel => MinDepth + Ocean.GetOceanLevel();

        public void ScheduledUpdate()
        {
            if (Time.time < _timeScanAgain) return;
            _timeScanAgain = Time.time + ScanInterval;

            if (!Physics.Raycast(transform.position, transform.forward, out var hit, MaxScanDistance, LayerMask,
                    QueryTriggerInteraction.Ignore)) return;
            
            if (hit.point.y > MinYLevel || hit.distance < TooCloseThreshold)
            {
                OnDetect();
            }
        }

        private void OnEnable()
        {
            UpdateSchedulerUtils.Register(this);
        }

        private void OnDisable()
        {
            UpdateSchedulerUtils.Deregister(this);
        }

        private void OnDetect()
        {
            if (seaVoyager.currentState != ShipState.Idle)
            {
                seaVoyager.voice.PlayVoiceLine(ShipVoice.VoiceLine.ApproachingShallowWater);
            }
        }

        public string GetProfileTag()
        {
            return "SeaVoyager:ShipShallowWaterScanner";
        }

        public int scheduledUpdateIndex { get; set; }
    }
}
