using ModStructureHelperPlugin.Editing.Managers;
using ModStructureHelperPlugin.StructureHandling;
using ModStructureHelperPlugin.UI;
using UnityEngine;

namespace ModStructureHelperPlugin.Editing.Tools;

public class DragAndDropTool : ToolBase
{
    public override ToolType Type => ToolType.DragAndDrop;

    private GameObject _currentlySelected;
    private bool[] _cachedColliderStates;
    private Collider[] _currentlySelectedColliders;

    private Vector3 _initialUpDir;
    private bool _upDirChanged;
    
    private float _rotation;
    private float _scaleOffset;
    private Vector3 _previousSelectedEntityScale;
    private UpDirection _upDirection;

    public bool Dragging => _currentlySelected != null;
    
    public override bool IncompatibleWithSelectTool => true;

    protected override void OnToolEnabled()
    {
        ((UndoTool) manager.GetTool(ToolType.Undo)).OnUndoHandler += OnUndo;
    }

    protected override void OnToolDisabled()
    {
        ((UndoTool) manager.GetTool(ToolType.Undo)).OnUndoHandler -= OnUndo;
    }

    public override void UpdateTool()
    {
        if (GameInput.GetButtonDown(StructureHelperInput.Interact) && !StructureHelperUI.main.IsCursorHoveringOverExternalWindows)
        {
            var ray = GetRay();
            if (!Physics.Raycast(ray, out var hit, 5000, -1, QueryTriggerInteraction.Ignore)) return;
            if (SelectionManager.TryGetObjectRoot(hit.collider.gameObject, out var root, SelectionManager.SelectionFilterMode.AllowTransformableObjects) == SelectionManager.ObjectRootResult.Success)
            {
                SetCurrentlySelected(root);
            }
            // I'm going to regret this hack surely?
            manager.OnToolStateChangedHandler?.Invoke(this, true);
        }
        if (GameInput.GetButtonUp(StructureHelperInput.Interact))
        {
            SetCurrentlySelected(null);
            manager.OnToolStateChangedHandler?.Invoke(this, true);
        }
        if (GameInput.GetButtonHeld(StructureHelperInput.BrushRotateLeft))
        {
            _rotation -= Time.deltaTime / 2f * Plugin.ModConfig.BrushRotateSpeed;
            _upDirChanged = true;
        }
        else if (GameInput.GetButtonHeld(StructureHelperInput.BrushRotateRight))
        {
            _rotation += Time.deltaTime / 2f * Plugin.ModConfig.BrushRotateSpeed;
            _upDirChanged = true;
        }
        if (GameInput.GetButtonHeld(StructureHelperInput.BrushDecreaseScale))
        {
            _scaleOffset = Mathf.Max(-1f, _scaleOffset - Time.deltaTime * Plugin.ModConfig.BrushScaleSpeed);
        }
        else if (GameInput.GetButtonHeld(StructureHelperInput.BrushIncreaseScale))
        {
            _scaleOffset = Mathf.Max(-1f, _scaleOffset + Time.deltaTime * Plugin.ModConfig.BrushScaleSpeed);
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
        return new Ray(ray.origin + ray.direction * 0.5f, ray.direction);
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
        _currentlySelectedColliders = _currentlySelected.GetComponentsInChildren<Collider>(true);
        _cachedColliderStates = new bool[_currentlySelectedColliders.Length];
        for (int i = 0; i < _currentlySelectedColliders.Length; i++)
        {
            _cachedColliderStates[i] = _currentlySelectedColliders[i].enabled;
            _currentlySelectedColliders[i].enabled = false;
        }

        _rotation = 0;
        _scaleOffset = 0;
        _previousSelectedEntityScale = obj.transform.localScale;
        
        _upDirChanged = false;
        var prefabIdentifier = obj.GetComponent<PrefabIdentifier>();
        _upDirection = prefabIdentifier == null
            ? UpDirection.Y
            : PrefabUpDirectionManager.GetUpDirectionForPrefab(prefabIdentifier.ClassId);
        _initialUpDir = _upDirection == UpDirection.Y
            ? _currentlySelected.transform.up
            : _currentlySelected.transform.forward; 
        var entityInstance = _currentlySelected.GetComponent<EntityInstance>();
        if (entityInstance) entityInstance.ManagedEntity.CreateAndSaveSnapshot();
        var transformableObject = _currentlySelected.GetComponent<TransformableObject>();
        if (transformableObject) transformableObject.CreateAndSaveSnapshot();
    }
    
    private void HandleDrag()
    {
        if (_currentlySelected == null) return;
        var ray = GetRay();
        if (!Physics.Raycast(ray, out var hit, 80, -1, QueryTriggerInteraction.Ignore)) return;
        var surfaceNormal = GameInput.GetButtonHeld(StructureHelperInput.UseGlobalUpNormal) ? Vector3.up : hit.normal;
        _currentlySelected.transform.position = manager.snappingManager.SnapPlacementPosition(hit.point);
        if (!_upDirChanged)
        {
            _upDirChanged = !Mathf.Approximately(surfaceNormal.x, _initialUpDir.x) ||
                                             !Mathf.Approximately(surfaceNormal.y, _initialUpDir.y) ||
                                             !Mathf.Approximately(surfaceNormal.z, _initialUpDir.z);
        }
        
        if (_upDirChanged)
        {
            if (_upDirection == UpDirection.Z)
            {
                _currentlySelected.transform.forward = surfaceNormal;
            }
            else
            {
                _currentlySelected.transform.up = surfaceNormal;
            }
        }

        _currentlySelected.transform.Rotate(_upDirection == UpDirection.Z ? Vector3.forward : Vector3.up,
            _rotation * 360, Space.Self);
        
        _currentlySelected.transform.rotation = manager.snappingManager.SnapPlacementRotation(_currentlySelected.transform.rotation);

        _currentlySelected.transform.localScale = _previousSelectedEntityScale * (1 + _scaleOffset);
    }

    private void OnUndo()
    {
        SetCurrentlySelected(null);
    }
}