using UnityEngine;

namespace TheRedPlague.Mono.FleshBlobs;

public class FleshBlobMovement : MonoBehaviour
{
    public float speed;
    public Vector2 targetPosition;

    public float inGroundDepth = 15;
    public float rotateSpeed = 1f;
    public float upwardsFactor = 1f;

    private float _timeUnfreeze;
    private FleshBlobGrowth _growth;

    private void Start()
    {
        _growth = GetComponent<FleshBlobGrowth>();
    }

    private void Update()
    {
        if (Time.time < _timeUnfreeze) return;
        var pos2d = Position2D;
        var moveTowards = Vector2.MoveTowards(pos2d, targetPosition, speed * Time.deltaTime);
        var normal = GetUpDirection(pos2d);
        transform.position = new Vector3(moveTowards.x, Mathf.MoveTowards(transform.position.y, GetYPosition(pos2d, normal), Time.deltaTime * 10 * _growth.Size), moveTowards.y);
        var up = Vector3.Slerp(transform.up, normal, Time.deltaTime * rotateSpeed);
        transform.up = up;
    }

    public Vector2 Position2D => new(transform.position.x, transform.position.z);
    
    public void FreezeForSeconds(float seconds)
    {
        _timeUnfreeze = Time.time + seconds;
    }
    
    private float GetYPosition(Vector2 pos, Vector3 normal)
    {
        if (WorldHeightLib.HeightMap.Instance.TryGetValueAtPosition(pos, out var y))
        {
            return y - (normal * (inGroundDepth * _growth.Size)).y;
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