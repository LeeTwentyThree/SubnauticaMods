using UnityEngine;

namespace PodshellLeviathan.Mono;

public class PodshellEyeAnimations : MonoBehaviour, IManagedLateUpdateBehaviour
{
    public Transform[] eyes;
    
    public float degreesPerSecond = 110;

    public bool useLimits;
    public float dotLimit;
    public float maxDistance = 43f;
    
    private Transform[] _eyeLookDummies;

    private Vector3[] _defaultLocalUps;

    private void OnEnable()
    {
        EnsureDummiesExist();
        if (_defaultLocalUps == null || _defaultLocalUps.Length == 0)
        {
            _defaultLocalUps = new Vector3[eyes.Length];
            for (int i = 0; i <  _defaultLocalUps.Length; i++)
            {
                _defaultLocalUps[i] = eyes[i].parent.InverseTransformDirection(eyes[i].up);
            }
        }
        BehaviourUpdateUtils.Register(this);
    }
    
    private void EnsureDummiesExist()
    {
        if (_eyeLookDummies == null)
        {
            _eyeLookDummies = new Transform[eyes.Length];
        }
        
        for (int i = 0; i < _eyeLookDummies.Length; i++)
        {
            if (_eyeLookDummies[i] != null)
                continue;
            
            _eyeLookDummies[i] = new GameObject("EyeLookDummy-" + i).transform;
            _eyeLookDummies[i].forward = eyes[i].up;
        }
    }
    
    private void OnDisable()
    {
        if (_eyeLookDummies != null)
        {
            foreach (var dummy in _eyeLookDummies)
            {
                if (dummy != null)
                {
                    Destroy(dummy.gameObject);
                }
            }
            _eyeLookDummies = null;
        }
        BehaviourUpdateUtils.Deregister(this);
    }

    public string GetProfileTag()
    {
        return "Podshell:PodshellEyeAnimations";
    }

    public void ManagedLateUpdate()
    {
        EnsureDummiesExist();

        var targetPosition = GetTargetPosition();

        for (int i = 0; i < eyes.Length; i++)
        {
            var difference = targetPosition - eyes[i].position;
            var direction = Vector3.Normalize(difference);
            if (Vector3.SqrMagnitude(difference) > maxDistance * maxDistance)
            {
                direction = eyes[i].parent.TransformDirection(_defaultLocalUps[i]);
            }
            else if (useLimits)
            {
                var defaultUpVector = eyes[i].parent.TransformDirection(_defaultLocalUps[i]);
                // if out of range, use default look direction
                if (Vector3.Dot(direction, defaultUpVector) < dotLimit)
                    direction = defaultUpVector;
            }
            _eyeLookDummies[i].rotation = Quaternion.RotateTowards(_eyeLookDummies[i].rotation,
                Quaternion.LookRotation(direction), Time.deltaTime * degreesPerSecond);
            eyes[i].up = _eyeLookDummies[i].forward;
        }
    }

    private Vector3 GetTargetPosition()
    {
        return MainCamera.camera.transform.position;
    }

    public int managedLateUpdateIndex { get; set; }
}