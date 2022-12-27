using UnityEngine;
using UnityEngine.UI;
using DebugHelper.Systems.Projection;
using System.Collections.Generic;
using DebugHelper.Managers;

namespace DebugHelper.Systems
{
    internal class DebugOverlay : MonoBehaviour
    {
        public static DebugOverlay main;
        public PrimitiveRenderer renderer;
        private RawImage rawImage;
        private Texture2D texture;

        private int lastSavedWidth;
        private int lastSavedHeight;

        public static void CreateInstance()
        {
            var gameObject = Instantiate(Main.assetBundle.LoadAsset<GameObject>("DebugOverlayCanvas"));
            var component = gameObject.AddComponent<DebugOverlay>();
            component.rawImage = gameObject.GetComponentInChildren<RawImage>();
        }

        private void Start()
        {
            main = this;
            RegenerateTexture(CalculateTextureSize());
            renderer = new PrimitiveRenderer();
            renderer.lines = new List<Line>();
        }

        private void RegenerateTexture(Vector2Int scale)
        {
            texture = new Texture2D(scale.x, scale.y, TextureFormat.ARGB32, false, true);
            rawImage.texture = texture;
            lastSavedWidth = scale.x;
            lastSavedHeight = scale.y;
            rawImage.transform.localScale = Vector3.one * Main.config.DebugOverlayResolutionDivisor;
        }

        private void Update()
        {
            if (!Main.config.DebugOverlayEnabled) return;
            var textureSize = CalculateTextureSize();
            if (lastSavedWidth != textureSize.x || lastSavedHeight != textureSize.y)
            {
                RegenerateTexture(textureSize);
            }
        }

        private void LateUpdate()
        {
            if (!Main.config.DebugOverlayEnabled || uGUI.isLoading)
            {
                rawImage.enabled = false;
                return;
            }
            rawImage.enabled = renderer.Rendered;
            renderer.RenderToTexture(texture, true);
        }

        private void Release()
        {
            renderer.Release();
            texture = null;
        }

        private Vector2Int CalculateTextureSize()
        {
            var divisor = Main.config.DebugOverlayResolutionDivisor;
            return new Vector2Int((int)(Screen.width / divisor), (int)(Screen.height / divisor));
        }

        private void OnDestroy()
        {
            Destroy(texture);
        }

        public void AddLineThisFrame(Line line)
        {
            renderer.lines.Add(line);
        }
    }
}
