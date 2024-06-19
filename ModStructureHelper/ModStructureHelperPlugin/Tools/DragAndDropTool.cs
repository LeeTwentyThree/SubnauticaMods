using UnityEngine;
using UWE;

namespace ModStructureHelperPlugin.Tools;

public class DragAndDropTool : ToolBase
{
    public override ToolType Type => ToolType.DragAndDrop;

    private GameObject _currentlySelected;
    private bool[] _cachedColliderStates;
    private Collider[] _currentlySelectedColliders;
    
    public override bool IncompatibleWithSelectTool => true;

    protected override void OnToolEnabled()
    {
    }

    protected override void OnToolDisabled()
    {
    }

    public override void UpdateTool()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var ray = GetRay();
            if (!Physics.Raycast(ray, out var hit, 5000, -1, QueryTriggerInteraction.Ignore)) return;
            if (SelectionManager.TryGetObjectRoot(hit.collider.gameObject, out var root) == SelectionManager.ObjectRootResult.Success)
            {
                SetCurrentlySelected(root);
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            SetCurrentlySelected(null);
        }
        HandleDrag();
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
    }
    
    private void HandleDrag()
    {
        if (_currentlySelected == null) return;
        var ray = GetRay();
        if (!Physics.Raycast(ray, out var hit, 80, -1, QueryTriggerInteraction.Ignore)) return;
        _currentlySelected.transform.position = hit.point;
        _currentlySelected.transform.up = hit.normal;
    }
}