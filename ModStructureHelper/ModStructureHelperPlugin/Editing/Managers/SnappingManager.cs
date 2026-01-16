using ModStructureHelperPlugin.Handle.Handles;
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
        if (!SnappingEnabled || !UseGlobalGrid)
        {
            return position;
        }
        
        return new Vector3(
            Mathf.Round((position.x - GlobalGridPosition.x) / PositionSnap.x) * PositionSnap.x + GlobalGridPosition.x,
            Mathf.Round((position.y - GlobalGridPosition.y) / PositionSnap.y) * PositionSnap.y + GlobalGridPosition.y,
            Mathf.Round((position.z - GlobalGridPosition.z) / PositionSnap.z) * PositionSnap.z + GlobalGridPosition.z);
    }
    
    public Quaternion SnapPlacementRotation(Quaternion rotation)
    {
        if (!SnappingEnabled || RotationSnap <= 0f)
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
            if (PositionSnap.x != 0) position.x = Mathf.Round(position.x / PositionSnap.x) * PositionSnap.x;
            if (PositionSnap.y != 0) position.y = Mathf.Round(position.y / PositionSnap.y) * PositionSnap.y;
            if (PositionSnap.x != 0) position.z = Mathf.Round(position.z / PositionSnap.z) * PositionSnap.z;
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
            if (PositionSnap.x != 0) position.x = Mathf.Round(position.x / PositionSnap.x) * PositionSnap.x;
            if (PositionSnap.y != 0) position.y = Mathf.Round(position.y / PositionSnap.y) * PositionSnap.y;
            if (PositionSnap.x != 0) position.z = Mathf.Round(position.z / PositionSnap.z) * PositionSnap.z;
        }

        return position;
    }

    public float SnapRotationAngleRadians(float angleRadians)
    {
        if (!SnappingEnabled || RotationSnap == 0)
        {
            return angleRadians;
        }

        var angleDegrees = Mathf.Round(angleRadians * Mathf.Rad2Deg / RotationSnap) * RotationSnap;
        return angleDegrees * Mathf.Deg2Rad;
    }

    public float SnapScale(float axisScaleDelta, Vector3 startScale, Vector3 axis)
    {
        if (!SnappingEnabled)
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
}