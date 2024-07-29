using Nautilus.Utility;
using UnityEngine;

namespace TheRedPlague.Mono.PlagueCyclops;

public class WelcomeAboardWreckTrigger : MonoBehaviour
{
    private bool _triggered;

    public FMOD_CustomEmitter emitter;

    public Pickupable plagueCyclopsCore;
    
    private readonly FMODAsset _welcomeAboard = AudioUtils.GetFmodAsset("PlagueCyclopsWelcomeAboardGlitched");
    private readonly FMODAsset _wreckLoop = AudioUtils.GetFmodAsset("PlagueCyclopsWreckSpeech");
    private readonly FMODAsset _deathSound = AudioUtils.GetFmodAsset("PlagueCyclopsDeath");

    private bool _entered;
    private float _timePlayLineAgain;

    private bool _died;
    
    private void OnTriggerEnter(Collider other)
    {
        if (_triggered) return;
        if (other.gameObject == Player.main.gameObject)
        {
            _triggered = true;
            _entered = true;
            _timePlayLineAgain = Time.time + 3;
            emitter.SetAsset(_welcomeAboard);
            emitter.Play();
        }
    }

    private void Update()
    {
        if (_died) return;
        if (plagueCyclopsCore == null || !plagueCyclopsCore.gameObject.activeSelf)
        {
            _died = true;
            emitter.SetAsset(_deathSound);
            emitter.Play();
        }
        if (!_died && _entered && Time.time > _timePlayLineAgain)
        {
            emitter.SetAsset(_wreckLoop);
            emitter.Play();
            _timePlayLineAgain = Time.time + 18;
        }
    }
}