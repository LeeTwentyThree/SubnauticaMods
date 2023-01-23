using UnityEngine;

namespace SubnauticaRuntimeEditor.Core.ObjectView
{
    public sealed class RenderedObject : MonoBehaviour
    {
        private Camera _renderCamera;

        private RenderTexture renderTexture;

        private Bounds objectSize;

        private float cameraDistance;

        private Vector3 viewAngle;

        public RenderTexture Setup()
        {
            _renderCamera = new GameObject("Render Camera").AddComponent<Camera>();
            _renderCamera.gameObject.SetActive(false);
            renderTexture = new RenderTexture(512, 512, 0);
            _renderCamera.targetTexture = renderTexture;
            _renderCamera.clearFlags = CameraClearFlags.SolidColor;
            _renderCamera.backgroundColor = Color.clear;
            var wboit = _renderCamera.gameObject.AddComponent<WBOIT>();
            wboit.compositeShader = Shader.Find("Hidden/WBOIT Composite");
            viewAngle = new Vector3(-0.1f, 0.1f, 1).normalized;
            objectSize = DetermineObjectSize();
            cameraDistance = DetermineCameraDistance(objectSize);

            _renderCamera.gameObject.SetActive(true);
            return renderTexture;
        }

        private void OnDisable()
        {
            Destroy(_renderCamera.gameObject);
            renderTexture.Release();
        }

        private void Update()
        {
            OrientCamera();
        }

        private void OrientCamera()
        {
            _renderCamera.transform.position = transform.position + viewAngle * cameraDistance;
            _renderCamera.transform.LookAt(transform);
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
            return Mathf.Clamp(distance, 2f, 100f);
        }
    }
}
