using UnityEngine;
using UnityEngine.UI;

namespace TheRedPlague.Mono;

public class FadingOverlay : MonoBehaviour
    {
        private Image _image;
        private Color _color;
        private float _fadeInTime, _holdDuration, _fadeOutTime, _maxAlpha;

        private float _startTime;

        #region Initialization
        public static FadingOverlay PlayFX(Color color, float fadeInTime, float holdDuration, float fadeOutTime, float maxAlpha = 1f)
        {
            var canvas = CreateCanvas();
            var image = CreateImage(canvas);
            var overlay = canvas.gameObject.AddComponent<FadingOverlay>();
            overlay._image = image;
            overlay._color = color;
            overlay._fadeInTime = fadeInTime;
            overlay._holdDuration = holdDuration;
            overlay._fadeOutTime = fadeOutTime;
            overlay._maxAlpha = maxAlpha;
            return overlay;
        }

        private static Canvas CreateCanvas()
        {
            var canvasObj = new GameObject("CanvasObj");
            var blackoutCanvas = canvasObj.AddComponent<Canvas>();
            blackoutCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
            blackoutCanvas.sortingOrder = 32766;
            blackoutCanvas.gameObject.layer = 5;
            return blackoutCanvas;
        }

        private static Image CreateImage(Canvas canvas)
        {
            var imgObj = new GameObject("Image");
            imgObj.transform.parent = canvas.transform;
            var image = imgObj.AddComponent<Image>();
            image.color = new Color(0f, 0f, 0f, 0f);
            var rt = imgObj.EnsureComponent<RectTransform>();
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.offsetMin = Vector2.zero;
            rt.offsetMax = Vector2.zero;
            return image;
        }
        #endregion

        private void Start()
        {
            _startTime = Time.time;
        }

        private void Update()
        {
            if (Time.time < _startTime + _fadeInTime)
            {
                _image.color = GetColWithAlpha(Mathf.InverseLerp(_startTime, _startTime + _fadeInTime, Time.time) * _maxAlpha);
            }
            else if (Time.time > _startTime + _fadeInTime + _holdDuration)
            {
                _image.color = GetColWithAlpha((1f - Mathf.InverseLerp(_startTime + _fadeInTime + _holdDuration, _startTime + _fadeInTime + _holdDuration + _fadeOutTime, Time.time)) * _maxAlpha);
            }
            else
            {
                _image.color = GetColWithAlpha(_maxAlpha);
            }
            if (Time.time > _startTime + _fadeInTime + _holdDuration + _fadeOutTime)
            {
                Destroy(gameObject);
            }
        }

        private Color GetColWithAlpha(float alpha)
        {
            return new Color(_color.r, _color.g, _color.b, alpha);
        }
    }