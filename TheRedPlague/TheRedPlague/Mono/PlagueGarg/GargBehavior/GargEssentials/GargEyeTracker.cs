using UnityEngine;

namespace TheRedPlague.Mono.PlagueGarg.GargBehavior.GargEssentials;

public class GargEyeTracker : MonoBehaviour
{
    public GargantuanBehaviour garg;

    private Rigidbody _rb;
    private float _minimumDot = 0.4f;
    private float _anglesPerSecond = 180f;
    private float _anglesPerSecondSnappingBack = 360f;
    private Transform _upReference;
    private Transform _defaultReference;
    private Transform _eyeDummy; // a non-animated object that stores the actual rotations

    private Transform TargetTransform => garg.EyeTrackTarget;

    private void Start()
    {
        _upReference = transform.parent;

        var defaultPosObj = new GameObject(gameObject.name + "-Reference");
        defaultPosObj.transform.parent = transform.parent;
        defaultPosObj.transform.localPosition = transform.localPosition;
        defaultPosObj.transform.localRotation = transform.localRotation;
        _defaultReference = defaultPosObj.transform;

        var dummyObj = new GameObject(gameObject.name + "-Dummy");
        dummyObj.transform.parent = transform.parent;
        dummyObj.transform.localPosition = transform.localPosition;
        dummyObj.transform.localRotation = transform.localRotation;
        _eyeDummy = dummyObj.transform;

        _rb = garg.gameObject.GetComponent<Rigidbody>();
    }

    private void LateUpdate()
    {
        if (TargetTransform == null)
        {
            ApplyDefaultEyeRotation();
            transform.localRotation = _eyeDummy.localRotation;
            return;
        }

        Vector3 lookDir = (TargetTransform.transform.position - _defaultReference.position).normalized;

        bool shouldLookStraight = Vector3.Dot(_defaultReference.up, lookDir) <= _minimumDot;
        if (shouldLookStraight)
        {
            //_eyeDummy.localRotation = Quaternion.RotateTowards(_eyeDummy.localRotation, _defaultReference.localRotation, _anglesPerSecondSnappingBack * Time.deltaTime);
            ApplyDefaultEyeRotation();
        }
        else
        {
            _eyeDummy.rotation = Quaternion.RotateTowards(_eyeDummy.rotation, LookRotationY(_upReference.forward, lookDir), _anglesPerSecond * Time.deltaTime);
        }
        transform.localRotation = _eyeDummy.localRotation;
    }

    private void ApplyDefaultEyeRotation()
    {
        Vector3 newEyeForward = _defaultReference.up;
        if (_rb.velocity.sqrMagnitude > 0.1f)
        {
            newEyeForward = Vector3.Slerp(newEyeForward, _rb.velocity, 0.5f);
        }
        _eyeDummy.rotation = Quaternion.RotateTowards(_eyeDummy.rotation, LookRotationY(_upReference.forward, newEyeForward), _anglesPerSecondSnappingBack * Time.deltaTime);
    }

    private static Quaternion LookRotationY(Vector3 approximateForward, Vector3 exactUp) // thank you DMGregory, i have no idea how you figured this out
    {
        Quaternion zToUp = Quaternion.LookRotation(exactUp, -approximateForward);
        Quaternion yToz = Quaternion.Euler(90, 0, 0);
        return zToUp * yToz;
    }
}