﻿using System.Collections;
using ModStructureHelperPlugin.UI;
using UnityEngine;
using UWE;

namespace ModStructureHelperPlugin.Tools;

public class PaintTool : ToolBase
{
    public override ToolType Type => ToolType.PaintBrush;

    public override bool IncompatibleWithSelectTool => true;

    private string _selectedClassId;
    private GameObject _selectedPrefab;
    private bool _loadingPrefab;
    private float _timePrefabLoadingStarted;

    private GameObject _currentPreview;

    private Vector3 _brushPosition;
    private Quaternion _brushRotation;
    private bool _brushLocationValid;

    private const float MaxPrefabLoadingTime = 5;

    private float _rotation;
    private Transform _dummyRotationTransform;
    private UpDirection _upDirection;

    protected override void OnToolEnabled()
    {
        _rotation = 0;
        if (_selectedClassId == null)
        {
            ErrorMessage.AddMessage("To use the brush tool, you must first select a prefab to brush with the entity browser or object picker!");
        }
        if (_selectedPrefab != null)
        {
            CreatePreview();
        }
    }

    protected override void OnToolDisabled()
    {
        Destroy(_currentPreview);
    }

    private void Update()
    {
        if (!ToolEnabled || _selectedPrefab == null || StructureInstance.Main == null)
        {
            Destroy(_currentPreview);
            _currentPreview = null;
            return;
        }

        if (_currentPreview == null)
        {
            CreatePreview();
        }

        if (Input.GetKey(Plugin.ModConfig.BrushRotateLeft))
        {
            _rotation -= Time.deltaTime / 2f;
        }
        else if (Input.GetKey(Plugin.ModConfig.BrushRotateRight))
        {
            _rotation += Time.deltaTime / 2f;
        }

        UpdateBrushPosition();
        
        _currentPreview.SetActive(_brushLocationValid);
        _currentPreview.transform.position = _brushPosition;
        _currentPreview.transform.rotation = _brushRotation;

        if (!_brushLocationValid) return;
        
        if (Input.GetKeyDown(KeyCode.Mouse0) && !StructureHelperUI.main.IsCursorHoveringOverExternalWindows)
        {
            var placedObject = Instantiate(_selectedPrefab, _brushPosition, _brushRotation);
            StructureInstance.Main.RegisterNewEntity(placedObject.GetComponent<PrefabIdentifier>(), true);
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        _dummyRotationTransform = new GameObject("DummyRotationTransform").transform;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        Destroy(_currentPreview);
        Destroy(_dummyRotationTransform.gameObject);
    }

    private void CreatePreview()
    {
        _currentPreview = Instantiate(_selectedPrefab);
        var lwe = _currentPreview.GetComponent<LargeWorldEntity>();
        if (lwe) lwe.enabled = false;
        Destroy(lwe);
        _currentPreview.GetComponentsInChildren<Renderer>().ForEach(renderer => renderer.SetFadeAmount(0.5f));
        ObjectStripper.StripAllChildren(_currentPreview);
        _rotation = 0;
    }

    private void UpdateBrushPosition()
    {
        var ray = MainCamera.camera.ScreenPointToRay(Input.mousePosition);
        var hitSurface = Physics.Raycast(new Ray(ray.origin + MainCamera.camera.transform.forward, ray.direction),
            out var hit, 5000, -1, QueryTriggerInteraction.Ignore);
        _brushPosition = hitSurface
            ? hit.point
            : MainCamera.camera.transform.position + MainCamera.camera.transform.forward * 10;
        if (hitSurface)
        {
            if (_upDirection == UpDirection.Z)
            {
                _dummyRotationTransform.forward = hit.normal;
                _dummyRotationTransform.Rotate(Vector3.forward, _rotation * 360, Space.Self);
            }
            else
            {
                _dummyRotationTransform.up = hit.normal;
                _dummyRotationTransform.Rotate(Vector3.up, _rotation * 360, Space.Self);
            }
            _brushRotation = _dummyRotationTransform.rotation;
        }
        else
        {
            _brushRotation = Quaternion.identity;
        }
        _brushLocationValid = hitSurface;
    }

    public void SetCurrentBrushEntity(string classId)
    {
        if (_loadingPrefab && Time.time < _timePrefabLoadingStarted + MaxPrefabLoadingTime) return;

        if (_selectedPrefab == null) _selectedClassId = null;

        if (_selectedClassId == classId) return;

        _selectedClassId = classId;
        _selectedPrefab = null;
        StartCoroutine(LoadEntity(classId));
        _upDirection = PrefabUpDirectionManager.GetUpDirectionForPrefab(classId);
    }

    private IEnumerator LoadEntity(string classId)
    {
        _loadingPrefab = true;
        _timePrefabLoadingStarted = Time.time;
        var task = PrefabDatabase.GetPrefabAsync(classId);
        yield return task;
        if (!task.TryGetPrefab(out var prefab))
        {
            ErrorMessage.AddMessage($"Failed to load prefab of Class ID '{classId}'!");
            _loadingPrefab = false;
            yield break;
        }

        _selectedPrefab = prefab;
        _loadingPrefab = false;

        if (isActiveAndEnabled)
            CreatePreview();
    }
}