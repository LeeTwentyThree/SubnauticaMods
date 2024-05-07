using Nautilus.Utility;
using UnityEngine;

namespace TheRedPlague.Mono.FleshBlobs;

public class FleshBlobSoundController : MonoBehaviour
{
    private FleshBlobSound[] _sounds = new[]
    {
        new FleshBlobSound(AudioUtils.GetFmodAsset("FleshBlobWalk"), 4f, 7f, 0, 100),
        new FleshBlobSound(AudioUtils.GetFmodAsset("FleshBlobGroan"), 10f, 20f, 100, 2000),
        new FleshBlobSound(AudioUtils.GetFmodAsset("FleshBlobRoar"), 10f, 20f, 0, 100),
        new FleshBlobSound(AudioUtils.GetFmodAsset("FleshBlobAlarm"), 10f, 20f, 200, 250),
    };
    
    private float[] _timeSoundsCanPlayAgain;

    private void Start()
    {
        _timeSoundsCanPlayAgain = new float[_sounds.Length];
        for (var i = 0; i < _sounds.Length; i++)
        {
            _timeSoundsCanPlayAgain[i] = Time.time + Random.Range(_sounds[i].minInterval, _sounds[i].maxInterval);
        }
        Utils.PlayFMODAsset(AudioUtils.GetFmodAsset("FleshBlobActivate"), transform.position);
    }

    private void Update()
    {
        var distanceFromPlayer = Vector3.Distance(MainCamera.camera.transform.position, transform.position);
        for (var i = 0; i < _sounds.Length; i++)
        {
            var snd = _sounds[i];
            if (distanceFromPlayer > snd.minDistance && distanceFromPlayer < snd.maxDistance &&
                Time.time > _timeSoundsCanPlayAgain[i])
            {
                Utils.PlayFMODAsset(snd.asset, transform.position);
                _timeSoundsCanPlayAgain[i] = Time.time + Random.Range(_sounds[i].minInterval, _sounds[i].maxInterval);
            }
        }
    }
}