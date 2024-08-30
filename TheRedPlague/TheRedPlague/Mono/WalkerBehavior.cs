using UnityEngine;
using WorldHeightLib;

namespace TheRedPlague.Mono;

public class WalkerBehavior : MonoBehaviour
{
    public float maxHeightUpdateDelay;
    public float horizontalMoveSpeed;
    // If negative, moves vertically instantly
    public float maxVerticalMoveSpeed;
    public float upwardsNormalFactor = 1f;
    public float rotateSpeed = 0.4f;
    public float depth;
    public float angleOffset;

    private float _timeUpdateHeightAgain;

    private float _currentHeight;
    private Vector3 _currentSurfaceNormal;

    private Vector3 _targetPosition;

    private void Start()
    {
        _currentHeight = transform.position.y;
        _currentSurfaceNormal = Vector3.up;
        _timeUpdateHeightAgain = Time.time + _timeUpdateHeightAgain * Random.value;
    }

    private void Update()
    {
        if (Time.time >= _timeUpdateHeightAgain)
        {
            UpdateHeight();
            _timeUpdateHeightAgain = Time.time + maxHeightUpdateDelay;
        }

        var newX = Mathf.MoveTowards(transform.position.x, _targetPosition.x, Time.deltaTime * horizontalMoveSpeed);
        var newZ = Mathf.MoveTowards(transform.position.z, _targetPosition.z, Time.deltaTime * horizontalMoveSpeed);

        var newY = maxVerticalMoveSpeed < 0
            ? _currentHeight
            : Mathf.MoveTowards(transform.position.y, _currentHeight, Time.deltaTime * maxVerticalMoveSpeed);

        transform.position = new Vector3(newX, newY, newZ);
        
        var up = Vector3.Slerp(transform.up, _currentSurfaceNormal, Time.deltaTime * rotateSpeed);
        transform.up = up;
        transform.Rotate(Vector3.up, angleOffset, Space.Self);
    }

    public void SetTargetPosition(Vector3 newPosition)
    {
        _targetPosition = newPosition;
    }

    private void UpdateHeight()
    {
        var pos = new Vector2(transform.position.x, transform.position.z);
        _currentHeight = HeightMap.Instance.TryGetValueAtPosition(pos, out var height) ? height : transform.position.y;
        _currentHeight -= depth;
        _currentSurfaceNormal = NormalMap.Instance.TryGetValueAtPosition(pos, out var normal)
            ? (normal + new Vector3(0, upwardsNormalFactor, 0)).normalized
            : Vector3.up;
    }
}