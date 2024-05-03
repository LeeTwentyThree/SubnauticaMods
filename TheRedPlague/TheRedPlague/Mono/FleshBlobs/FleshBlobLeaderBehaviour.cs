using UnityEngine;

namespace TheRedPlague.Mono.FleshBlobs;

// ADD RED MESMER EFFECT
// RED RADIATION EFFECT
// 0.6 GRAYSCALE
// Split behavior into multiple classes - roam mode and attack mode
public class FleshBlobLeaderBehaviour : MonoBehaviour
{
    public int pathIndex;
    
    public float speed = 4;
    public float inGroundDepth = 15;
    public float rotateSpeed = 0.3f;
    public float upwardsFactor = 1f;

    private int _targetPathIndex;
    private Vector2 _targetPosition;

    private FleshBlobPath _myPath;

    private Transform _vfxParent;

    private void Start()
    {
        var classId = GetComponent<PrefabIdentifier>().classId;
        pathIndex = int.Parse(classId.Substring(classId.Length - 1));
        _myPath = FleshBlobData.Paths[pathIndex];
        var currentIndex = _myPath.GetClosestPointIndex(Get2DPosition);
        SetCurrentTarget(_myPath.GetNextPointIndex(currentIndex));
        _vfxParent = transform.Find("VFX");
    }

    private void Update()
    {
        var pos2d = Get2DPosition;
        var moveTowards = Vector2.MoveTowards(pos2d, _targetPosition, speed * Time.deltaTime);
        var normal = GetUpDirection(pos2d);
        transform.position = new Vector3(moveTowards.x, Mathf.MoveTowards(transform.position.y, GetYPosition(pos2d, normal), Time.deltaTime * 10), moveTowards.y);
        var up = Vector3.Slerp(transform.up, normal, Time.deltaTime * rotateSpeed);
        transform.up = up;
        if (Vector2.Distance(pos2d, _targetPosition) < 10)
        {
            SetCurrentTarget(_myPath.GetNextPointIndex(_targetPathIndex));
        }
        _vfxParent.rotation = Quaternion.identity;
    }

    private void SetCurrentTarget(int index)
    {
        _targetPathIndex = index;
        _targetPosition = _myPath.Points[index];
    }

    private Vector2 Get2DPosition => new(transform.position.x, transform.position.z);

    private float GetYPosition(Vector2 pos, Vector3 normal)
    {
        if (WorldHeightLib.HeightMap.Instance.TryGetValueAtPosition(pos, out var y))
        {
            return y - (normal * inGroundDepth).y;
        }
        return -2000;
    }

    private Vector3 GetUpDirection(Vector2 pos)
    {
        if (WorldHeightLib.NormalMap.Instance.TryGetValueAtPosition(pos, out var normal))
        {
            return (normal + new Vector3(0, upwardsFactor, 0)).normalized;
        }
        return Vector3.up;
    }
}