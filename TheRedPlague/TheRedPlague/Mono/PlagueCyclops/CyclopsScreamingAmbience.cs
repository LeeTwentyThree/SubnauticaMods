using Nautilus.Utility;
using UnityEngine;

namespace TheRedPlague.Mono.PlagueCyclops;

public class CyclopsScreamingAmbience : MonoBehaviour
{
    public PlagueCyclopsWreckFall wreck;

    private static readonly FMODAsset _screamSound = AudioUtils.GetFmodAsset("PlagueCyclopsScream");

    private float _timeScreamAgain;
    
    private void Update()
    {
        if (wreck.fell) return;
        if (Time.time > _timeScreamAgain)
        {
            _timeScreamAgain = Time.time + Random.Range(15, 20);
            Utils.PlayFMODAsset(_screamSound, transform.position);
        }
    }
}