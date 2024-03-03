using UnityEngine;

namespace SeaVoyager.Mono
{
    public class ShipShallowWaterScanner : MonoBehaviour
    {
        public SeaVoyager seaVoyager;
        private static LayerMask _layerMask = LayerMask.GetMask("TerrainCollider");
        private float _scanInterval = 0.25f;
        private float _timeScanAgain;
        private float _maxScanDistance = 200f;
        private float _minDepth = -26f;
        private float _tooCloseThreshold = 20f;

        private float MinYLevel
        {
            get
            {
                return _minDepth + Ocean.GetOceanLevel();
            }
        }

        private void Update()
        {
            if (Time.time > _timeScanAgain) 
            {
                _timeScanAgain = Time.time + _scanInterval;
                if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, _maxScanDistance, _layerMask, QueryTriggerInteraction.Ignore))
                {
                    if (hit.point.y > MinYLevel || hit.distance < _tooCloseThreshold)
                    {
                        OnDetect();
                    }
                }
            }
        }

        private void OnDetect()
        {
            if (seaVoyager.currentState != ShipState.Idle)
            {
                seaVoyager.voice.PlayVoiceLine(ShipVoice.VoiceLine.ApproachingShallowWater);
            }
        }
    }
}
