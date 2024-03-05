using UnityEngine;

namespace DeExtinction.Mono;

public class CreatureSwallowedAnimation : MonoBehaviour
{
    public Transform target;
    public float animationLength;
    private float _timeStarted;
    private float _timeFinished;
    private Vector3 _defaultScale;
    private Vector3 _defaultPosition;

    void Start()
    {
        _timeStarted = Time.time;
        _timeFinished = Time.time + animationLength;
        _defaultScale = transform.localScale;
        _defaultPosition = transform.position;
        foreach (var collider in GetComponentsInChildren<Collider>())
        {
            Destroy(collider);
        }

        var flinch = GetComponent<CreatureFlinch>();
        if (flinch != null)
        {
            flinch.OnTakeDamage(new DamageInfo() {damage = 100f});
        }
    }

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }
        
        var animationProgress = Mathf.InverseLerp(_timeStarted, _timeFinished, Time.time);
        transform.localScale = Vector3.Scale(_defaultScale, Vector3.one - (Vector3.one * animationProgress));
        transform.position = Vector3.Lerp(_defaultPosition, target.transform.position, animationProgress);
    }
}