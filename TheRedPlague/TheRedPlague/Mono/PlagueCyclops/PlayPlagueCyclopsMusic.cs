using Nautilus.Utility;
using UnityEngine;

namespace TheRedPlague.Mono.PlagueCyclops;

public class PlayPlagueCyclopsMusic : MonoBehaviour
{
    public float musicDuration = 240;
    
    private float _timeCheckAgain;
    private static float _timeMusicCanPlayAgain;

    private static readonly FMODAsset Music = AudioUtils.GetFmodAsset("VoidIslandMusic");
    
    private void Update()
    {
        if (Time.time < _timeMusicCanPlayAgain) return;

        if (Time.time < _timeCheckAgain) return;
        
        var playerBiome = Player.main.biomeString;
        if (playerBiome.Equals("plaguecyclopsislandinterior"))
        {
            Utils.PlayFMODAsset(Music, MainCamera.camera.transform.position);
            _timeMusicCanPlayAgain = Time.time + musicDuration;
        }
        
        _timeCheckAgain = Time.time + 0.5f;
    }
}