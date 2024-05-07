using UnityEngine;

namespace TheRedPlague.Mono.FleshBlobs;

public class FleshBlobTornadoSounds : MonoBehaviour
{
    public FMOD_CustomEmitter emitter;
    private float _timePlayAgain;
    
    private void Update()
    {
        if (Time.time > _timePlayAgain)
        {
            emitter.Play();
            _timePlayAgain = Time.time + 74;
        }
    }
}