﻿using ModStructureHelperPlugin.UI;
using UnityEngine;

namespace ModStructureHelperPlugin.Tools;

public class DragAndDropTool : ToolBase
{
    public override ToolType Type => ToolType.DragAndDrop;

    private GameObject _currentlySelected;
    private bool[] _cachedColliderStates;
    private Collider[] _currentlySelectedColliders;
    
    private float _rotation;

    public bool Dragging => _currentlySelected != null;
    
    public override bool IncompatibleWithSelectTool => true;

    protected override void OnToolEnabled()
    {
    }

    protected override void OnToolDisabled()
    {
    }

    public override void UpdateTool()
    {
        if (Input.GetMouseButtonDown(0) && !StructureHelperUI.main.IsCursorHoveringOverExternalWindows)
        {
            var ray = GetRay();
            if (!Physics.Raycast(ray, out var hit, 5000, -1, QueryTriggerInteraction.Ignore)) return;
            if (SelectionManager.TryGetObjectRoot(hit.collider.gameObject, out var root) == SelectionManager.ObjectRootResult.Success)
            {
                SetCurrentlySelected(root);
            }
            // I'm going to regret this hack surely?
            manager.OnToolStateChangedHandler.Invoke(this, true);
        }
        if (Input.GetMouseButtonUp(0))
        {
            SetCurrentlySelected(null);
            manager.OnToolStateChangedHandler.Invoke(this, true);
        }
        if (Input.GetKey(Plugin.ModConfig.BrushRotateLeft))
        {
            _rotation -= Time.deltaTime / 2f;
        }
        else if (Input.GetKey(Plugin.ModConfig.BrushRotateRight))
        {
            _rotation += Time.deltaTime / 2f;
        }
        HandleDrag();
    }

    protected override void OnDisable()
    {
        SetCurrentlySelected(null);
    }

    private Ray GetRay()
    {
        var ray = MainCamera.camera.ScreenPointToRay(Input.mousePosition);
        // extend the ray to ignore the main character's collider
        return new Ray(ray.origin + MainCamera.camera.transform.forward * 0.5f, ray.direction);
    }

    private void SetCurrentlySelected(GameObject obj)
    {
        if (obj == null)
        {
            if (_currentlySelected != null)
            {
                for (int i = 0; i < _currentlySelectedColliders.Length; i++)
                {
                    if (_currentlySelectedColliders[i]) _currentlySelectedColliders[i].enabled = _cachedColliderStates[i];
                }
            }

            _currentlySelected = null;
            return;
        }
        _currentlySelected = obj;
        _currentlySelectedColliders = obj.GetComponentsInChildren<Collider>(true);
        _cachedColliderStates = new bool[_currentlySelectedColliders.Length];
        for (int i = 0; i < _currentlySelectedColliders.Length; i++)
        {
            _cachedColliderStates[i] = _currentlySelectedColliders[i].enabled;
            _currentlySelectedColliders[i].enabled = false;
        }

        _rotation = 0;
    }
    
    private void HandleDrag()
    {
        if (_currentlySelected == null) return;
        var ray = GetRay();
        if (!Physics.Raycast(ray, out var hit, 80, -1, QueryTriggerInteraction.Ignore)) return;
        _currentlySelected.transform.position = hit.point;
        _currentlySelected.transform.up = hit.normal;
        _currentlySelected.transform.Rotate(Vector3.up, _rotation * 360, Space.Self);
    }
}