using System;
using UnityEngine;
using UnityEngine.UI;

namespace PdaUpgradeChips.MonoBehaviours.Upgrades;

internal class MinimapUpgrade : UpgradeChipBase, IManagedUpdateBehaviour, IScheduledUpdateBehaviour
{
    private Camera _camera;
    private RawImage _image;
    private RenderTexture _renderTexture;
    private RectTransform _uiTransform;
    private RectTransform _maskTransform;

    private bool _inAurora;

    public int managedUpdateIndex { get; set; }
    public int scheduledUpdateIndex { get; set; }

    private Location _location;
    private float _scale;
    private float _offsetX;
    private float _offsetY;

    private void Start()
    {
        var cameraObject = new GameObject("MinimapCamera");
        _camera = cameraObject.AddComponent<Camera>();
        cameraObject.transform.forward = Vector3.down;
        _renderTexture = new RenderTexture(256, 256, 0);
        _camera.targetTexture = _renderTexture;
        _camera.cullingMask = LayerMask.GetMask("Default");
        _camera.fieldOfView = Plugin.Config.MinimapFOV;

        var screenCanvas = uGUI.main.transform.Find("ScreenCanvas");
        var ui = Instantiate(Plugin.Bundle.LoadAsset<GameObject>("MinimapCanvas").transform.Find("Content").gameObject,
            screenCanvas, true);
        _uiTransform = ui.GetComponent<RectTransform>();
        _uiTransform.localPosition = Vector3.zero;
        _uiTransform.localScale = Vector3.one;
        _uiTransform.pivot = new Vector2(0.5f, 0.5f);
        ui.name = "Minimap";
        _maskTransform = ui.transform.Find("Mask").GetComponent<RectTransform>();
        _image = _maskTransform.Find("MapDisplay").GetComponent<RawImage>();
        _image.texture = _renderTexture;

        var sonarFx = cameraObject.AddComponent<MinimapPostProcessing>();
        try
        {
            sonarFx.shader = MainCamera.camera.GetComponent<SonarScreenFX>()._shader;
            var material = screenCanvas.Find("HUD/Content/BarsPanel/StatsBackgroundQuintuple")
                .GetComponent<Image>().material;
            foreach (var graphic in ui.GetComponentsInChildren<Graphic>())
            {
                if (graphic is not RawImage)
                    graphic.material = material;
            }
        }
        catch (Exception e)
        {
            Plugin.Logger.LogError("Failed to find base-game references: " + e);
        }

        BehaviourUpdateUtils.Register(this);
        UpdateSchedulerUtils.Register(this);

        UpdateSettings();
        UpdateImageDisplay();
    }

    private void OnDestroy()
    {
        BehaviourUpdateUtils.Deregister(this);
        UpdateSchedulerUtils.Deregister(this);

        _renderTexture.Release();
        Destroy(_renderTexture);
        if (_camera)
            Destroy(_camera.gameObject);
        if (_uiTransform)
            Destroy(_uiTransform.gameObject);
    }

    private void UpdateImageDisplay()
    {
        if (_uiTransform)
            _uiTransform.gameObject.SetActive(ShouldRenderMinimap());
    }

    private bool ShouldRenderMinimap()
    {
        if (!uGUI.main)
            return false;
        var loading = uGUI.main.loading;
        if (!loading)
            return false;
        return !loading.isLoading;
    }

    public void ManagedUpdate()
    {
        UpdateUI();

        _camera.fieldOfView = Plugin.Config.MinimapFOV;
        _camera.transform.eulerAngles = new Vector3(90,
            Plugin.Config.MinimapPointsNorth ? 0 : MainCamera.camera.transform.eulerAngles.y, 0);
        var distAbove = Plugin.Config.MinimapHeightOffset;
        if (MinimapRaycast(Plugin.Config.MinimapHeightOffset, out var raycastDistance))
        {
            distAbove = raycastDistance;
        }

        _camera.transform.position =
            MainCamera.camera.transform.position + Vector3.up * Plugin.Config.MinimapHeightOffset;
        _camera.nearClipPlane = Plugin.Config.MinimapHeightOffset - distAbove + 0.1f;
        _camera.farClipPlane = Mathf.Max(Plugin.Config.MinimapHeightOffset + 10, _inAurora ? 30 : 200f);
    }

    private void UpdateUI()
    {
        var changed = _location != Plugin.Config.Location ||
                      !Mathf.Approximately(_offsetX, Plugin.Config.MinimapOffsetX) ||
                      !Mathf.Approximately(_offsetY, Plugin.Config.MinimapOffsetY) ||
                      !Mathf.Approximately(_scale, Plugin.Config.MinimapScale);
        if (changed)
            UpdateSettings();
    }

    private bool MinimapRaycast(float distanceAbove, out float distance)
    {
        if (Physics.Raycast(new Ray(MainCamera.camera.transform.position, Vector3.up), out var hit,
                distanceAbove,
                _inAurora ? -1 : 1 << LayerID.TerrainCollider,
                QueryTriggerInteraction.Ignore))
        {
            distance = hit.distance;
            return true;
        }

        distance = distanceAbove;
        return false;
    }

    public string GetProfileTag()
    {
        return "MinimapUpgrade";
    }

    public void ScheduledUpdate()
    {
        _inAurora = Player.main.GetBiomeString().StartsWith("crashedShip") ||
                    Player.main.GetBiomeString().StartsWith("generatorRoom");
        UpdateImageDisplay();
    }

    private void UpdateSettings()
    {
        _location = Plugin.Config.Location;
        _offsetX = Plugin.Config.MinimapOffsetX;
        _offsetY = Plugin.Config.MinimapOffsetY;
        _scale = Plugin.Config.MinimapScale;
        switch (_location)
        {
            case Location.TopLeft:
                _uiTransform.anchorMin = new Vector2(0, 1);
                _uiTransform.anchorMax = new Vector2(0, 1);
                break;
            case Location.Top:
                _uiTransform.anchorMin = new Vector2(0.5f, 1);
                _uiTransform.anchorMax = new Vector2(0.5f, 1);
                break;
            case Location.TopRight:
                _uiTransform.anchorMin = new Vector2(1, 1);
                _uiTransform.anchorMax = new Vector2(1, 1);
                break;
            case Location.Right:
                _uiTransform.anchorMin = new Vector2(1, 0.5f);
                _uiTransform.anchorMax = new Vector2(1, 0.5f);
                break;
            case Location.BottomRight:
                _uiTransform.anchorMin = new Vector2(1, 0);
                _uiTransform.anchorMax = new Vector2(1, 0);
                break;
            case Location.Bottom:
                _uiTransform.anchorMin = new Vector2(0.5f, 0);
                _uiTransform.anchorMax = new Vector2(0.5f, 0);
                break;
            case Location.BottomLeft:
                _uiTransform.anchorMin = new Vector2(0, 0);
                _uiTransform.anchorMax = new Vector2(0, 0);
                break;
            case Location.Left:
                _uiTransform.anchorMin = new Vector2(0, 0.5f);
                _uiTransform.anchorMax = new Vector2(0, 0.5f);
                break;
        }

        _uiTransform.sizeDelta = new Vector2(_scale, _scale);
        _uiTransform.anchoredPosition = new Vector2((_offsetX + _scale / 2) * (0.5f - _uiTransform.anchorMin.x) * 2f,
            (_offsetY + _scale / 2) * (0.5f - _uiTransform.anchorMin.y) * 2f);
    }

    public enum Location
    {
        TopLeft,
        Top,
        TopRight,
        Right,
        BottomRight,
        Bottom,
        BottomLeft,
        Left
    }
}