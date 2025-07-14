using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace SeaVoyager.Mono
{
    public class BedRoomWindow : MonoBehaviour
    {
        public SeaVoyager ship;

        public Transform cameraLocation;

        public Camera camera;
        public RawImage rawImage;
        public Transform distanceCheckTransform;
        public Transform bedRoomPortalTransform;

        private float _maxDistance = 9f;
        private float _maxYDifference = 4f;

        [SerializeField] // unity doesn't like private fields on prefabs
        private RenderTexture renderTexture;

        public void SetupPrefab(SeaVoyager ship)
        {
            this.ship = ship;

            cameraLocation = Helpers.FindChild(gameObject, "BedRoomCamera").transform;
            var cameraObject = new GameObject("BedRoomCameraInstance");
            cameraObject.transform.SetParent(cameraLocation.parent);
            cameraObject.transform.localPosition = cameraLocation.localPosition;
            cameraObject.transform.localEulerAngles = cameraLocation.localEulerAngles;
            camera = cameraObject.EnsureComponent<Camera>();
            camera.fieldOfView = 80;
            if (MainCamera.camera)
            {
                camera.cullingMask = MainCamera.camera.cullingMask;
            }
            rawImage = Helpers.FindChild(gameObject, "BedRoomWindowDisplay").GetComponent<RawImage>();
            distanceCheckTransform = Helpers.FindChild(gameObject, "BedRoomDistanceTracker").transform;
            bedRoomPortalTransform = Helpers.FindChild(gameObject, "BedRoomPortalPosition").transform;

            renderTexture = new RenderTexture(850, 550, 16);
            camera.targetTexture = renderTexture;
            rawImage.texture = renderTexture;

            var waterVolumeOnCamera = camera.gameObject.AddComponent<WaterscapeVolumeOnCamera>();
            waterVolumeOnCamera.settings = WaterBiomeManager.main.gameObject.GetComponent<WaterscapeVolume>();
        }

        private void Update()
        {
            var shouldRender = GetShouldRender();
            camera.enabled = shouldRender;
            rawImage.enabled = shouldRender;

            if (shouldRender)
            {
                var playerCamera = MainCameraControl.main.transform;
                Vector3 playerOffsetFromPortal = playerCamera.position - cameraLocation.position;
                camera.transform.position = bedRoomPortalTransform.position + playerOffsetFromPortal;

                float angularDifferenceBetweenPortalRotations =
                    Quaternion.Angle(bedRoomPortalTransform.rotation, cameraLocation.rotation);

                Quaternion portalRotationalDifference =
                    Quaternion.AngleAxis(angularDifferenceBetweenPortalRotations, Vector3.up);
                Vector3 newCameraDirection = portalRotationalDifference * playerCamera.forward;
                camera.transform.rotation = Quaternion.LookRotation(newCameraDirection, Vector3.up);
            }
        }

        private bool GetShouldRender()
        {
            if (!Plugin.config.EnableCabinWindow)
            {
                return false;
            }

            if (!ship.IsOccupiedByPlayer)
            {
                return false;
            }

            if (Mathf.Abs(MainCameraControl.main.transform.position.y - distanceCheckTransform.position.y) >
                _maxYDifference)
            {
                return false;
            }

            if (Vector3.Distance(MainCameraControl.main.transform.position, distanceCheckTransform.position) >
                _maxDistance)
            {
                return false;
            }

            return true;
        }
    }
}