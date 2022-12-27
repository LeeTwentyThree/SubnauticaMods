using DebugHelper.Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace DebugHelper.Systems
{
    public class DebugIconInstance : MonoBehaviour
    {
        public Image image;
        public Text text;

        private const float kMinDistance = 1f;
        private const float kTransitionDistance = 2f;
        private const float kScaleFactor3D = 2.5f;
        private const float kScaleFactor2D = 0.3f;

        private void Awake()
        {
            image = GetComponentInChildren<Image>();
            text = GetComponentInChildren<Text>();
        }

        public void UpdateAppearance(IDebugIcon reference) // should be called by late update
        {
            var newPos = reference.Position;
            var distanceToCamera = Vector3.Distance(newPos, SNCameraRoot.main.transform.position);
            var screenPoint = SNCameraRoot.main.mainCam.WorldToScreenPoint(newPos);

            var color = reference.Color;
            color = color.ToAlpha(color.a * GetDistanceAlphaBlend(screenPoint.z));

            var sprite = reference.Icon;
            var label = reference.Label;

            UpdatePosition(screenPoint, reference.Scale, distanceToCamera);
            image.color = color;
            image.enabled = sprite != null;
            text.color = color;
            text.text = label;
            text.enabled = !string.IsNullOrEmpty(label);
            image.sprite = sprite;
        }

        private void UpdatePosition(Vector3 screenPoint, float scale, float distance)
        {
            float x = screenPoint.x - Screen.width / 2;
            float y = screenPoint.y - Screen.height / 2;
            transform.localPosition = new Vector3(x, y, 0f);
            var renderedScale = Vector3.one * scale * DetermineScaleFactor() * Main.config.DebugIconScale;
            if (Main.config.DebugIconsAre3D && distance > kMinDistance)
            {
                renderedScale /= distance;
            }
            transform.localScale = renderedScale;
        }

        private static float DetermineScaleFactor()
        {
            if (Main.config.DebugIconsAre3D) return kScaleFactor3D;
            return kScaleFactor2D;
        }

        private float GetDistanceAlphaBlend(float distance)
        {
            if (distance < kMinDistance)
            {
                return 0f;
            }
            if (distance > kMinDistance + kTransitionDistance)
            {
                return 1f;
            }
            return Mathf.Clamp((distance - kMinDistance) / kTransitionDistance, 0f, 1f);
        }
    }
}