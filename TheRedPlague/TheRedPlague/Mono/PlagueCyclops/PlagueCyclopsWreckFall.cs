using Nautilus.Utility;
using UnityEngine;

namespace TheRedPlague.Mono.PlagueCyclops;

public class PlagueCyclopsWreckFall : MonoBehaviour
{
    public const int FallenTentacleRequirement = 4;

    public Rigidbody rigidbody;

    private float _timeCheckAgain;

    public bool fell;

    private bool _falling;
    private bool _rotating;

    private Vector3 _targetPosition = new Vector3(-982.771f, 0.133f, 1734.737f);
    private Vector3 _targetAngle = new Vector3(34, 116, 355);

    private Vector3 _velocity;

    private float _timeStartFalling;

    private float _rotationDuration = 4f;

    private Quaternion _startRotation;
    private Quaternion _endRotation;

    private void Update()
    {
        if (_falling)
        {
            if (Vector3.SqrMagnitude(transform.position - _targetPosition) <= 0.1f * 0.1f)
            {
                _falling = false;
                transform.position = _targetPosition;
            }
            else
            {
                transform.position = transform.position += _velocity * Time.deltaTime;
                _velocity += (_targetPosition - transform.position).normalized * (3f * Time.deltaTime);
            }
        }

        if (_rotating)
        {
            if (Time.time > _timeStartFalling + _rotationDuration)
            {
                _rotating = false;
                transform.eulerAngles = _targetAngle;
            }
            else
            {
                transform.rotation = Quaternion.Slerp(_startRotation, _endRotation, (Time.time - _timeStartFalling) / _rotationDuration);
            }
        }

        if (fell) return;
        if (Time.time < _timeCheckAgain) return;
        if (CyclopsTentacle.GetReleasedCyclopsTentaclesCount() >= FallenTentacleRequirement)
        {
            Release();
        }
        _timeCheckAgain = Time.time + 0.5f;
    }

    private void Release()
    {
        fell = true;
        _falling = true;
        _rotating = true;
        _timeStartFalling = Time.time;
        _startRotation = transform.rotation;
        _endRotation = Quaternion.Euler(_targetAngle);
        Invoke(nameof(PlayFallSound), 2.3f);
    }

    private void PlayFallSound()
    {
        Utils.PlayFMODAsset(AudioUtils.GetFmodAsset("PlagueCyclopsFall"), transform.position);
    }
}