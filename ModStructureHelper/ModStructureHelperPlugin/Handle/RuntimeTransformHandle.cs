using ModStructureHelperPlugin.Editing.Tools;
using ModStructureHelperPlugin.Handle.Handles;
using ModStructureHelperPlugin.Handle.Handles.Position;
using ModStructureHelperPlugin.Handle.Handles.Rotation;
using ModStructureHelperPlugin.Handle.Handles.Scale;
using ModStructureHelperPlugin.Interfaces;
using ModStructureHelperPlugin.UI;
using UnityEngine;
using UnityEngine.Events;

namespace ModStructureHelperPlugin.Handle;

/**
     * Created by Peter @sHTiF Stefcek 21.10.2020
     */
public class RuntimeTransformHandle : MonoBehaviour
{
    public static RuntimeTransformHandle main;
        
    public HandleAxes axes = HandleAxes.XYZ;
    public HandleSpace space = HandleSpace.LOCAL;
    public HandleType type = HandleType.POSITION;
    public HandleSnappingType snappingType = HandleSnappingType.RELATIVE;

    public Vector3 positionSnap = Vector3.zero;
    public float rotationSnap = 0;
    public Vector3 scaleSnap = Vector3.zero;

    public bool autoScale = false;
    public float autoScaleFactor = 1;
    public Camera handleCamera;

    private Vector3 _previousMousePosition;
    private HandleBase _previousAxis;

    private HandleBase _draggingHandle;

    private HandleType _previousType;
    private HandleAxes _previousAxes;

    private PositionHandle _positionHandle;
    private RotationHandle _rotationHandle;
    private ScaleHandle _scaleHandle;

    public Transform Target { get; private set; }

    private bool _targetRbWasKinematic;

    public UnityEvent startedDraggingHandle = new UnityEvent();
    public UnityEvent isDraggingHandle = new UnityEvent();
    public UnityEvent endedDraggingHandle = new UnityEvent();

    [SerializeField] private bool disableWhenNoTarget;

    void Start()
    {
        main = this;
            
        if (handleCamera == null)
            handleCamera = MainCamera.camera;

        _previousType = type;

        if (Target == null)
            Target = transform;

        if (disableWhenNoTarget && Target == transform)
            gameObject.SetActive(false);

        CreateHandles();
    }

    void CreateHandles()
    {
        switch (type)
        {
            case HandleType.POSITION:
                _positionHandle = gameObject.AddComponent<PositionHandle>().Initialize(this);
                break;
            case HandleType.ROTATION:
                _rotationHandle = gameObject.AddComponent<RotationHandle>().Initialize(this);
                break;
            case HandleType.SCALE:
                _scaleHandle = gameObject.AddComponent<ScaleHandle>().Initialize(this);
                break;
        }
    }

    void Clear()
    {
        _draggingHandle = null;

        if (_positionHandle) _positionHandle.Destroy();
        if (_rotationHandle) _rotationHandle.Destroy();
        if (_scaleHandle) _scaleHandle.Destroy();
    }

    void Update()
    {
        if (Target == null)
        {
            gameObject.SetActive(false);
            return;
        }
            
        if (Plugin.ModConfig.ConsistentHandleScales)
            transform.localScale =
                Vector3.one * (Vector3.Distance(handleCamera.transform.position, transform.position) * Plugin.ModConfig.HandleScaleMultiplier) / 20;

        if (_previousType != type || _previousAxes != axes)
        {
            Clear();
            CreateHandles();
            _previousType = type;
            _previousAxes = axes;
        }

        HandleBase handle = null;
        Vector3 hitPoint = Vector3.zero;
        GetHandle(ref handle, ref hitPoint);

        HandleOverEffect(handle, hitPoint);

        if (PointerIsDown() && _draggingHandle != null)
        {
            _draggingHandle.Interact(_previousMousePosition);
            isDraggingHandle.Invoke();
        }

        if (GetPointerDown() && handle != null)
        {
            _draggingHandle = handle;
            _draggingHandle.StartInteraction(hitPoint);
            startedDraggingHandle.Invoke();
            OnStartTransforming();
        }

        if (GetPointerUp() && _draggingHandle != null)
        {
            _draggingHandle.EndInteraction();
            _draggingHandle = null;
            endedDraggingHandle.Invoke();
            OnFinishTransforming();
        }

        _previousMousePosition = GetMousePosition();
            
        transform.position = Target.transform.position;
        if (space == HandleSpace.LOCAL || type == HandleType.SCALE)
        {
            transform.rotation = Target.transform.rotation;
        }
        else
        {
            transform.rotation = Quaternion.identity;
        }
    }
        
    // both of the following methods expect Target to be not null and valid

    private void OnStartTransforming()
    {
        foreach (var transformationListener in Target.GetComponents<ITransformationListener>())
            transformationListener.OnStartTransforming();
    }
        
    private void OnFinishTransforming()
    {
        foreach (var onFinishTransformation in Target.GetComponents<ITransformationListener>())
            onFinishTransformation.OnFinishTransforming();
    }

    public static bool GetPointerDown()
    {
        return GameInput.GetButtonDown(StructureHelperInput.Interact);
    }

    public static bool PointerIsDown()
    {
        return GameInput.GetButtonHeld(StructureHelperInput.Interact);
    }

    public static bool GetPointerUp()
    {
        return GameInput.GetButtonUp(StructureHelperInput.Interact);
    }

    public static Vector3 GetMousePosition()
    {
        return Input.mousePosition;
    }

    private void OnDisable()
    {
        if (Target != null && _draggingHandle != null)
        {
            OnFinishTransforming();
        }
    }

    void HandleOverEffect(HandleBase p_axis, Vector3 p_hitPoint)
    {
        if (_draggingHandle == null && _previousAxis != null && (_previousAxis != p_axis || !_previousAxis.CanInteract(p_hitPoint)))
        {
            _previousAxis.SetDefaultColor();
            _previousAxis.IsHovering = false;
        }

        if (p_axis != null && _draggingHandle == null && p_axis.CanInteract(p_hitPoint))
        {
            p_axis.SetColor(Color.yellow);
            p_axis.IsHovering = true;
        }

        _previousAxis = p_axis;
    }

    private void GetHandle(ref HandleBase p_handle, ref Vector3 p_hitPoint)
    {
        Ray ray = Camera.main.ScreenPointToRay(GetMousePosition());
        RaycastHit[] hits = Physics.RaycastAll(ray);
        if (hits.Length == 0)
            return;

        foreach (RaycastHit hit in hits)
        {
            p_handle = hit.collider.gameObject.GetComponentInParent<HandleBase>();

            if (p_handle != null)
            {
                p_hitPoint = hit.point;
                return;
            }
        }
    }

    static public RuntimeTransformHandle Create(Transform p_target, HandleType p_handleType)
    {
        RuntimeTransformHandle runtimeTransformHandle = new GameObject().AddComponent<RuntimeTransformHandle>();
        runtimeTransformHandle.Target = p_target;
        runtimeTransformHandle.type = p_handleType;

        return runtimeTransformHandle;
    }

    #region public methods to control handles
    public void SetTarget(Transform newTarget)
    {
        var canActivate = false;
        foreach (var tool in StructureHelperUI.main.toolManager.tools)
        {
            if (!tool.ToolEnabled) continue;
            if (tool.Type is ToolType.Rotate or ToolType.Scale or ToolType.Translate)
            {
                canActivate = true;
            }
        }
        gameObject.SetActive(canActivate && newTarget != null);
        if (Target != null)
        {
            var rb = Target.gameObject.GetComponent<Rigidbody>();
            if (rb) rb.isKinematic = _targetRbWasKinematic;
        }
        Target = newTarget;
        if (newTarget == null) return;
        var newRb = newTarget.gameObject.GetComponent<Rigidbody>();
        _targetRbWasKinematic = !newRb || newRb.isKinematic;
        if (newRb) newRb.isKinematic = true;
    }
        
    /*
        public void SetTarget(GameObject newTarget)
        {
            Target = newTarget.transform;

            if (Target == null)
                Target = transform;

            if (disableWhenNoTarget && Target == transform)
                gameObject.SetActive(false);
            else if(disableWhenNoTarget && Target != transform)
                gameObject.SetActive(true);
        }
        */

    public void SetHandleMode(int mode)
    {
        SetHandleMode((HandleType)mode);
    }

    public void SetHandleMode(HandleType mode)
    {
        type = mode;
    }

    public void EnableXAxis(bool enable)
    {
        if (enable)
            axes |= HandleAxes.X;
        else
            axes &= ~HandleAxes.X;
    }

    public void EnableYAxis(bool enable)
    {
        if (enable)
            axes |= HandleAxes.Y;
        else
            axes &= ~HandleAxes.Y;
    }

    public void EnableZAxis(bool enable)
    {
        if (enable)
            axes |= HandleAxes.Z;
        else
            axes &= ~HandleAxes.Z;
    }

    public void SetAxis(HandleAxes newAxes)
    {
        axes = newAxes;
    }

    public bool GetIsAnyHandleHovered()
    {
        if (!isActiveAndEnabled) return false;

        return type switch
        {
            HandleType.POSITION => _positionHandle.GetIsAnyHandleHovered(),
            HandleType.ROTATION => _rotationHandle.GetIsAnyHandleHovered(),
            HandleType.SCALE => _scaleHandle.GetIsAnyHandleHovered(),
            _ => false
        };
    }
    #endregion
}