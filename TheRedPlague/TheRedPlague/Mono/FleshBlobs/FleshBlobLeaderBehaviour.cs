using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace TheRedPlague.Mono.FleshBlobs;

// ADD RED MESMER EFFECT
// RED RADIATION EFFECT
// 0.6 GRAYSCALE
// Split behavior into multiple classes - roam mode and attack mode
public class FleshBlobLeaderBehaviour : MonoBehaviour
{
    public FleshBlobMovement movement;
    public float wanderSpeed = 4f;
    
    public int pathIndex;
    
    private int _targetPathIndex;

    private FleshBlobPath _myPath;

    private Transform _vfxParent;

    private void Start()
    {
        var classId = GetComponent<PrefabIdentifier>().classId;
        pathIndex = int.Parse(classId.Substring(classId.Length - 1));
        _myPath = FleshBlobData.Paths[pathIndex];
        var currentIndex = _myPath.GetClosestPointIndex(movement.Position2D);
        SetCurrentTarget(_myPath.GetNextPointIndex(currentIndex), wanderSpeed);
        _vfxParent = transform.Find("VFX");
    }

    private void Update()
    {
        if (Vector2.Distance(movement.Position2D, movement.targetPosition) < 10)
        {
            SetCurrentTarget(_myPath.GetNextPointIndex(_targetPathIndex), wanderSpeed);
        }
        _vfxParent.rotation = Quaternion.identity;
    }

    private void SetCurrentTarget(int index, float speed)
    {
        _targetPathIndex = index;
        movement.targetPosition = _myPath.Points[index];
        movement.speed = speed;
    }
}