using UnityEngine;

namespace TheRumbling.Mono;

public class RepeatSoundEvery36Seconds : MonoBehaviour
{
    private void Start()
    {
        InvokeRepeating(nameof(Play), 36, 36);
    }

    private void Play()
    {
        GetComponent<FMOD_CustomEmitter>().Play();
    }
}