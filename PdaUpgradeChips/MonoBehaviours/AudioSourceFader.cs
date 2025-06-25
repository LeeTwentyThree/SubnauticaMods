using UnityEngine;
using UWE;

namespace PdaUpgradeChips.MonoBehaviours;

public class AudioSourceFader : MonoBehaviour, IManagedUpdateBehaviour
{
    public AudioSource source;
    public float fadeDuration;

    private float _volume;
    private float _startingVolume;
    
    private void Start()
    {
        _startingVolume = source.volume;
        _volume = _startingVolume;
        BehaviourUpdateUtils.Register(this);
    }

    private void OnDestroy()
    {
        BehaviourUpdateUtils.Deregister(this);
    }

    public string GetProfileTag()
    {
        return "SoundTimeTracker";
    }

    public void ManagedUpdate()
    {
        if (FreezeTime.ShouldPauseMusic()) return;
        _volume -= Time.deltaTime * _startingVolume / fadeDuration;
        source.volume = Mathf.Clamp01(_volume);
        if (_volume <= 0)
        {
            Destroy(this);
        }
    }

    public int managedUpdateIndex { get; set; }
}