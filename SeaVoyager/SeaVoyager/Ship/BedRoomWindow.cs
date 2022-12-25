using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace ShipMod.Ship
{
    public class BedRoomWindow : MonoBehaviour
    {
        public SeaVoyager ship;
        public Camera camera;
        public RawImage rawImage;
        public Transform distanceCheckTransform;

        private float _maxDistance = 9f;
        private float _maxYDifference = 4f;

        [SerializeField] // unity doesn't like private fields on prefabs
        private RenderTexture _renderTexture;

        public void SetupPrefab(SeaVoyager ship)
        {
            this.ship = ship;

            camera = Helpers.FindChild(gameObject, "BedRoomCamera").EnsureComponent<Camera>();
            rawImage = Helpers.FindChild(gameObject, "BedRoomWindowDisplay").GetComponent<RawImage>();
            distanceCheckTransform = Helpers.FindChild(gameObject, "BedRoomDistanceTracker").transform;

            _renderTexture = new RenderTexture(850, 550, 16);
            camera.targetTexture = _renderTexture;
            rawImage.texture = _renderTexture;

            var waterVolumeOnCamera = camera.gameObject.AddComponent<WaterscapeVolumeOnCamera>();
            waterVolumeOnCamera.settings = WaterBiomeManager.main.gameObject.GetComponent<WaterscapeVolume>();
        }

        private void Update()
        {
            var shouldRender = GetShouldRender();
            camera.enabled = shouldRender;
            rawImage.enabled = shouldRender;
        }

        private bool GetShouldRender()
        {
            if (!QPatch.config.EnableCabinWindow)
            {
                return false;
            }
            if (!ship.IsOccupiedByPlayer)
            {
                return false;
            }
            if (Mathf.Abs(MainCameraControl.main.transform.position.y - distanceCheckTransform.position.y) > _maxYDifference)
            {
                return false;
            }
            if (Vector3.Distance(MainCameraControl.main.transform.position, distanceCheckTransform.position) > _maxDistance)
            {
                return false;
            }
            return true;
        }
    }
}
