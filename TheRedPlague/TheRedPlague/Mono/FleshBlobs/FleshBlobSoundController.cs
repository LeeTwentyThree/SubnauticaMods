using Nautilus.Utility;
using UnityEngine;

namespace TheRedPlague.Mono.FleshBlobs;

public class FleshBlobSoundController : MonoBehaviour
{
    private FleshBlobGrowth _growth;
    
    private FleshBlobSound[] _sounds = new[]
    {
        new FleshBlobSound(AudioUtils.GetFmodAsset("FleshBlobWalk"), 4f, 7f, 0, 100, 0f),
        new FleshBlobSound(AudioUtils.GetFmodAsset("FleshBlobGroan"), 10f, 20f, 100, 2000, 0f),
        new FleshBlobSound(AudioUtils.GetFmodAsset("FleshBlobRoar"), 10f, 20f, 0, 100, 0.5f),
        new FleshBlobSound(AudioUtils.GetFmodAsset("FleshBlobAlarm"), 10f, 20f, 200, 250, 0.6f),
    };
    
    private float[] _timeSoundsCanPlayAgain;
    
    private void Start()
    {
        _timeSoundsCanPlayAgain = new float[_sounds.Length];
        for (var i = 0; i < _sounds.Length; i++)
        {
            _timeSoundsCanPlayAgain[i] = Time.time + Random.Range(_sounds[i].minInterval, _sounds[i].maxInterval);
        }

        _growth = GetComponent<FleshBlobGrowth>();
    }

    private void Update()
    {
        var distanceFromPlayer = Vector3.Distance(MainCamera.camera.transform.position, transform.position);
        for (var i = 0; i < _sounds.Length; i++)
        {
            var snd = _sounds[i];
            if (distanceFromPlayer > snd.minDistance && distanceFromPlayer < snd.maxDistance &&
                Time.time > _timeSoundsCanPlayAgain[i] && _growth.Size > snd.minSize)
            {
                Utils.PlayFMODAsset(snd.asset, transform.position);
                _timeSoundsCanPlayAgain[i] = Time.time + Random.Range(_sounds[i].minInterval, _sounds[i].maxInterval);
            }
        }
    }
}