using ModStructureHelperPlugin.Handle.Handles;
using ModStructureHelperPlugin.Mono;
using UnityEngine;

namespace ModStructureHelperPlugin.Editing.Managers;

public class SnappingManager : MonoBehaviour
{
    private Vector3 PositionSnap { get; set; }
    private float RotationSnap { get; set; }
    private Vector3 ScalingSnap { get; set; }
    private HandleSnappingType ScalingSnapType => HandleSnappingType.RELATIVE;
    public bool UseGlobalGrid { get; private set; }
    private Vector3 GlobalGridPosition { get; set; }
    private Vector3 GlobalGridRotation { get; set; }
    public bool SnappingEnabled { get; set; }

    public bool GetUseSnapping()
    {
        if (SnappingEnabled == false)
            return false;
        
        foreach (var selected in SelectionManager.SelectedObjects)
        {
            return selected != null && selected.GetComponent<TransformableGridPlane>() == null;
        }

        return SnappingEnabled;
    }

    public void SetUseGlobalGrid(bool useGlobalGrid)
    {
        UseGlobalGrid = useGlobalGrid;
    }

    public void SetGlobalGridCenter(Vector3 globalGridPosition)
    {
        GlobalGridPosition = globalGridPosition;
    }

    public void SetGlobalGridRotation(Vector3 globalGridRotation)
    {
        GlobalGridRotation = globalGridRotation;
    }
    
    public void SetPositionSnapping(Vector3 vector)
    {
        PositionSnap = vector;
    }

    public void SetPositionSnapping(float spacing)
    {
        PositionSnap = Vector3.one * spacing;
    }

    public void RemovePositionSnapping()
    {
        PositionSnap = Vector3.zero;
    }

    public void SetRotationSnapping(float angle)
    {
        RotationSnap = angle;
    }

    public void RemoveRotationSnapping()
    {
        RotationSnap = 0;
    }

    public Vector3 SnapPlacementPosition(Vector3 position)
    {
        if (!GetUseSnapping() || !UseGlobalGrid)
            return position;

        // Global grid space
        var gridRotation = Quaternion.Euler(GlobalGridRotation);
        var invGridRotation = Quaternion.Inverse(gridRotation);

        var localPos = invGridRotation * (position - GlobalGridPosition);

        if (PositionSnap.x != 0)
            localPos.x = Mathf.Round(localPos.x / PositionSnap.x) * PositionSnap.x;

        if (PositionSnap.y != 0)
            localPos.y = Mathf.Round(localPos.y / PositionSnap.y) * PositionSnap.y;

        if (PositionSnap.z != 0)
            localPos.z = Mathf.Round(localPos.z / PositionSnap.z) * PositionSnap.z;

        // World space
        return gridRotation * localPos + GlobalGridPosition;
    }
    
    public Quaternion SnapPlacementRotation(Quaternion rotation)
    {
        if (!GetUseSnapping() || RotationSnap <= 0f)
            return rotation;

        var gridRotation = Quaternion.Euler(GlobalGridRotation);
        var localRotation = Quaternion.Inverse(gridRotation) * rotation;

        var euler = localRotation.eulerAngles;
        var snap = RotationSnap;

        euler.x = Mathf.Round(euler.x / snap) * snap;
        euler.y = Mathf.Round(euler.y / snap) * snap;
        euler.z = Mathf.Round(euler.z / snap) * snap;

        return gridRotation * Quaternion.Euler(euler);
    }
    
    public Vector3 SnapPositionAxis(Vector3 startPosition, Vector3 offset, Vector3 axis)
    {
        if (!SnappingEnabled)
        {
            return startPosition + offset;
        }
        
        float snap = Vector3.Scale(PositionSnap, axis).magnitude;
        if (snap != 0 && !UseGlobalGrid)
        {
            offset = Mathf.Round(offset.magnitude / snap) * snap * offset.normalized;
        }

        var position = startPosition + offset;

        if (snap != 0 && UseGlobalGrid)
        {
            return SnapPlacementPosition(position);
        }

        return position;
    }

    public Vector3 SnapPositionPlane(Vector3 startPosition, Vector3 offset, Vector3 axis)
    {
        if (!SnappingEnabled)
        {
            return startPosition + offset;
        }
        
        float snap = Vector3.Scale(PositionSnap, axis).magnitude;
        if (snap != 0 && !UseGlobalGrid)
        {
            if (PositionSnap.x != 0) offset.x = Mathf.Round(offset.x / PositionSnap.x) * PositionSnap.x;
            if (PositionSnap.y != 0) offset.y = Mathf.Round(offset.y / PositionSnap.y) * PositionSnap.y;
            if (PositionSnap.z != 0) offset.z = Mathf.Round(offset.z / PositionSnap.z) * PositionSnap.z;
        }

        Vector3 position = startPosition + offset;

        if (snap != 0 && UseGlobalGrid)
        {
            return SnapPlacementPosition(position);
        }

        return position;
    }



    public float SnapRotationAngleRadians(float angleRadians)
    {
        if (!GetUseSnapping() || RotationSnap == 0)
        {
            return angleRadians;
        }

        var angleDegrees = Mathf.Round(angleRadians * Mathf.Rad2Deg / RotationSnap) * RotationSnap;
        return angleDegrees * Mathf.Deg2Rad;
    }

    public float SnapScale(float axisScaleDelta, Vector3 startScale, Vector3 axis)
    {
        if (!GetUseSnapping())
            return axisScaleDelta;
        
        var snap = Mathf.Abs(Vector3.Dot(ScalingSnap, axis));
        if (snap != 0)
        {
            if (ScalingSnapType == HandleSnappingType.RELATIVE)
            {
                axisScaleDelta = Mathf.Round(axisScaleDelta / snap) * snap;
            }
            else
            {
                float axisStartScale = Mathf.Abs(Vector3.Dot(startScale, axis));
                axisScaleDelta = Mathf.Round((axisScaleDelta + axisStartScale) / snap) * snap - axisStartScale;
            }
        }

        return axisScaleDelta;
    }
    
    private Vector3 ToGridSpace(Vector3 world)
    {
        return Quaternion.Inverse(Quaternion.Euler(GlobalGridRotation)) * (world - GlobalGridPosition);
    }

    private Vector3 FromGridSpace(Vector3 grid)
    {
        return Quaternion.Euler(GlobalGridRotation) * grid + GlobalGridPosition;
    }
}