using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

namespace ShipMod.Ship
{
    public class SonarCam : MonoBehaviour
    {
        private Camera _cam;
        private SonarScreenFX _screenFx;
        private float _pingInterval = 5f;
        private float _timeLastPing;

        private void Start()
        {
            _cam = gameObject.GetComponent<Camera>();
            _screenFx = gameObject.EnsureComponent<SonarScreenFX>();
            _screenFx._shader = Shader.Find("Image Effects/Sonar");
        }

        private void Update()
        {
            if (!_cam.enabled)
            {
                return;
            }
            if (Time.time > _timeLastPing)
            {
                _screenFx.Ping();
                _timeLastPing = Time.time + _pingInterval;
            }
        }
    }
}
