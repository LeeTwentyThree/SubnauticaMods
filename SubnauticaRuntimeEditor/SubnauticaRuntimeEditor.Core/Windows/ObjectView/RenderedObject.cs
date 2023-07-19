using UnityEngine;

namespace SubnauticaRuntimeEditor.Core.ObjectView
{
    public sealed class RenderedObject : MonoBehaviour
    {
        private Camera _renderCamera;

        private RenderTexture renderTexture;

        private Bounds objectSize;

        private float cameraDistance;

        public float horizontalAngle;
        public float verticalAngle;

        private float sensitivity = 1f;

        private Vector3 offset;

        public RenderTexture Setup()
        {
            _renderCamera = new GameObject("Render Camera").AddComponent<Camera>();
            _renderCamera.gameObject.SetActive(false);
            renderTexture = new RenderTexture(512, 512, 0);
            _renderCamera.targetTexture = renderTexture;
            _renderCamera.clearFlags = CameraClearFlags.SolidColor;
            _renderCamera.backgroundColor = Color.clear;
            _renderCamera.allowMSAA = false;
            var wboit = _renderCamera.gameObject.AddComponent<WBOIT>();
            wboit.compositeShader = GetWBOITShader();
            wboit.camera = _renderCamera;
            objectSize = DetermineObjectSize();
            cameraDistance = DetermineCameraDistance(objectSize);

            _renderCamera.gameObject.SetActive(true);

            if (!gameObject.activeSelf) gameObject.SetActive(true);
            return renderTexture;
        }

        private Shader GetWBOITShader()
        {
            var cam = MainCamera.camera;
            if (!cam) return null;
            var wboit = cam.GetComponent<WBOIT>();
            if (!wboit) return null;
            return wboit.compositeShader;
        }

        private void OnDisable()
        {
            Destroy(_renderCamera.gameObject);
            renderTexture.Release();
        }

        private void Update()
        {
            ProcessInput();
            OrientCamera();
        }

        private void ProcessInput()
        {
            if (GameInput.GetKey(KeyCode.Mouse1))
            {
                horizontalAngle += Input.GetAxis("Mouse X") * (Time.deltaTime * sensitivity);
                verticalAngle = Mathf.Clamp(Input.GetAxis("Mouse Y") * (Time.deltaTime * sensitivity), -Mathf.PI, Mathf.PI);
            }

            if (GameInput.GetKey(KeyCode.Mouse2))
            {
                var oX = Input.GetAxis("Mouse X") * (Time.deltaTime * sensitivity);
                var oY = Input.GetAxis("Mouse Y") * (Time.deltaTime * sensitivity);
                offset += _renderCamera.transform.TransformVector(new Vector3(oX, oY, 0f));
            }

            cameraDistance = Mathf.Clamp(cameraDistance - Input.mouseScrollDelta.y * 3f, 1f, 100f);
        }

        private void OrientCamera()
        {
            Vector3 viewAngle = new Vector3(Mathf.Cos(horizontalAngle), Mathf.Sin(-verticalAngle), Mathf.Sin(horizontalAngle));
            _renderCamera.transform.position = transform.position + viewAngle * cameraDistance;
            _renderCamera.transform.LookAt(transform);
            _renderCamera.transform.position += offset;
        }

        private Bounds DetermineObjectSize()
        {
            var bounds = new Bounds(transform.position, new Vector3(2, 2, 2));
            foreach (var renderer in gameObject.GetComponentsInChildren<Renderer>())
            {
                if (renderer.enabled && renderer.gameObject.activeSelf)
                    bounds.Encapsulate(renderer.bounds);
            }
            return bounds;
        }

        private float DetermineCameraDistance(Bounds bounds)
        {
            float distance = 0f;
            if (bounds.extents.x > distance) distance = bounds.extents.x;
            if (bounds.extents.y > distance) distance = bounds.extents.y;
            if (bounds.extents.z > distance) distance = bounds.extents.z;
            return Mathf.Clamp(distance, 2f, 50f);
        }
    }
}
