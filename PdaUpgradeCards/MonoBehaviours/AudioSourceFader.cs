using UnityEngine;
using UWE;

namespace PdaUpgradeCards.MonoBehaviours;

public class AudioSourceFader : MonoBehaviour, IManagedUpdateBehaviour
{
    public AudioSource source;
    public float fadeDuration;

    private float _volume = 1f;
    
    private void Start()
    {
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
        _volume -= Time.deltaTime / fadeDuration;
        source.volume = Mathf.Clamp01(_volume);
        if (_volume <= 0)
        {
            Destroy(this);
        }
    }

    public int managedUpdateIndex { get; set; }
}