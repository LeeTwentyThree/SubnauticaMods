using UnityEngine;

namespace TheRedPlague.Mono.VFX;

public class ScaleToSize : MonoBehaviour
{
    public float scaleSpeed;

    private float _currentScale;
    private float _targetSize;

    private void Start()
    {
        _currentScale = transform.localScale.x;
        if(_targetSize == 0) _targetSize = transform.localScale.x;
    }

    private void Update()
    {
        _currentScale = Mathf.MoveTowards(_currentScale, _targetSize, scaleSpeed * Time.deltaTime);
        transform.localScale = new Vector3(_currentScale, _currentScale, _currentScale);
    }

    public void SetNewSize(float newSize)
    {
        _targetSize = newSize;
    }

    public void SetSizeInstantly(float newSize)
    {
        _targetSize = newSize;
        _currentScale = newSize;
    }
}