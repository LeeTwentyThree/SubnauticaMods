using UnityEngine;

namespace TheRedPlague.Mono;

public class PlayRandomSounds : MonoBehaviour
{
    public float minDelay;
    public float maxDelay;

    public FMODAsset asset;

    private float _timePlayAgain;

    private void Update()
    {
        if (Time.time > _timePlayAgain)
        {
            Utils.PlayFMODAsset(asset, transform.position);
            _timePlayAgain = Time.time + Random.Range(minDelay, maxDelay);
        }
    }
}